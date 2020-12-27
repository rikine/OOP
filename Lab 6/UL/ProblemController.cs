using System;
using System.Collections.Generic;
using BLL.Task;
using BLL;
using UL.Convertion;
using UL.Staff;
using BLL.Condtion;

namespace UL
{
    public class ProblemController
    {
        ConvertionULBLL convertion = new ConvertionULBLL();
        IProblemService _problemService;

        public ProblemController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        public Problem MakeProblem(string name, string description, IStaff who)
        {
            if (who as StaffUL != null)
                return _problemService.MakeProblem(name, description, convertion.ConvertULToBLL(who as StaffUL));
            else if (who as DirectorUL != null)
                return _problemService.MakeProblem(name, description, convertion.ConvertULToBLL(who as DirectorUL));
            else
                return _problemService.MakeProblem(name, description, convertion.ConvertULToBLL(who as TeamLeadUL));
        }

        public IEnumerable<Problem> GetAll()
        {
            return _problemService.GetAll();
        }
        public Problem GetProblem(int id)
        {
            return _problemService.GetProblem(id);
        }
        public Problem FindByTimeOfCreation(DateTime time)
        {
            return _problemService.FindByTimeOfCreation(time);
        }
        public Problem FindByTimeOfLastChange(DateTime time)
        {
            return _problemService.FindByTimeOfLastChange(time);
        }
        public IEnumerable<Problem> FindProblemOfStaff(IStaff staff)
        {
            if (staff as StaffUL != null)
                return _problemService.FindProblemOfStaff(convertion.ConvertULToBLL(staff as StaffUL));
            else if (staff as DirectorUL != null)
                return _problemService.FindProblemOfStaff(convertion.ConvertULToBLL(staff as DirectorUL));
            else
                return _problemService.FindProblemOfStaff(convertion.ConvertULToBLL(staff as TeamLeadUL));
        }
        public IEnumerable<Problem> FindProblemOfChangesOfStaff(IStaff staff)
        {
            if (staff as StaffUL != null)
                return _problemService.FindProblemOfChangesOfStaff(convertion.ConvertULToBLL(staff as StaffUL));
            else if (staff as DirectorUL != null)
                return _problemService.FindProblemOfChangesOfStaff(convertion.ConvertULToBLL(staff as DirectorUL));
            else
                return _problemService.FindProblemOfChangesOfStaff(convertion.ConvertULToBLL(staff as TeamLeadUL));
        }
        public ConditionOfProblem GetConditionOfProblem(Problem problem)
        {
            return _problemService.GetConditionOfProblem(problem);
        }
        public void ChangeConditionOfProblem(Problem problem, ConditionOfProblem condition, IStaff who)
        {
            if (who as StaffUL != null)
                _problemService.ChangeConditionOfProblem(problem, condition, convertion.ConvertULToBLL(who as StaffUL));
            else if (who as DirectorUL != null)
                _problemService.ChangeConditionOfProblem(problem, condition, convertion.ConvertULToBLL(who as DirectorUL));
            else
                _problemService.ChangeConditionOfProblem(problem, condition, convertion.ConvertULToBLL(who as TeamLeadUL));
        }
        public void AddComment(Problem problem, string comment, IStaff who)
        {
            if (who as StaffUL != null)
                _problemService.AddComment(problem, comment, convertion.ConvertULToBLL(who as StaffUL));
            else if (who as DirectorUL != null)
                _problemService.AddComment(problem, comment, convertion.ConvertULToBLL(who as DirectorUL));
            else
                _problemService.AddComment(problem, comment, convertion.ConvertULToBLL(who as TeamLeadUL));
        }
        public void ChangeStaffForProblem(Problem problem, IStaff staff, IStaff who)
        {
            if (staff as StaffUL != null)
            {
                if (who as StaffUL != null)
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as StaffUL), convertion.ConvertULToBLL(who as StaffUL));
                else if (who as DirectorUL != null)
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as StaffUL), convertion.ConvertULToBLL(who as DirectorUL));
                else
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as StaffUL), convertion.ConvertULToBLL(who as TeamLeadUL));
            }
            else if (staff as DirectorUL != null)
            {
                if (who as StaffUL != null)
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as DirectorUL), convertion.ConvertULToBLL(who as StaffUL));
                else if (who as DirectorUL != null)
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as DirectorUL), convertion.ConvertULToBLL(who as DirectorUL));
                else
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as DirectorUL), convertion.ConvertULToBLL(who as TeamLeadUL));
            }
            else
            {
                if (who as StaffUL != null)
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as TeamLeadUL), convertion.ConvertULToBLL(who as StaffUL));
                else if (who as DirectorUL != null)
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as TeamLeadUL), convertion.ConvertULToBLL(who as DirectorUL));
                else
                    _problemService.ChangeStaffForProblem(problem, convertion.ConvertULToBLL(staff as TeamLeadUL), convertion.ConvertULToBLL(who as TeamLeadUL));
            }
        }
        public IEnumerable<Problem> FindProblemOfSlavesOfStaff(IStaff manager)
        {
            if (manager as StaffUL != null)
                return _problemService.FindProblemOfSlavesOfStaff(convertion.ConvertULToBLL(manager as StaffUL));
            else if (manager as DirectorUL != null)
                return _problemService.FindProblemOfSlavesOfStaff(convertion.ConvertULToBLL(manager as DirectorUL));
            else
                return _problemService.FindProblemOfSlavesOfStaff(convertion.ConvertULToBLL(manager as TeamLeadUL));
        }
    }
}