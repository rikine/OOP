using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Text;
using UL.Staff;

namespace UL.Report
{
    public class SprintReport
    {
        public int Id { get; private set; }
        public IStaff Staff { get; private set; }
        public DateTime TimeOfCreation { get; }
        public ConditionOfSprint Condition { get; private set; }
        private StringBuilder _readyReport = new StringBuilder();
        private List<(DateTime time, string comments)> _comments = new List<(DateTime time, string comments)>();
        private List<DayReport> _dayReports;

        public SprintReport(IStaff staff, List<DayReport> dayReports)
        {
            Staff = staff;
            Condition = ConditionOfSprint.Open;
            TimeOfCreation = DateTime.Now;
            _dayReports = dayReports;
        }

        public SprintReport(BLL.Reports.SprintReport sprintReport)
        {
            var convertion = new UL.Convertion.ConvertionULBLL();
            Id = sprintReport.Id;
            if (sprintReport.Staff as BLL.Staff.StaffBLL != null)
                this.Staff = convertion.ConvertBLLToUL(sprintReport.Staff as BLL.Staff.StaffBLL);
            else if (sprintReport.Staff as BLL.Staff.DirectorBLL != null)
                this.Staff = convertion.ConvertBLLToUL(sprintReport.Staff as BLL.Staff.DirectorBLL);
            else
                this.Staff = convertion.ConvertBLLToUL(sprintReport.Staff as BLL.Staff.TeamLeadBLL);
            TimeOfCreation = sprintReport.TimeOfCreation;
            Condition = (ConditionOfSprint)sprintReport.Condition;
            foreach (var comment in sprintReport.Comments)
            {
                _comments.Add(comment);
            }
            foreach (var dayReport in sprintReport.DayReports)
            {
                _dayReports.Add(new DayReport(dayReport));
            }
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public BLL.Reports.SprintReport MapperToBLL()
        {
            var convertion = new UL.Convertion.ConvertionULBLL();
            BLL.Staff.IStaffBLL staff = null;
            List<BLL.Reports.DayReport> dayReports = new List<BLL.Reports.DayReport>();
            if (Staff as StaffUL != null)
                staff = convertion.ConvertULToBLL(Staff as StaffUL);
            else if (Staff as DirectorUL != null)
                staff = convertion.ConvertULToBLL(Staff as DirectorUL);
            else
                staff = convertion.ConvertULToBLL(Staff as TeamLeadUL);

            foreach (var dayReport in _dayReports)
            {
                dayReports.Add(dayReport.MapperToBLL());
            }
            var sprintReportBLL = new BLL.Reports.SprintReport(staff, dayReports);
            sprintReportBLL.Id = Id;
            sprintReportBLL.TimeOfCreation = TimeOfCreation;
            sprintReportBLL.Condition = (BLL.Condtion.ConditionOfSprint)this.Condition;
            foreach (var comment in _comments)
            {
                sprintReportBLL.Comments.Add(comment);
            }
            return sprintReportBLL;
        }

        public void AddComment(string comment)
        {
            _comments.Add((DateTime.Now, comment));
        }

        public void SetConditionOfSprint(ConditionOfSprint condition)
        {
            if (Condition == ConditionOfSprint.Closed)
                throw new ApplicationException("Can not change condition of Sprint Report. It's closed");
            if (condition != ConditionOfSprint.Closed)
                throw new ApplicationException("Can not set open condition of Sprint Report. It's open");
            Condition = condition;
            CreateReport(_dayReports);
        }

        public void RemoveComment(DateTime time)
        {
            _comments.Remove(_comments.Find(comm => comm.time == time));
        }


        public override string ToString()
        {
            return _readyReport.ToString();
        }

        public ImmutableList<DayReport> GetDayReports() => _dayReports.ToImmutableList();

        private void CreateReport(List<DayReport> dayReports)
        {
            foreach (var report in dayReports)
            {
                _readyReport.Append(report.ToString());
            }

            foreach (var comm in _comments)
            {
                _readyReport.Append($"Comment was added at {comm.time}: {comm.comments}");
            }
        }
    }
}