using System.Collections.Generic;

namespace UL.Staff
{
    public class TeamLeadUL : AStaff
    {
        public override int? Id { get; set; }
        public override string Name { get; set; }
        public List<IStaff> Slaves = new List<IStaff>();

        public TeamLeadUL() { }

        public TeamLeadUL(string name, List<IStaff> slaves = null)
        {
            Name = name;
            Slaves = slaves;
        }

        public void AddSlave(IStaff staff) => Slaves.Add(staff);
        public void RemoveSlave(IStaff staff) => Slaves.Remove(staff);

        public override string Print(int offset = 0)
        {
            string s = $"\nTeamLead\n {Name}\n";
            s += "Slaves:\t\n";
            foreach (var slave in Slaves)
            {
                s += slave.Print(offset + 1);
            }
            return s;
        }

        public override bool EqualsBLL(BLL.Staff.IStaffBLL staff)
        {
            if (!Id.HasValue && !staff.Id.HasValue && Name == staff.Name)
            {
                return true;
            }
            if (staff.Id == Id && staff.Name == Name && staff as BLL.Staff.IStaffBLL != null)
            {
                return true;
            }
            return false;
        }

    }
}