using System;
using System.Collections.Generic;
using BLL.Task;
using BLL.Staff;

namespace BLL.Reports
{
    public class DayReport
    {
        public int Id { get; set; }
        public IStaffBLL Staff { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public List<Problem> ReadyProblems = new List<Problem>();
        public List<(DateTime Time, string comment)> Comments = new List<(DateTime Time, string comment)>();

        public DayReport(IStaffBLL staff)
        {
            Staff = staff;
            TimeOfCreation = DateTime.Now;
        }

        public DayReport(DAL.Reports.DayReport dayReport)
        {
            var convertion = new Convertion.ConvertionDALBLL();
            Id = dayReport.Id;
            if (dayReport.staff as DAL.Staff.DALStaff != null)
                Staff = convertion.ConvertDALToBLL(dayReport.staff as DAL.Staff.DALStaff);
            else if (dayReport.staff as DAL.Staff.Director != null)
                Staff = convertion.ConvertDALToBLL(dayReport.staff as DAL.Staff.Director);
            else
                Staff = convertion.ConvertDALToBLL(dayReport.staff as DAL.Staff.TeamLead);
            TimeOfCreation = dayReport.TimeOfCreation;
            foreach (var readyProblem in dayReport.ReadyProblems)
            {
                ReadyProblems.Add(new Problem(readyProblem));
            }
            foreach (var comment in dayReport.Comments)
            {
                Comments.Add(comment);
            }
        }

        public DAL.Reports.DayReport MapperToDAL()
        {
            var dayReport = new DAL.Reports.DayReport();
            var convertion = new Convertion.ConvertionDALBLL();
            dayReport.Id = Id;
            if (Staff as StaffBLL != null)
                dayReport.staff = convertion.ConvertBLLToDAL(Staff as StaffBLL);
            else if (Staff as DirectorBLL != null)
                dayReport.staff = convertion.ConvertBLLToDAL(Staff as DirectorBLL);
            else
                dayReport.staff = convertion.ConvertBLLToDAL(Staff as TeamLeadBLL);
            dayReport.TimeOfCreation = TimeOfCreation;
            foreach (var readyProblem in ReadyProblems)
            {
                dayReport.ReadyProblems.Add(readyProblem.MapperToDLL());
            }
            foreach (var comment in Comments)
            {
                dayReport.Comments.Add(comment);
            }
            return dayReport;
        }

    }
}