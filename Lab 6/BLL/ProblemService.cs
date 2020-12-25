using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using BLL.Staff;
using BLL.Task;

namespace BLL
{
    public class ProblemService : IProblemService
    {
        private List<Problem> _problems = new List<Problem>();

        public Problem MakeProblem(string name, string description, IStaffBLL who)
        {
            var problem = new Problem(_problems.Count + 1, name, description, who);
            _problems.Add(problem);
            return problem;
        }

        public void AddComment(Problem problem, string comment, IStaffBLL who)
        {
            var probleInList = _problems.Find(prob => prob.Id == problem.Id);
            if (probleInList == null)
                throw new ProblemNotInListException($"Problem with id = {problem.Id} was not found in service");
            problem.AddComment(comment, who);
        }

        public void ChangeConditionOfProblem(Problem problem, ConditionOfProblem condition, IStaffBLL who)
        {
            var probleInList = _problems.Find(prob => prob.Id == problem.Id);
            if (probleInList == null)
                throw new ProblemNotInListException($"Problem with id = {problem.Id} was not found in service");
            problem.ChangeCondition(condition, who);
        }

        public void ChangeStaffForProblem(Problem problem, IStaffBLL staff, IStaffBLL who)
        {
            var probleInList = _problems.Find(prob => prob.Id == problem.Id);
            if (probleInList == null)
                throw new ProblemNotInListException($"Problem with id = {problem.Id} was not found in service");
            problem.ChangeStaff(staff, who);
        }

        public Problem FindByTimeOfCreation(DateTime time)
        {
            foreach (var problem in _problems)
            {
                if (problem.TimeOfCreation == time)
                    return problem;
            }
            throw new ProblemNotFoundException($"Problem with creation time isn't found. Creation Time = {time}");
        }

        public Problem FindByTimeOfLastChange(DateTime time)
        {
            foreach (var problem in _problems)
            {
                if (problem.TimeOfLastChange() == time)
                    return problem;
            }
            throw new ProblemNotFoundException($"Problem with last change time isn't found. Last change Time = {time}");
        }

        public IEnumerable<Problem> FindProblemOfChangesOfStaff(IStaffBLL staff)
        {
            List<Problem> problems = new List<Problem>();
            foreach (var problem in _problems)
            {
                if (problem.IsStaffMakeAnyChanges(staff))
                    problems.Add(problem);
            }
            return problems;
        }

        public IEnumerable<Problem> FindProblemOfSlavesOfStaff(IStaffBLL manager)
        {
            List<Problem> problems = new List<Problem>();
            if (manager as TeamLeadBLL != null)
                FindProblemOfSlavesOfStaff(manager as TeamLeadBLL, problems);
            else if (manager as DirectorBLL != null)
                FindProblemOfSlavesOfStaff(manager as DirectorBLL, problems);
            else
                throw new NoSlavesFromStaffException($"Staff with name = {manager.Name} can't have slaves");
            return problems;
        }

        private void FindProblemOfSlavesOfStaff(TeamLeadBLL teamLead, List<Problem> problems)
        {
            foreach (var slave in teamLead.Slaves)
            {
                problems.AddRange(FindProblemOfStaff(slave));
                if (slave as DirectorBLL != null)
                    FindProblemOfSlavesOfStaff(slave as DirectorBLL, problems);
            }
        }

        private void FindProblemOfSlavesOfStaff(DirectorBLL director, List<Problem> problems)
        {
            foreach (var slave in director.Slaves)
            {
                problems.AddRange(FindProblemOfStaff(slave));
                if (slave as DirectorBLL != null)
                    FindProblemOfSlavesOfStaff(slave as DirectorBLL, problems);
            }
        }

        public IEnumerable<Problem> FindProblemOfStaff(IStaffBLL staff)
        {
            List<Problem> problems = new List<Problem>();
            foreach (var problem in _problems)
            {
                if (problem.GetStaff() == staff)
                    problems.Add(problem);
            }
            return problems;
        }

        public IEnumerable<Problem> GetAll() => _problems.ToImmutableArray();

        public ConditionOfProblem GetConditionOfProblem(Problem problem) => problem.GetCondition();

        public Problem GetProblem(int id) => _problems.Find(problem => problem.Id == id);
    }
}