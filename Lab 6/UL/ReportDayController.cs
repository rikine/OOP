using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using UL.Report;
using BLL.Task;
using System.Collections.Immutable;

namespace UL
{
    public class ReportDayController
    {
        private DayReportService _dayReportService;

        public ReportDayController(DayReportService dayReportService)
        {
            _dayReportService = dayReportService;
        }

        public IEnumerable<DayReport> GetEnumerable()
        {
            var all = _dayReportService.GetEnumerable();
            List<DayReport> dayReports = new List<DayReport>();
            foreach (var report in all)
            {
                dayReports.Add(new DayReport(report));
            }
            return dayReports;
        }

        public DayReport Get(int id)
        {
            return GetEnumerable().Single(rep => rep.Id == id);
        }

        public void Add(DayReport report)
        {
            var reportDAL = report.MapperToBLL();
            _dayReportService.Add(reportDAL);
            report.SetId(reportDAL.Id);
        }

        private void Replace(DayReport report)
        {
            _dayReportService.Replace(report.MapperToBLL());
        }

        public void AddReadyProblem(DayReport report, Problem problem)
        {
            report.AddReadyProblem(problem);
            Replace(report);
        }

        public ImmutableList<Problem> GetReadyProblems(DayReport report)
        {
            return report.GetReadyProblems();
        }

        public void AddComment(DayReport report, string comment)
        {
            report.AddComment(comment);
            Replace(report);
        }

        public void RemoveComment(DayReport report, DateTime time)
        {
            report.RemoveComment(time);
            Replace(report);
        }

        public ImmutableList<Problem> ShowAllReadyProblemsOfStaffForDay(IStaff staff)
        {
            var all = GetEnumerable();
            var problems = new List<Problem>();
            foreach (var report in all)
            {
                if (report.Staff.Id == staff.Id)
                {
                    problems.AddRange(report.GetReadyProblems());
                }
            }
            return problems.ToImmutableList();
        }



    }
}