using System;
using System.Collections.Generic;
using BLL.Staff;

namespace BLL.Task
{
    public class Problem
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        private IStaffBLL _staff;
        private ConditionOfProblem _condition;
        private string _comment;
        public DateTime TimeOfCreation { get; }
        private List<ChangeStaff> _historyOfChangesOfStaff = new List<ChangeStaff>();
        private List<ChangeCondition> _historyOfChangesOfCondition = new List<ChangeCondition>();
        private List<ChangeComment> _historyOfChangesOfComment = new List<ChangeComment>();

        public Problem(int id, string name, string description, IStaffBLL staff)
        {
            Id = id;
            TimeOfCreation = DateTime.Now;
            _condition = ConditionOfProblem.Open;
            Name = name;
            Description = description;
            _staff = staff;
        }

        public void AddComment(string comment, IStaffBLL who)
        {
            _comment = comment;
            _historyOfChangesOfComment.Add(new ChangeComment(comment, who));
        }

        public string GetComment() => _comment;

        public void ChangeCondition(ConditionOfProblem condition, IStaffBLL who)
        {
            _condition = condition;
            _historyOfChangesOfCondition.Add(new ChangeCondition(condition, who));
        }

        public ConditionOfProblem GetCondition() => _condition;

        public void ChangeStaff(IStaffBLL staff, IStaffBLL who)
        {
            _staff = staff;
            _historyOfChangesOfStaff.Add(new ChangeStaff(staff, who));
        }

        public IStaffBLL GetStaff() => _staff;

        public DateTime? TimeOfLastChange()
        {
            DateTime? lastDateTime = null;
            foreach (var change in _historyOfChangesOfComment)
            {
                if (lastDateTime == null)
                    lastDateTime = change.Time;
                if (lastDateTime < change.Time)
                    lastDateTime = change.Time;
            }
            foreach (var change in _historyOfChangesOfCondition)
            {
                if (lastDateTime == null)
                    lastDateTime = change.Time;
                if (lastDateTime < change.Time)
                    lastDateTime = change.Time;
            }
            foreach (var change in _historyOfChangesOfStaff)
            {
                if (lastDateTime == null)
                    lastDateTime = change.Time;
                if (lastDateTime < change.Time)
                    lastDateTime = change.Time;
            }
            return lastDateTime;
        }

        public bool IsStaffMakeAnyChanges(IStaffBLL staff)
        {
            foreach (var change in _historyOfChangesOfComment)
            {
                if (change.Who == staff)
                    return true;
            }
            foreach (var change in _historyOfChangesOfCondition)
            {
                if (change.Who == staff)
                    return true;
            }
            foreach (var change in _historyOfChangesOfStaff)
            {
                if (change.Who == staff)
                    return true;
            }
            return false;
        }

        public List<ChangeStaff> AllChangesOfStaffByStaff(IStaffBLL staff)
        {
            var history = new List<ChangeStaff>();
            foreach (var change in _historyOfChangesOfStaff)
            {
                if (change.Who == staff)
                    history.Add(change);
            }
            return history;
        }

        public List<ChangeComment> AllChangesOfCommentByStaff(IStaffBLL staff)
        {
            var history = new List<ChangeComment>();
            foreach (var change in _historyOfChangesOfComment)
            {
                if (change.Who == staff)
                    history.Add(change);
            }
            return history;
        }

        public List<ChangeCondition> AllChangesOfConditionByStaff(IStaffBLL staff)
        {
            var history = new List<ChangeCondition>();
            foreach (var change in _historyOfChangesOfCondition)
            {
                if (change.Who.Id == staff.Id)
                    history.Add(change);
            }
            return history;
        }
    }

    public struct ChangeStaff
    {
        public DateTime Time;
        public IStaffBLL NewStaff;
        public IStaffBLL Who;

        public ChangeStaff(IStaffBLL newStaff, IStaffBLL who)
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
        public IStaffBLL Who;

        public ChangeCondition(ConditionOfProblem newCondition, IStaffBLL who)
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
        public IStaffBLL Who;

        public ChangeComment(string newComment, IStaffBLL who)
        {
            Time = DateTime.Now;
            NewComment = newComment;
            Who = who;
        }
    }

}