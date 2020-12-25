using System;
using System.Collections.Generic;
using DAL.Staff;

namespace DAL
{
    public interface IEmployees<T> where T : IStaff
    {
        IEnumerable<T> GetEnumerable();
        T Get(int id);

        void Add(T person);
        void Add(T person, List<int> managerIds);
        void UpdateManager(T person, T managerToChange);
        void Delete(int id);

        TeamLead GetTreeOfEmployees();
    }
}