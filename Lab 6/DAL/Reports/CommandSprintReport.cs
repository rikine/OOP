using System;
using System.Collections.Generic;
using DAL.Condition;

namespace DAL.Reports
{
    public class CommandSprintReport
    {
        public int Id { get; set; }
        public ConditionOfSprint Condition { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public List<SprintReport> SprintReports = new List<SprintReport>();
        public string Report { get; set; } = string.Empty;
    }
}