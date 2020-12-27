using System;
using System.Collections.Generic;
using DAL.Condition;
using DAL.Staff;

namespace DAL.Reports
{
    public class SprintReport
    {
        public int Id { get; set; }
        public IStaff staff { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public ConditionOfSprint Condition { get; set; }
        public List<(DateTime time, string comments)> Comments = new List<(DateTime time, string comments)>();
        public List<DayReport> DayReports = new List<DayReport>();
    }
}