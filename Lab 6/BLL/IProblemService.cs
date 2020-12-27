using System;
using BLL.Task;
using BLL.Staff;
using System.Collections.Generic;
using BLL.Condtion;
using System.Collections.Immutable;

namespace BLL
{
    public interface IProblemService
    {
        Problem MakeProblem(string name, string description, IStaffBLL who);
        IEnumerable<Problem> GetAll();
        Problem GetProblem(int id);
        Problem FindByTimeOfCreation(DateTime time);
        Problem FindByTimeOfLastChange(DateTime time);
        IEnumerable<Problem> FindProblemOfStaff(IStaffBLL staff);
        IEnumerable<Problem> FindProblemOfChangesOfStaff(IStaffBLL staff);
        ConditionOfProblem GetConditionOfProblem(Problem problem);
        void ChangeConditionOfProblem(Problem problem, ConditionOfProblem condition, IStaffBLL who);
        void AddComment(Problem problem, string comment, IStaffBLL who);
        void ChangeStaffForProblem(Problem problem, IStaffBLL staff, IStaffBLL who);
        IEnumerable<Problem> FindProblemOfSlavesOfStaff(IStaffBLL manager);
    }
}