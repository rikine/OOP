using System.Linq;
using System.Collections.Generic;
using BLL.Reports;
using DAL;

namespace BLL
{
    public class CommandSprintReportSevice
    {
        private CommandSprintReportRepository _commandSprintReportRepository;

        public CommandSprintReportSevice(CommandSprintReportRepository commandSprintReportRepository)
        {
            _commandSprintReportRepository = commandSprintReportRepository;
        }

        public IEnumerable<CommandSprintReport> GetEnumerable()
        {
            var all = _commandSprintReportRepository.GetEnumerable();
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

        public void Replace(CommandSprintReport report)
        {
            _commandSprintReportRepository.Replace(report.MapperToDAL());
        }

        public void Add(CommandSprintReport report)
        {
            var repordDAL = report.MapperToDAL();
            _commandSprintReportRepository.Add(repordDAL);
            report.Id = repordDAL.Id;
        }
    }
}