using System.Collections.Generic;
using System.Linq;
using BLL.Staff;
using DAL;
using BLL.Convertion;

namespace BLL
{
    public class EmployeesService : IEmployeesService<IStaffBLL>
    {
        IEmployees<DAL.Staff.IStaff> _employees;
        ConvertionDALBLL _convertion;

        public EmployeesService(IEmployees<DAL.Staff.IStaff> employees)
        {
            _employees = employees;
            _convertion = new ConvertionDALBLL();
        }

        public void Add(IStaffBLL person, List<int> managerIds = null)
        {
            if (person as TeamLeadBLL != null)
            {
                var teamLeadDAL = _convertion.ConvertBLLToDAL(person as TeamLeadBLL);
                _employees.Add(teamLeadDAL);
                (person as TeamLeadBLL).Id = teamLeadDAL.Id.Value;
            }
            else if (person as DirectorBLL != null)
            {
                var directorDAL = _convertion.ConvertBLLToDAL(person as DirectorBLL);
                _employees.Add(directorDAL, managerIds);
                (person as DirectorBLL).Id = directorDAL.Id.Value;
            }
            else
            {
                var staffDAL = _convertion.ConvertBLLToDAL(person as StaffBLL);
                _employees.Add(staffDAL);
                (person as StaffBLL).Id = staffDAL.Id.Value;
            }
        }

        public void Delete(int id)
        {
            _employees.Delete(id);
        }

        public IStaffBLL Get(int id)
        {
            var personDAL = _employees.Get(id);
            if (personDAL as DAL.Staff.TeamLead != null)
                return _convertion.ConvertDALToBLL(personDAL as DAL.Staff.TeamLead);
            else if (personDAL as DAL.Staff.Director != null)
                return _convertion.ConvertDALToBLL(personDAL as DAL.Staff.Director);
            else
                return _convertion.ConvertDALToBLL(personDAL as DAL.Staff.DALStaff);
        }

        public IEnumerable<IStaffBLL> GetEnumerable()
        {
            var allDAL = _employees.GetEnumerable();
            var allBLL = new List<IStaffBLL>();
            foreach (var person in allDAL)
            {
                if (person as DAL.Staff.DALStaff != null)
                    allBLL.Add(_convertion.ConvertDALToBLL(person as DAL.Staff.DALStaff));
                else if (person as DAL.Staff.Director != null)
                    allBLL.Add(_convertion.ConvertDALToBLL(person as DAL.Staff.Director));
                else
                    allBLL.Add(_convertion.ConvertDALToBLL(person as DAL.Staff.TeamLead));
            }
            return allBLL.ToArray();
        }

        public TeamLeadBLL GetTreeOfEmployees()
        {
            return _convertion.ConvertDALToBLL(_employees.GetTreeOfEmployees());
        }

        public void UpdateManager(IStaffBLL person, IStaffBLL managerToChange)
        {
            if (person as StaffBLL != null && managerToChange as DirectorBLL != null)
                _employees.UpdateManager(_convertion.ConvertBLLToDAL(person as StaffBLL), _convertion.ConvertBLLToDAL(managerToChange as DirectorBLL));
            else if (person as StaffBLL != null && managerToChange as TeamLeadBLL != null)
                _employees.UpdateManager(_convertion.ConvertBLLToDAL(person as StaffBLL), _convertion.ConvertBLLToDAL(managerToChange as TeamLeadBLL));
            else if (person as DirectorBLL != null && managerToChange as DirectorBLL != null)
                _employees.UpdateManager(_convertion.ConvertBLLToDAL(person as DirectorBLL), _convertion.ConvertBLLToDAL(managerToChange as DirectorBLL));
            else if (person as DirectorBLL != null && managerToChange as TeamLeadBLL != null)
                _employees.UpdateManager(_convertion.ConvertBLLToDAL(person as DirectorBLL), _convertion.ConvertBLLToDAL(managerToChange as TeamLeadBLL));
            else
                throw new UpdatePersonException($"Can not change manager at person id = {person.Id}");
        }

    }
}