using System.Resources;
using System.Collections.Generic;
using UL.Staff;
using BLL;
using UL.Convertion;

namespace UL
{
    public class EmployeeController
    {
        IEmployeesService<BLL.Staff.IStaffBLL> _employeesService;
        ConvertionULBLL _convertion;

        public EmployeeController(IEmployeesService<BLL.Staff.IStaffBLL> employeesService)
        {
            _employeesService = employeesService;
            _convertion = new ConvertionULBLL();
        }

        public void Add(IStaff person)
        {
            if (person as TeamLeadUL != null)
            {
                var teamLeadBLL = _convertion.ConvertULToBLL(person as TeamLeadUL);
                _employeesService.Add(teamLeadBLL);
                (person as TeamLeadUL).Id = teamLeadBLL.Id.Value;
            }
            else if (person as DirectorUL != null)
            {
                var managerIds = new List<int>();
                foreach (var slave in (person as DirectorUL).Slaves)
                {
                    if (slave as DirectorUL != null)
                        managerIds.Add((slave as DirectorUL).Manager.Id.Value);
                    else
                        managerIds.Add((slave as StaffUL).Manager.Id.Value);
                }
                var directorBLL = _convertion.ConvertULToBLL(person as DirectorUL);
                _employeesService.Add(directorBLL, managerIds);
                (person as DirectorUL).Id = directorBLL.Id.Value;
            }
            else
            {
                var staffBLL = _convertion.ConvertULToBLL(person as StaffUL);
                _employeesService.Add(staffBLL);
                (person as StaffUL).Id = staffBLL.Id.Value;
            }
        }

        public void Delete(int id)
        {
            _employeesService.Delete(id);
        }

        public IStaff Get(int id)
        {
            var personBLL = _employeesService.Get(id);
            if (personBLL as BLL.Staff.TeamLeadBLL != null)
                return _convertion.ConvertBLLToUL(personBLL as BLL.Staff.TeamLeadBLL);
            else if (personBLL as BLL.Staff.DirectorBLL != null)
                return _convertion.ConvertBLLToUL(personBLL as BLL.Staff.DirectorBLL);
            else
                return _convertion.ConvertBLLToUL(personBLL as BLL.Staff.StaffBLL);
        }

        public IEnumerable<IStaff> GetEnumerable()
        {
            var allBLL = _employeesService.GetEnumerable();
            var allUL = new List<IStaff>();
            foreach (var person in allBLL)
            {
                if (person as BLL.Staff.StaffBLL != null)
                    allUL.Add(_convertion.ConvertBLLToUL(person as BLL.Staff.StaffBLL));
                else if (person as BLL.Staff.DirectorBLL != null)
                    allUL.Add(_convertion.ConvertBLLToUL(person as BLL.Staff.DirectorBLL));
                else
                    allUL.Add(_convertion.ConvertBLLToUL(person as BLL.Staff.TeamLeadBLL));
            }
            return allUL.ToArray();
        }

        public TeamLeadUL GetTreeOfEmployees()
        {
            return _convertion.ConvertBLLToUL(_employeesService.GetTreeOfEmployees());
        }

        public void UpdateManager(IStaff person, IStaff managerToChange)
        {
            if (person as StaffUL != null && managerToChange as DirectorUL != null)
                _employeesService.UpdateManager(_convertion.ConvertULToBLL(person as StaffUL), _convertion.ConvertULToBLL(managerToChange as DirectorUL));
            else if (person as StaffUL != null && managerToChange as TeamLeadUL != null)
                _employeesService.UpdateManager(_convertion.ConvertULToBLL(person as StaffUL), _convertion.ConvertULToBLL(managerToChange as TeamLeadUL));
            else if (person as DirectorUL != null && managerToChange as DirectorUL != null)
                _employeesService.UpdateManager(_convertion.ConvertULToBLL(person as DirectorUL), _convertion.ConvertULToBLL(managerToChange as DirectorUL));
            else if (person as DirectorUL != null && managerToChange as TeamLeadUL != null)
                _employeesService.UpdateManager(_convertion.ConvertULToBLL(person as DirectorUL), _convertion.ConvertULToBLL(managerToChange as TeamLeadUL));
            else
                throw new UpdatePersonException($"Can not change manager at person name = {person.Name}");
        }

    }
}