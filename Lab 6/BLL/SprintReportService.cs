using System.Linq;
using System.Collections.Generic;
using BLL.Reports;
using DAL;

namespace BLL
{
    public class SprintReportService
    {
        private SprintReportRepository _sprintReportRepository;

        public SprintReportService(SprintReportRepository sprintReportRepository)
        {
            _sprintReportRepository = sprintReportRepository;
        }

        public IEnumerable<SprintReport> GetEnumerable()
        {
            var all = _sprintReportRepository.GetEnumerable();
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
            var repordDAL = report.MapperToDAL();
            _sprintReportRepository.Add(repordDAL);
            report.Id = repordDAL.Id;
        }

        public void Replace(SprintReport report)
        {
            _sprintReportRepository.Replace(report.MapperToDAL());
        }
    }
}