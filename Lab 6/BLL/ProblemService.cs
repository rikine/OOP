using System.Linq;
using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using BLL.Staff;
using BLL.Task;
using DAL.Problem;
using DAL;
using BLL.Condtion;

namespace BLL
{
    public class ProblemService : IProblemService
    {
        private ProblemRepositoryDAL _problemRepository;

        public ProblemService(ProblemRepositoryDAL problemRepository)
        {
            _problemRepository = problemRepository;
        }

        public Problem MakeProblem(string name, string description, IStaffBLL who)
        {
            var problem = new Problem(name, description, who);
            var problemDAL = problem.MapperToDLL();
            _problemRepository.Add(problemDAL);
            return new Problem(problemDAL);
        }

        public void AddComment(Problem problem, string comment, IStaffBLL who)
        {
            var probleInList = _problemRepository.GetEnumerable().Single(prob => prob.Id == problem.Id);
            if (probleInList == null)
                throw new ProblemNotInListException($"Problem with id = {problem.Id} was not found in service");
            problem.AddComment(comment, who);
            _problemRepository.Replace(problem.MapperToDLL());
        }

        public void ChangeConditionOfProblem(Problem problem, ConditionOfProblem condition, IStaffBLL who)
        {
            var probleInList = _problemRepository.GetEnumerable().Single(prob => prob.Id == problem.Id);
            if (probleInList == null)
                throw new ProblemNotInListException($"Problem with id = {problem.Id} was not found in service");
            problem.ChangeCondition(condition, who);
            _problemRepository.Replace(problem.MapperToDLL());
        }

        public void ChangeStaffForProblem(Problem problem, IStaffBLL staff, IStaffBLL who)
        {
            var probleInList = _problemRepository.GetEnumerable().Single(prob => prob.Id == problem.Id);
            if (probleInList == null)
                throw new ProblemNotInListException($"Problem with id = {problem.Id} was not found in service");
            problem.ChangeStaff(staff, who);
            _problemRepository.Replace(problem.MapperToDLL());
        }

        public Problem FindByTimeOfCreation(DateTime time)
        {
            foreach (var problem in _problemRepository.GetEnumerable())
            {
                if (problem.TimeOfCreation == time)
                    return new Problem(problem);
            }
            throw new ProblemNotFoundException($"Problem with creation time isn't found. Creation Time = {time}");
        }

        public Problem FindByTimeOfLastChange(DateTime time)
        {
            foreach (var problem in _problemRepository.GetEnumerable())
            {
                var problemBLL = new Problem(problem);
                if (problemBLL.TimeOfLastChange() == time)
                    return problemBLL;
            }
            throw new ProblemNotFoundException($"Problem with last change time isn't found. Last change Time = {time}");
        }

        public IEnumerable<Problem> FindProblemOfChangesOfStaff(IStaffBLL staff)
        {
            List<Problem> problems = new List<Problem>();
            foreach (var problem in _problemRepository.GetEnumerable())
            {
                var problemBLL = new Problem(problem);
                if (problemBLL.IsStaffMakeAnyChanges(staff))
                    problems.Add(problemBLL);
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
            foreach (var problem in _problemRepository.GetEnumerable())
            {
                var problemBLL = new Problem(problem);
                if (problemBLL.GetStaff() == staff)
                    problems.Add(problemBLL);
            }
            return problems;
        }

        public IEnumerable<Problem> GetAll()
        {
            var all = _problemRepository.GetEnumerable();
            List<Problem> problems = new List<Problem>();
            foreach (var problem in all)
            {
                problems.Add(new Problem(problem));
            }
            return problems.ToArray();
        }

        public ConditionOfProblem GetConditionOfProblem(Problem problem) => problem.GetCondition();

        public Problem GetProblem(int id) => new Problem(_problemRepository.Get(id));
    }
}