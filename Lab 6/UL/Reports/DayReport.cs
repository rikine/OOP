using System.Collections.Generic;
using System.Collections.Immutable;
using BLL.Task;
using System;

namespace UL.Report
{
    public class DayReport
    {
        public DateTime TimeOfCreation { get; }
        public DayReport() => TimeOfCreation = DateTime.Now;

        private List<Problem> _readyProblems = new List<Problem>();
        private List<(string comment, DateTime time)> _comments = new List<(string comment, DateTime time)>();

        public void AddReadyProblem(Problem problem)
        {
            if (problem.GetCondition() == ConditionOfProblem.Resolved)
                _readyProblems.Add(problem);
            else
                throw new ApplicationException($"Problem can not be added. It's not resolved. Problem id = {problem.Id}");
        }

        public ImmutableList<Problem> GetReadyProblems() => _readyProblems.ToImmutableList();

        public void AddComment(string comment) => _comments.Add((comment, DateTime.Now));

        public void RemoveComment(DateTime time) => _comments.Remove(_comments.Find(comm => comm.time == time));

        public override string ToString()
        {
            if (_readyProblems.Count == 0)
            {
                return AllComments();
            }
            return AllComments() + '\n' + AllProblems();
        }

        private string AllComments()
        {
            string s = string.Empty;
            foreach (var comment in _comments)
            {
                s += comment.comment + '\n';
            }
            return s;
        }

        private string AllProblems()
        {
            string s = string.Empty;
            foreach (var problem in _readyProblems)
            {
                s += $"PROBLEM ID = {problem.Id}\n";
                s += ChangesOfStaff(problem.AllChangesOfStaffByStaff(problem.GetStaff()));
                s += ChangesOfConditions(problem.AllChangesOfConditionByStaff(problem.GetStaff()));
                s += ChangesOfComments(problem.AllChangesOfCommentByStaff(problem.GetStaff()));
                s += '\n';
            }
            return s;
        }

        private string ChangesOfConditions(List<ChangeCondition> changeConditions)
        {
            if (changeConditions.Count == 0)
                return string.Empty;
            string s = string.Empty;
            foreach (var change in changeConditions)
            {
                if (change.Time.Year == TimeOfCreation.Year && change.Time.DayOfYear == TimeOfCreation.DayOfYear)
                    s += $"Condition changed by {change.Who.Name} at {change.Time}: new value = {ConvertEnumToString(change.NewCondition)}.\n";
            }
            return s;
        }

        private string ChangesOfStaff(List<ChangeStaff> changeStaffs)
        {
            if (changeStaffs.Count == 0)
                return string.Empty;
            string s = string.Empty;
            foreach (var change in changeStaffs)
            {
                if (change.Time.Year == TimeOfCreation.Year && change.Time.DayOfYear == TimeOfCreation.DayOfYear)
                    s += $"Staff changed by {change.Who.Name} at {change.Time}: new value = {change.NewStaff}.\n";
            }
            return s;
        }

        private string ChangesOfComments(List<ChangeComment> changeComments)
        {
            if (changeComments.Count == 0)
                return string.Empty;
            string s = string.Empty;
            foreach (var change in changeComments)
            {
                if (change.Time.Year == TimeOfCreation.Year && change.Time.DayOfYear == TimeOfCreation.DayOfYear)
                    s += $"Comment from {change.Who.Name} at {change.Time}: {change.NewComment}\n";
            }
            return s;
        }

        private string ConvertEnumToString(ConditionOfProblem condition)
        {
            switch (condition)
            {
                case ConditionOfProblem.Active:
                    return "Active";
                case ConditionOfProblem.Open:
                    return "Open";
                case ConditionOfProblem.Resolved:
                    return "Resolved";
                default:
                    return string.Empty;
            }
        }

    }
}