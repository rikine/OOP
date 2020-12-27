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
    }
}