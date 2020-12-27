using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Staff;
using BLL.Condtion;

namespace BLL.Task
{
    public class Problem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        private IStaffBLL _staff;
        private ConditionOfProblem _condition;
        private string _comment;
        public DateTime TimeOfCreation { get; private set; }
        private List<ChangeStaff> _historyOfChangesOfStaff = new List<ChangeStaff>();
        private List<ChangeCondition> _historyOfChangesOfCondition = new List<ChangeCondition>();
        private List<ChangeComment> _historyOfChangesOfComment = new List<ChangeComment>();

        public Problem(DAL.Problem.ProblemDAL problemDAL)
        {
            var convertion = new BLL.Convertion.ConvertionDALBLL();
            Id = problemDAL.Id;
            Name = problemDAL.Name;
            Description = problemDAL.Description;
            if (problemDAL.Staff as DAL.Staff.DALStaff != null)
                _staff = convertion.ConvertDALToBLL(problemDAL.Staff as DAL.Staff.DALStaff);
            else if (problemDAL.Staff as DAL.Staff.Director != null)
                _staff = convertion.ConvertDALToBLL(problemDAL.Staff as DAL.Staff.Director);
            else
                _staff = convertion.ConvertDALToBLL(problemDAL.Staff as DAL.Staff.TeamLead);
            _condition = (ConditionOfProblem)problemDAL.Condition;
            _comment = problemDAL.Comment;
            TimeOfCreation = problemDAL.TimeOfCreation;

            foreach (var change in problemDAL.HistoryOfChangesOfComment)
            {
                var changeComment = new ChangeComment();
                changeComment.Time = change.Time;
                changeComment.NewComment = change.NewComment;
                if (change.Who as DAL.Staff.DALStaff != null)
                    changeComment.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.DALStaff);
                else if (change.Who as DAL.Staff.Director != null)
                    changeComment.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.Director);
                else
                    changeComment.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.TeamLead);
                _historyOfChangesOfComment.Add(changeComment);
            }

            foreach (var change in problemDAL.HistoryOfChangesOfCondition)
            {
                var changeCondition = new ChangeCondition();
                changeCondition.Time = change.Time;
                changeCondition.NewCondition = (ConditionOfProblem)change.NewCondition;
                if (change.Who as DAL.Staff.DALStaff != null)
                    changeCondition.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.DALStaff);
                else if (change.Who as DAL.Staff.Director != null)
                    changeCondition.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.Director);
                else
                    changeCondition.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.TeamLead);
                _historyOfChangesOfCondition.Add(changeCondition);
            }

            foreach (var change in problemDAL.HistoryOfChangesOfStaff)
            {
                var changeStaff = new ChangeStaff();
                changeStaff.Time = change.Time;

                if (change.NewStaff as DAL.Staff.DALStaff != null)
                    changeStaff.NewStaff = convertion.ConvertDALToBLL(change.NewStaff as DAL.Staff.DALStaff);
                else if (change.NewStaff as DAL.Staff.Director != null)
                    changeStaff.NewStaff = convertion.ConvertDALToBLL(change.NewStaff as DAL.Staff.Director);
                else
                    changeStaff.NewStaff = convertion.ConvertDALToBLL(change.NewStaff as DAL.Staff.TeamLead);
                if (change.Who as DAL.Staff.DALStaff != null)
                    changeStaff.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.DALStaff);
                else if (change.Who as DAL.Staff.Director != null)
                    changeStaff.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.Director);
                else
                    changeStaff.Who = convertion.ConvertDALToBLL(change.Who as DAL.Staff.TeamLead);
                _historyOfChangesOfStaff.Add(changeStaff);
            }
        }

        public Problem(string name, string description, IStaffBLL staff)
        {
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

        public DAL.Problem.ProblemDAL MapperToDLL()
        {
            var convertion = new BLL.Convertion.ConvertionDALBLL();
            var problemDAL = new DAL.Problem.ProblemDAL();
            problemDAL.Id = Id;
            problemDAL.Name = Name;
            problemDAL.Description = Description;
            if (GetStaff() as StaffBLL != null)
                problemDAL.Staff = convertion.ConvertBLLToDAL(GetStaff() as StaffBLL);
            else if (GetStaff() as DirectorBLL != null)
                problemDAL.Staff = convertion.ConvertBLLToDAL(GetStaff() as DirectorBLL);
            else
                problemDAL.Staff = convertion.ConvertBLLToDAL(GetStaff() as TeamLeadBLL);
            problemDAL.Condition = (DAL.Condition.ConditionOfProblem)GetCondition();
            problemDAL.Comment = GetComment();
            problemDAL.TimeOfCreation = TimeOfCreation;
            List<DAL.Problem.ChangeStaff> changesStaff = new List<DAL.Problem.ChangeStaff>();
            foreach (var change in _historyOfChangesOfStaff)
            {
                var changeStaff = new DAL.Problem.ChangeStaff();
                changeStaff.Time = change.Time;
                if (change.NewStaff as StaffBLL != null)
                    changeStaff.NewStaff = convertion.ConvertBLLToDAL(change.NewStaff as StaffBLL);
                else if (change.NewStaff as DirectorBLL != null)
                    changeStaff.NewStaff = convertion.ConvertBLLToDAL(change.NewStaff as DirectorBLL);
                else
                    changeStaff.NewStaff = convertion.ConvertBLLToDAL(change.NewStaff as TeamLeadBLL);

                if (change.Who as StaffBLL != null)
                    changeStaff.Who = convertion.ConvertBLLToDAL(change.Who as StaffBLL);
                else if (change.Who as DirectorBLL != null)
                    changeStaff.Who = convertion.ConvertBLLToDAL(change.Who as DirectorBLL);
                else
                    changeStaff.Who = convertion.ConvertBLLToDAL(change.Who as TeamLeadBLL);
                changesStaff.Add(changeStaff);
            }

            List<DAL.Problem.ChangeCondition> changesCondition = new List<DAL.Problem.ChangeCondition>();
            foreach (var change in _historyOfChangesOfCondition)
            {
                var changeCond = new DAL.Problem.ChangeCondition();
                changeCond.Time = change.Time;
                changeCond.NewCondition = (DAL.Condition.ConditionOfProblem)change.NewCondition;
                if (change.Who as StaffBLL != null)
                    changeCond.Who = convertion.ConvertBLLToDAL(change.Who as StaffBLL);
                else if (change.Who as DirectorBLL != null)
                    changeCond.Who = convertion.ConvertBLLToDAL(change.Who as DirectorBLL);
                else
                    changeCond.Who = convertion.ConvertBLLToDAL(change.Who as TeamLeadBLL);
                changesCondition.Add(changeCond);
            }

            List<DAL.Problem.ChangeComment> changesComment = new List<DAL.Problem.ChangeComment>();
            foreach (var change in _historyOfChangesOfComment)
            {
                var changeComment = new DAL.Problem.ChangeComment();
                changeComment.Time = change.Time;
                changeComment.NewComment = change.NewComment;
                if (change.Who as StaffBLL != null)
                    changeComment.Who = convertion.ConvertBLLToDAL(change.Who as StaffBLL);
                else if (change.Who as DirectorBLL != null)
                    changeComment.Who = convertion.ConvertBLLToDAL(change.Who as DirectorBLL);
                else
                    changeComment.Who = convertion.ConvertBLLToDAL(change.Who as TeamLeadBLL);
                changesComment.Add(changeComment);
            }
            problemDAL.HistoryOfChangesOfComment = changesComment;
            problemDAL.HistoryOfChangesOfCondition = changesCondition;
            problemDAL.HistoryOfChangesOfStaff = changesStaff;
            return problemDAL;
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