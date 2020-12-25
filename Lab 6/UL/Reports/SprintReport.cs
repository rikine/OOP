using System;
using System.Collections.Generic;
using System.Text;

namespace UL.Report
{
    public class SprintReport
    {
        public DateTime TimeOfCreation { get; }
        public ConditionOfSprint Condition { get; private set; }
        private StringBuilder _readyReport;

        private List<(string comments, DateTime time)> comments = new List<(string comments, DateTime time)>();

        private List<DayReport> _dayReports;

        public SprintReport(string draftOfSprint, List<DayReport> dayReports)
        {
            Condition = ConditionOfSprint.Open;
            _readyReport = new StringBuilder();
            _readyReport.Append(draftOfSprint);
            TimeOfCreation = DateTime.Now;
            _dayReports = dayReports;
        }

        public void AddComment(string comment)
        {
            comments.Add((comment, DateTime.Now));
        }

        public void SetConditionOfSprint(ConditionOfSprint condition)
        {
            if (Condition == ConditionOfSprint.Closed)
                throw new ApplicationException("Can not change condition of Sprint Report. It's closed");
            if (condition != ConditionOfSprint.Closed)
                throw new ApplicationException("Can not set open condition of Sprint Report. It's open");
            Condition = condition;
            CreateReport(_dayReports);
        }

        public void RemoveComment(DateTime time)
        {
            comments.Remove(comments.Find(comm => comm.time == time));
        }

        private void CreateReport(List<DayReport> dayReports)
        {
            foreach (var report in dayReports)
            {
                _readyReport.Append(report.ToString());
            }

            foreach (var comm in comments)
            {
                _readyReport.Append($"Comment was added at {comm.time}: {comm.comments}");
            }
        }
    }
}