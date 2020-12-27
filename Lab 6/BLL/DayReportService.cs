using System.Linq;
using System.Collections.Generic;
using BLL.Reports;
using DAL;

namespace BLL
{
    public class DayReportService
    {
        private DayReportRepository _dayReportRepository;

        public DayReportService(DayReportRepository dayReportRepository)
        {
            _dayReportRepository = dayReportRepository;
        }

        public IEnumerable<DayReport> GetEnumerable()
        {
            var all = _dayReportRepository.GetEnumerable();
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
            var reportDAL = report.MapperToDAL();
            _dayReportRepository.Add(reportDAL);
            report.Id = reportDAL.Id;
        }

        public void Replace(DayReport report)
        {
            _dayReportRepository.Replace(report.MapperToDAL());
        }

    }
}