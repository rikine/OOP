#nullable enable
using System.Collections.Generic;

namespace DAL.Staff
{
    public class TeamLead : IStaff
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<IStaff> Slaves = new List<IStaff>();

        public void AddSlave(IStaff staff) => Slaves.Add(staff);
        public void RemoveSlave(IStaff staff) => Slaves.Remove(staff);

        public string Print(int offset = 0)
        {
            string s = $"\nTeamLead\n{Id} {Name}\n";
            s += "Slaves:\t\n";
            foreach (var slave in Slaves)
            {
                s += slave.Print(offset + 1);
            }
            return s;
        }

    }
}