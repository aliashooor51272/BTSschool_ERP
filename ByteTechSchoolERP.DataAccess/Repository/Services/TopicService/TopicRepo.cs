using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ITopic;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Topic;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.TopicService
{
    public class TopicRepo : GenericRepository<Topic>, ITopic
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TopicRepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> AddOrUpdateTopic(Topic topic)
        {
            var responseModel = new ResponseModel();

            try
            {
                var existingTopic = await _context.Topics.FirstOrDefaultAsync(c => c.Id == topic.Id);
                if (existingTopic != null)
                {
                    existingTopic.ClassId = topic.ClassId;
                    existingTopic.SectionId = topic.SectionId;
                    existingTopic.SubjectId = topic.SubjectId;
                    existingTopic.LessonId = topic.LessonId;
                    existingTopic.TopicName = topic.TopicName;
                    existingTopic.Id = topic.Id;

                    _context.Topics.Update(existingTopic);
                    responseModel.Message = "Topic updated successfully.";
                }
                else
                {
                    await _context.Topics.AddAsync(topic);
                    responseModel.Message = "Topic added successfully.";
                }

                await _context.SaveChangesAsync();
                responseModel.isSuccess = true;
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"Error: {ex.Message}";
            }

            return responseModel;
        }

        public List<AddTopicViewModel> GetTopicList()
        {
            return _context.Topics
                           .Select(c => new AddTopicViewModel
                           {
                               ClassId = c.ClassId,
                               SectionId = c.SectionId,
                               SubjectId = c.SubjectId,
                               LessonId = c.LessonId,
                           Id=c.Id,
                               Topic = c.TopicName,
                           }).ToList();
        }
    }
}
