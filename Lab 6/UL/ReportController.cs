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
    public class ReportCommandController
    {
        private CommandSprintReportSevice _commandSprintReportSevice;
        private ReportSprintController _reportSprintController;

        public ReportCommandController(CommandSprintReportSevice commandSprintReportSevice, ReportSprintController reportSprintController)
        {
            _commandSprintReportSevice = commandSprintReportSevice;
            _reportSprintController = reportSprintController;
        }

        public IEnumerable<CommandSprintReport> GetEnumerable()
        {
            var all = _commandSprintReportSevice.GetEnumerable();
            List<CommandSprintReport> commandSprintReports = new List<CommandSprintReport>();
            foreach (var report in all)
            {
                commandSprintReports.Add(new CommandSprintReport(report));
            }
            return commandSprintReports;
        }

        public CommandSprintReport Get(int id)
        {
            return GetEnumerable().Single(report => report.Id == id);
        }

        private void Replace(CommandSprintReport commandSprintReport)
        {
            _commandSprintReportSevice.Replace(commandSprintReport.MapperToBLL());
        }

        public void Add(CommandSprintReport commandSprintReport)
        {
            var temp = commandSprintReport.MapperToBLL();
            _commandSprintReportSevice.Add(temp);
            commandSprintReport.SetId(temp.Id);
        }

        public void AddSprintReport(CommandSprintReport commandSprintReport, SprintReport sprint)
        {
            commandSprintReport.AddSprintReport(sprint);
            Replace(commandSprintReport);
        }

        public void CloseCommandSprintReport(CommandSprintReport commandSprintReport)
        {
            commandSprintReport.CloseCommandReport();
            string s = string.Empty;
            foreach (var sprintReport in commandSprintReport.GetSprintReports())
            {
                s += _reportSprintController.ShowReport(sprintReport) + "\n\n";
            }
            commandSprintReport.SetReport(s);
            Replace(commandSprintReport);
        }

    }
}