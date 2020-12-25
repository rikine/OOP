using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Staff;
using BLL.Task;
using UL.Report;

namespace UL
{
    public abstract class AStaff : IStaff
    {
        public abstract int? Id { get; set; }
        public abstract string Name { get; set; }
        public abstract bool EqualsBLL(IStaffBLL staff);
        public abstract string Print(int offset);

        private List<DayReport> _dayReports = new List<DayReport>();
        private SprintReport _sprintReport;

        public void AddCommentToReport(string comment)
        {
            if (_dayReports.Count == 0 || _dayReports.Last().TimeOfCreation.Day != DateTime.Now.Day)
                _dayReports.Add(new DayReport());
            _dayReports.Last().AddComment(comment);
        }

        public void AddReadyProblem(Problem problem)
        {
            if (_dayReports.Count == 0 || _dayReports.Last().TimeOfCreation.Day != DateTime.Now.Day)
                _dayReports.Add(new DayReport());
            _dayReports.Last().AddReadyProblem(problem);
        }

        public void RemoveComment(DateTime time)
        {
            if (_dayReports.Count == 0 || _dayReports.Last().TimeOfCreation.Day != DateTime.Now.Day)
                _dayReports.Add(new DayReport());
            _dayReports.Last().RemoveComment(time);
        }

        public ImmutableList<DayReport> GetDayReports() => _dayReports.ToImmutableList();

        public SprintReport GetSprintReport() => _sprintReport;

        public void MakeSprintReport(string defaultDraftOfSprintReport)
        {
            if (_sprintReport == null || _sprintReport != null && _sprintReport.Condition == ConditionOfSprint.Open)
                _sprintReport = new SprintReport(defaultDraftOfSprintReport, _dayReports);
        }

        public void ClearAfterSprint()
        {
            _sprintReport = null;
            _dayReports.Clear();
        }
        
    }
}