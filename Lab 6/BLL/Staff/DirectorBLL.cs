#nullable enable
using System.Collections.Generic;
using DAL.Staff;

namespace BLL.Staff
{
    public class DirectorBLL : IStaffBLL
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<IStaffBLL> Slaves = new List<IStaffBLL>();
        public IStaffBLL? Manager { get; set; }

        public void AddSlave(IStaffBLL staff) => Slaves.Add(staff);
        public void RemoveSlave(IStaffBLL staff) => Slaves.Remove(staff);

        public bool EqualsDAL(IStaff staff)
        {
            if (!Id.HasValue && !staff.Id.HasValue && Name == staff.Name)
            {
                return true;
            }
            if (staff.Id == Id && staff.Name == Name && staff as Director != null)
            {
                return true;
            }
            return false;
        }
        
    }
}