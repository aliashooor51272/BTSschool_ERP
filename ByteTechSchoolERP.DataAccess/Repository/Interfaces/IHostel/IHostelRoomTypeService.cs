using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IHostel
{
    public interface IHostelRoomTypeService
    {
        Task<ResponseModel> AddHostelRoomType(HostelRoomType hostelRoomType);
        Task<ResponseModel> DeleteRoomTypeById(Guid id);


    }
}
