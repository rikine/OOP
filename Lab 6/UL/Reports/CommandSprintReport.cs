using System.Collections.Immutable;
using System.Text;
using System;
using System.Collections.Generic;

namespace UL.Report
{
    public class CommandSprintReport
    {
        public int Id { get; private set; }
        public ConditionOfSprint Condition { get; private set; }
        public DateTime TimeOfCreation { get; }
        List<SprintReport> _sprintReports = new List<SprintReport>();
        public string Report { get; private set; } = string.Empty;

        public CommandSprintReport()
        {
            TimeOfCreation = DateTime.Now;
            Condition = ConditionOfSprint.Open;
        }

        public CommandSprintReport(BLL.Reports.CommandSprintReport report)
        {
            Id = report.Id;
            Condition = (ConditionOfSprint)report.Condition;
            TimeOfCreation = report.TimeOfCreation;
            foreach (var rep in report.SprintReports)
            {
                _sprintReports.Add(new SprintReport(rep));
            }
            Report = report.Report;
        }

        public BLL.Reports.CommandSprintReport MapperToBLL()
        {
            var reportDAL = new BLL.Reports.CommandSprintReport();
            reportDAL.Id = Id;
            reportDAL.Condition = (BLL.Condtion.ConditionOfSprint)this.Condition;
            reportDAL.TimeOfCreation = TimeOfCreation;
            foreach (var rep in _sprintReports)
            {
                reportDAL.SprintReports.Add(rep.MapperToBLL());
            }
            reportDAL.Report = Report;
            return reportDAL;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void AddSprintReport(SprintReport sprint)
        {
            if (Condition == ConditionOfSprint.Closed)
                throw new ApplicationException("CommandSprintReport sprint is closed");
            if (sprint.Condition == ConditionOfSprint.Closed)
                _sprintReports.Add(sprint);
            else
                throw new ApplicationException("Can not add not ready sprint in command report");
        }

        public ImmutableList<SprintReport> GetSprintReports()
        {
            return _sprintReports.ToImmutableList();
        }

        public void CloseCommandReport()
        {
            Condition = ConditionOfSprint.Closed;
        }

        public void SetReport(string s)
        {
            if (Report == string.Empty)
            {
                Report = s;
            }
        }

    }
}