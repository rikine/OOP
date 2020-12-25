#nullable enable

namespace DAL.Staff
{
    public class DALStaff : IStaff
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public IStaff? Manager { get; set; }

        public string Print(int offset = 0)
        {
            return $"\n{new string('\t', offset)}Staff\n{new string('\t', offset)}{Id} {Name}";
        }
    }
}