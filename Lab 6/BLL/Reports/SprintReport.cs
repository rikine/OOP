using System;
using System.Collections.Generic;
using BLL.Staff;
using BLL.Condtion;

namespace BLL.Reports
{
    public class SprintReport
    {
        public int Id { get; set; }
        public IStaffBLL Staff { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public ConditionOfSprint Condition { get; set; }
        public List<(DateTime time, string comments)> Comments = new List<(DateTime time, string comments)>();
        public List<DayReport> DayReports;

        public SprintReport(IStaffBLL staff, List<DayReport> dayReports)
        {
            TimeOfCreation = DateTime.Now;
            Condition = ConditionOfSprint.Open;
            DayReports = dayReports;
            Staff = staff;
        }

        public SprintReport(DAL.Reports.SprintReport sprintReport)
        {
            var convertion = new BLL.Convertion.ConvertionDALBLL();
            Id = sprintReport.Id;
            if (sprintReport.staff as DAL.Staff.DALStaff != null)
                Staff = convertion.ConvertDALToBLL(sprintReport.staff as DAL.Staff.DALStaff);
            else if (sprintReport.staff as DAL.Staff.Director != null)
                Staff = convertion.ConvertDALToBLL(sprintReport.staff as DAL.Staff.Director);
            else
                Staff = convertion.ConvertDALToBLL(sprintReport.staff as DAL.Staff.TeamLead);
            TimeOfCreation = sprintReport.TimeOfCreation;
            Condition = (ConditionOfSprint)sprintReport.Condition;
            foreach (var comment in sprintReport.Comments)
            {
                Comments.Add(comment);
            }
            foreach (var dayReport in sprintReport.DayReports)
            {
                DayReports.Add(new DayReport(dayReport));
            }
        }

        public DAL.Reports.SprintReport MapperToDAL()
        {
            var convertion = new BLL.Convertion.ConvertionDALBLL();
            var sprintReportDAL = new DAL.Reports.SprintReport();
            sprintReportDAL.Id = Id;
            if (Staff as StaffBLL != null)
                sprintReportDAL.staff = convertion.ConvertBLLToDAL(Staff as StaffBLL);
            else if (Staff as DirectorBLL != null)
                sprintReportDAL.staff = convertion.ConvertBLLToDAL(Staff as DirectorBLL);
            else
                sprintReportDAL.staff = convertion.ConvertBLLToDAL(Staff as TeamLeadBLL);
            sprintReportDAL.TimeOfCreation = TimeOfCreation;
            sprintReportDAL.Condition = (DAL.Condition.ConditionOfSprint)this.Condition;
            foreach (var comment in Comments)
            {
                sprintReportDAL.Comments.Add(comment);
            }
            foreach (var dayReport in DayReports)
            {
                sprintReportDAL.DayReports.Add(dayReport.MapperToDAL());
            }
            return sprintReportDAL;
        }

    }
}