using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Topic;
using ByteTechSchoolERP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.ITopic
{
  
        public interface ITopic
        {
            Task<ResponseModel> AddOrUpdateTopic(Topic topic);
        List<AddTopicViewModel> GetTopicList();
    }
    
}
