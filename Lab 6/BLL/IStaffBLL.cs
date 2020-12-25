#nullable enable
using DAL.Staff;

namespace BLL.Staff
{
    public interface IStaffBLL
    {
        int? Id { get; }
        string? Name { get; }
        string Print(int offset);

        bool EqualsDAL(IStaff staff);
    }
}