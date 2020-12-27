using System;
using System.Collections.Generic;
using BLL.Condtion;

namespace BLL.Reports
{
    public class CommandSprintReport
    {
        public int Id { get; set; }
        public ConditionOfSprint Condition { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public List<SprintReport> SprintReports = new List<SprintReport>();
        public string Report { get; set; } = string.Empty;

        public CommandSprintReport() { }

        public CommandSprintReport(DAL.Reports.CommandSprintReport report)
        {
            Id = report.Id;
            Condition = (ConditionOfSprint)report.Condition;
            TimeOfCreation = report.TimeOfCreation;
            foreach (var rep in report.SprintReports)
            {
                SprintReports.Add(new SprintReport(rep));
            }
            Report = report.Report;
        }

        public DAL.Reports.CommandSprintReport MapperToDAL()
        {
            var reportDAL = new DAL.Reports.CommandSprintReport();
            reportDAL.Id = Id;
            reportDAL.Condition = (DAL.Condition.ConditionOfSprint)this.Condition;
            reportDAL.TimeOfCreation = TimeOfCreation;
            foreach (var rep in SprintReports)
            {
                reportDAL.SprintReports.Add(rep.MapperToDAL());
            }
            reportDAL.Report = Report;
            return reportDAL;
        }

    }
}