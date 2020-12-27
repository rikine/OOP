#nullable enable

namespace DAL.Staff
{
    public class DALStaff : IStaff
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public IStaff? Manager { get; set; }
    }
}