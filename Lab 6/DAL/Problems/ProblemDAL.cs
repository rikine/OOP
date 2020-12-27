using System;
using DAL.Staff;
using DAL.Condition;
using System.Collections.Generic;

namespace DAL.Problem
{
    public class ProblemDAL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IStaff Staff { get; set; }
        public ConditionOfProblem Condition { get; set; }
        public string Comment { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public List<ChangeStaff> HistoryOfChangesOfStaff = new List<ChangeStaff>();
        public List<ChangeCondition> HistoryOfChangesOfCondition = new List<ChangeCondition>();
        public List<ChangeComment> HistoryOfChangesOfComment = new List<ChangeComment>();
    }

    public struct ChangeStaff
    {
        public DateTime Time;
        public IStaff NewStaff;
        public IStaff Who;

        public ChangeStaff(IStaff newStaff, IStaff who)
        {
            Time = DateTime.Now;
            NewStaff = newStaff;
            Who = who;
        }
    }

    public struct ChangeCondition
    {
        public DateTime Time;
        public ConditionOfProblem NewCondition;
        public IStaff Who;

        public ChangeCondition(ConditionOfProblem newCondition, IStaff who)
        {
            Time = DateTime.Now;
            NewCondition = newCondition;
            Who = who;
        }
    }

    public struct ChangeComment
    {
        public DateTime Time;
        public string NewComment;
        public IStaff Who;

        public ChangeComment(string newComment, IStaff who)
        {
            Time = DateTime.Now;
            NewComment = newComment;
            Who = who;
        }
    }
}