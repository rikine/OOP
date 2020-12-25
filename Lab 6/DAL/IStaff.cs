#nullable enable
using System;

namespace DAL.Staff
{
    public interface IStaff
    {
        int? Id { get; }
        string? Name { get; }
        string Print(int offset);
    }
}
