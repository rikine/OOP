using System.Collections.Generic;
using BLL.Staff;

namespace BLL
{
    public interface IEmployeesService<T> where T : IStaffBLL
    {
        IEnumerable<IStaffBLL> GetEnumerable();
        IStaffBLL Get(int id);

        void Add(IStaffBLL person, List<int> managerIds = null);
        void UpdateManager(IStaffBLL person, IStaffBLL managerToChange);
        void Delete(int id);

        TeamLeadBLL GetTreeOfEmployees();
    }
}