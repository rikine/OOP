#nullable enable
using System.Collections.Generic;

namespace DAL.Staff
{
    public class Director : IStaff
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<IStaff> Slaves = new List<IStaff>();
        public IStaff? Manager { get; set; }

        public void AddSlave(IStaff staff) => Slaves.Add(staff);
        public void RemoveSlave(IStaff staff) => Slaves.Remove(staff);

        public string Print(int offset = 0)
        {
            string s = $"\n{new string('\t', offset)}Director\n{new string('\t', offset)}{Id} {Name}\n";
            s += $"{new string('\t', offset)}Slaves:";
            foreach (var slave in Slaves)
            {
                s += slave.Print(offset + 1);
            }
            return s;
        }

    }
}