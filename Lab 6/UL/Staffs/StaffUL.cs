namespace UL.Staff
{
    public class StaffUL : AStaff
    {
        public override int? Id { get; set; }
        public override string Name { get; set; }
        public IStaff Manager { get; set; }

        public StaffUL() { }

        public StaffUL(string name, IStaff manager)
        {
            Name = name;
            Manager = manager;
        }

        public override string Print(int offset = 0)
        {
            return $"\n{new string('\t', offset)}Staff\n{new string('\t', offset)} {Name}";
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