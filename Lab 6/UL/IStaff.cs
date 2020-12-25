using System;
using System.Collections.Immutable;
using BLL.Task;
using UL.Report;

namespace UL
{
    public interface IStaff
    {
        int? Id { get; }
        string Name { get; }
        string Print(int offset);
        bool EqualsBLL(BLL.Staff.IStaffBLL staff);
        ImmutableList<DayReport> GetDayReports();
        SprintReport GetSprintReport();
        void MakeSprintReport(string defaultDraftOfSprintReport);
        void AddCommentToReport(string comment);
        void AddReadyProblem(Problem problem);
        void RemoveComment(DateTime time);
    }
}