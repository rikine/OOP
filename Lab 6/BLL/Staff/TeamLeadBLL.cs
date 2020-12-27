#nullable enable
using System.Collections.Generic;
using DAL.Staff;

namespace BLL.Staff
{
    public class TeamLeadBLL : IStaffBLL
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<IStaffBLL> Slaves = new List<IStaffBLL>();

        public void AddSlave(IStaffBLL staff) => Slaves.Add(staff);
        public void RemoveSlave(IStaffBLL staff) => Slaves.Remove(staff);

        public bool EqualsDAL(IStaff staff)
        {
            if (!Id.HasValue && !staff.Id.HasValue && Name == staff.Name)
            {
                return true;
            }
            if (staff.Id == Id && staff.Name == Name && staff as TeamLead != null)
            {
                return true;
            }
            return false;
        }
    }
}