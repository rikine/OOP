using System.Collections.Generic;
using System.Collections.Immutable;
using BLL.Task;
using System;
using BLL.Condtion;
using UL.Staff;

namespace UL.Report
{
    public class DayReport
    {
        public int Id { get; private set; }
        public IStaff Staff { get; private set; }
        public DateTime TimeOfCreation { get; }
        private List<Problem> _readyProblems = new List<Problem>();
        private List<(DateTime Time, string Comment)> _comments = new List<(DateTime Time, string comment)>();

        public DayReport(IStaff staff)
        {
            Staff = staff;
            TimeOfCreation = DateTime.Now;
        }

        public DayReport(BLL.Reports.DayReport dayReport)
        {
            var convertion = new Convertion.ConvertionULBLL();
            Id = dayReport.Id;
            if (dayReport.Staff as DAL.Staff.DALStaff != null)
                Staff = convertion.ConvertBLLToUL(dayReport.Staff as BLL.Staff.StaffBLL);
            else if (dayReport.Staff as DAL.Staff.Director != null)
                Staff = convertion.ConvertBLLToUL(dayReport.Staff as BLL.Staff.DirectorBLL);
            else
                Staff = convertion.ConvertBLLToUL(dayReport.Staff as BLL.Staff.TeamLeadBLL);
            TimeOfCreation = dayReport.TimeOfCreation;
            foreach (var readyProblem in dayReport.ReadyProblems)
            {
                _readyProblems.Add(readyProblem);
            }
            foreach (var comment in dayReport.Comments)
            {
                _comments.Add(comment);
            }
        }

        public BLL.Reports.DayReport MapperToBLL()
        {
            BLL.Reports.DayReport dayReport = null;
            var convertion = new Convertion.ConvertionULBLL();
            if (Staff as StaffUL != null)
                dayReport = new BLL.Reports.DayReport(convertion.ConvertULToBLL(Staff as StaffUL));
            else if (Staff as DirectorUL != null)
                dayReport = new BLL.Reports.DayReport(convertion.ConvertULToBLL(Staff as DirectorUL));
            else
                dayReport = new BLL.Reports.DayReport(convertion.ConvertULToBLL(Staff as TeamLeadUL));
            dayReport.Id = Id;
            dayReport.TimeOfCreation = TimeOfCreation;
            foreach (var readyProblem in _readyProblems)
            {
                dayReport.ReadyProblems.Add(readyProblem);
            }
            foreach (var comment in _comments)
            {
                dayReport.Comments.Add(comment);
            }
            return dayReport;
        }

        public void AddReadyProblem(Problem problem)
        {
            if (problem.GetCondition() == ConditionOfProblem.Resolved)
                _readyProblems.Add(problem);
            else
                throw new ApplicationException($"Problem can not be added. It's not resolved. Problem id = {problem.Id}");
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public ImmutableList<Problem> GetReadyProblems() => _readyProblems.ToImmutableList();

        public void AddComment(string comment) => _comments.Add((DateTime.Now, comment));

        public void RemoveComment(DateTime time) => _comments.Remove(_comments.Find(comm => comm.Time == time));

        public override string ToString()
        {
            string s = string.Empty;
            s += "DayReport " + Id.ToString() + '\n';
            s += Staff.Name + '\n';
            s += TimeOfCreation.ToString() + '\n';

            if (_readyProblems.Count == 0)
            {
                return s + AllComments();
            }
            return s + AllComments() + '\n' + AllProblems();
        }

        private string AllComments()
        {
            string s = string.Empty;
            foreach (var comment in _comments)
            {
                s += comment.Comment + '\n';
            }
            return s;
        }

        private string AllProblems()
        {
            string s = string.Empty;
            foreach (var problem in _readyProblems)
            {
                s += $"PROBLEM ID = {problem.Id}\n";
                s += "Problem name = " + problem.Name + '\n';
                s += "Problem description = " + problem.Description + '\n';
                s += "Problem timeOfCreation = " + problem.TimeOfCreation.ToString() + '\n';
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