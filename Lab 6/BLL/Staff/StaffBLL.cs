#nullable enable
using DAL.Staff;

namespace BLL.Staff
{
    public class StaffBLL : IStaffBLL
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public IStaffBLL? Manager { get; set; }

        public bool EqualsDAL(IStaff staff)
        {
            if (!Id.HasValue && !staff.Id.HasValue && Name == staff.Name)
            {
                return true;
            }
            if (staff.Id == Id && staff.Name == Name && staff as DALStaff != null)
            {
                return true;
            }
            return false;
        }

    }
}