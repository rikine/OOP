using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.Task;
using UL.Staff;
using UL.Report;

namespace UL
{
    public class ReportController
    {
        private readonly string _defaultDraftOfSprintReport;

        List<CommandSprintReport> commandSprintReports = new List<CommandSprintReport>();
        List<SprintReport> savedSprintReports = new List<SprintReport>();

        public ReportController()
        {
            _defaultDraftOfSprintReport = "SPRINT REPORT\nALL STUFF MUST WRITE IT.\n";
        }

        public ImmutableList<Problem> ShowAllReadyProblemsOfStaffForSprint(IStaff staff)
        {
            List<Problem> problems = new List<Problem>();
            foreach (var dayReport in staff.GetDayReports())
                problems.AddRange(dayReport.GetReadyProblems());
            return problems.ToImmutableList();
        }

        public ImmutableList<Problem> ShowAllReadyProblemsOfStaffForDay(IStaff staff)
        {
            return staff.GetDayReports().Last().GetReadyProblems();
        }

        public ImmutableList<SprintReport> ShowAllSprintOfSlavesOfStaff(IStaff staff)
        {
            List<SprintReport> sprintReports = new List<SprintReport>();
            if (staff as StaffUL != null)
                throw new ApplicationException("Typical staff can not have slaves");
            else if (staff as DirectorUL != null)
            {
                foreach (var slave in (staff as DirectorUL).Slaves)
                {
                    sprintReports.Add(slave.GetSprintReport());
                }
            }
            else
            {
                foreach (var slave in (staff as TeamLeadUL).Slaves)
                {
                    sprintReports.Add(slave.GetSprintReport());
                }
            }
            return sprintReports.ToImmutableList();
        }

        public void MakeSprintReport(IStaff staff)
        {
            staff.MakeSprintReport(_defaultDraftOfSprintReport);
        }

        public void AddCommentSprint(IStaff staff, string comment)
        {
            staff.GetSprintReport().AddComment(comment);
        }

        public void RemoveCommentSprint(IStaff staff, DateTime time)
        {
            staff.GetSprintReport().RemoveComment(time);
        }

        public void SaveAndCloseSprintReport(IStaff staff)
        {
            staff.GetSprintReport().SetConditionOfSprint(ConditionOfSprint.Closed);
            savedSprintReports.Add(staff.GetSprintReport());
        }

        public void CreateCommandSprintReport()
        {
            var allComandReportsSprint = new CommandSprintReport();
            foreach (var report in savedSprintReports)
            {
                allComandReportsSprint.AddSprintReport(report);
            }
            allComandReportsSprint.CloseCommandReport();
        }

    }
}