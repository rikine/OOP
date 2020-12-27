using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BLL;
using BLL.Task;
using UL.Report;
using UL.Staff;

namespace UL
{
    public class ReportSprintController
    {
        private SprintReportService _sprintReportService;
        private readonly string _draftOfSprint;

        public ReportSprintController(string draftOfSprint, SprintReportService sprintReportService)
        {
            _sprintReportService = sprintReportService;
            _draftOfSprint = draftOfSprint;
        }

        public IEnumerable<SprintReport> GetEnumerable()
        {
            var all = _sprintReportService.GetEnumerable();
            List<SprintReport> sprintReports = new List<SprintReport>();
            foreach (var sprint in all)
            {
                sprintReports.Add(new SprintReport(sprint));
            }
            return sprintReports.ToArray();
        }

        public SprintReport Get(int id)
        {
            return GetEnumerable().Single(rep => rep.Id == id);
        }

        public void Add(SprintReport report)
        {
            var repordDAL = report.MapperToBLL();
            _sprintReportService.Add(repordDAL);
            report.SetId(repordDAL.Id);
        }

        public SprintReport Get(IStaff staff)
        {
            return GetEnumerable().Single(report => report.Staff.Id == staff.Id);
        }

        public void Replace(SprintReport report)
        {
            _sprintReportService.Replace(report.MapperToBLL());
        }

        public void AddComment(SprintReport report, string comment)
        {
            report.AddComment(comment);
            Replace(report);
        }

        public void RemoveComment(SprintReport report, DateTime time)
        {
            report.RemoveComment(time);
            Replace(report);
        }

        public void CloseAndSaveSprint(SprintReport report)
        {
            report.SetConditionOfSprint(ConditionOfSprint.Closed);
            Replace(report);
        }

        public string ShowReport(SprintReport report)
        {
            return _draftOfSprint + '\n' + report.ToString();
        }

        public ImmutableList<Problem> ShowAllReadyProblemsOfStaffForSprint(IStaff staff)
        {
            List<Problem> problems = new List<Problem>();
            foreach (var sprintReport in GetEnumerable())
            {
                if (sprintReport.Staff.Id == staff.Id)
                {
                    foreach (var dayReports in sprintReport.GetDayReports())
                    {
                        problems.AddRange(dayReports.GetReadyProblems());
                    }
                }
            }
            return problems.ToImmutableList();
        }

        public ImmutableList<SprintReport> ShowAllSprintsOfSlavesOfStaff(IStaff staff)
        {
            var allSprints = GetEnumerable();
            List<SprintReport> sprintReports = new List<SprintReport>();
            if (staff as StaffUL != null)
                throw new ApplicationException("Typical staff can not have slaves");
            else if (staff as DirectorUL != null)
            {
                foreach (var slave in (staff as DirectorUL).Slaves)
                {
                    sprintReports.Add(Get(slave));
                }
            }
            else
            {
                foreach (var slave in (staff as TeamLeadUL).Slaves)
                {
                    sprintReports.Add(Get(slave));
                }
            }
            return sprintReports.ToImmutableList();
        }

    }
}