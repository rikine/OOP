using System;
using System.Collections.Generic;
using DAL.Staff;
using DAL.Problem;

namespace DAL.Reports
{
    public class DayReport
    {
        public int Id { get; set; }
        public IStaff staff { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public List<ProblemDAL> ReadyProblems = new List<ProblemDAL>();
        public List<(DateTime Time, string comment)> Comments = new List<(DateTime Time, string comment)>();
    }
}