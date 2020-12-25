using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace UL.Staff
{
    public class DirectorUL : AStaff
    {
        public override int? Id { get; set; }
        public override string Name { get; set; }
        public IStaff Manager { get; set; }
        public List<IStaff> Slaves = new List<IStaff>();

        public DirectorUL() { }

        public DirectorUL(string name, IStaff manager, List<IStaff> slaves = null)
        {
            Name = name;
            Manager = manager;
            Slaves = slaves;
        }

        public void AddSlave(IStaff staff) => Slaves.Add(staff);
        public void RemoveSlave(IStaff staff) => Slaves.Remove(staff);

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

        public override string Print(int offset = 0)
        {
            string s = $"\n{new string('\t', offset)}Director\n{new string('\t', offset)} {Name}\n";
            s += $"{new string('\t', offset)}Slaves:";
            foreach (var slave in Slaves)
            {
                s += slave.Print(offset + 1);
            }
            return s;
        }

    }
}