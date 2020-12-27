using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using DAL.Problem;
using System.Text.RegularExpressions;
using DAL.Condition;
using DAL.Staff;

namespace DAL
{
    public class ProblemRepositoryDAL
    {
        string _pathDB;
        readonly string _idPattern = @"^id=\d+";
        readonly string _namePattern = @"name=\w+";
        readonly string _descriptionPattern = @"description=\S";
        readonly string staffIdPattern = @"staffid=\d+";
        readonly string conditionPattern = @"condition=\w+";
        readonly string commentPattern = @"comment=\w+";
        readonly string withoutCommentPattern = @"comment=";
        readonly string timeOfCreationPattern = @"timeOfCreation=\d{2}\.\d{2}\.\d{4}\d{2}:\d{2}:\d{2}";
        readonly string staffPattern = @"historyOfChangesOfStaff:";
        readonly string histConditionPattern = @"historyOfChangesOfCondition:";
        readonly string histCommentPattern = @"historyOfChangesOfComment:";
        readonly string time = @"\d{2}\.\d{2}\.\d{4}\d{2}:\d{2}:\d{2}";

        IEmployees<IStaff> _employees;

        public ProblemRepositoryDAL(string pathDB, IEmployees<IStaff> employees)
        {
            _pathDB = pathDB;
            if (!File.Exists(_pathDB)) File.Create(_pathDB).Close();
            _employees = employees;
        }

        public IEnumerable<ProblemDAL> GetEnumerable()
        {
            List<ProblemDAL> problems = new List<ProblemDAL>();
            using (var sr = new StreamReader(_pathDB))
            {
                string s;
                ProblemDAL problem = null;
                while ((s = sr.ReadLine()) != null)
                {
                    var tempWithoutSpaces = s.Replace(" ", string.Empty);
                    if ("[Problem]" == s.Replace(" ", string.Empty))
                        problem = new ProblemDAL();
                    else if (Regex.IsMatch(tempWithoutSpaces, _idPattern))
                    {
                        problem.Id = int.Parse(tempWithoutSpaces.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, _namePattern))
                    {
                        problem.Name = s.Split('=')[1];
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, _descriptionPattern))
                    {
                        problem.Description = s.Split('=')[1];
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, staffIdPattern))
                    {
                        problem.Staff = _employees.Get(int.Parse(tempWithoutSpaces.Split('=')[1]));
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, conditionPattern))
                    {
                        problem.Condition = stringToEnum(tempWithoutSpaces.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, commentPattern))
                    {
                        problem.Comment = s.Split('=')[1];
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, withoutCommentPattern))
                    {
                        problem.Comment = string.Empty;
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, timeOfCreationPattern))
                    {
                        problem.TimeOfCreation = DateTime.Parse(s.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, staffPattern))
                    {
                        ParseHistoryStaff(problem, sr);
                        problems.Add(problem);
                    }
                    else if (tempWithoutSpaces == string.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        throw new ApplicationException("Parse Problem Exception");
                    }
                }
            }
            return problems.ToArray();
        }

        private void ParseHistoryStaff(ProblemDAL problem, StreamReader sr)
        {
            string s;
            var historyStaff = new ChangeStaff();
            while ((s = sr.ReadLine()) != null)
            {
                var tempWithoutSpaces = s.Replace(" ", string.Empty);
                if (Regex.IsMatch(tempWithoutSpaces, time))
                {
                    historyStaff.Time = DateTime.Parse(s);
                    historyStaff.NewStaff = _employees.Get(int.Parse(s = sr.ReadLine()));
                    historyStaff.Who = _employees.Get(int.Parse(s = sr.ReadLine()));
                    problem.HistoryOfChangesOfStaff.Add(historyStaff);
                    historyStaff = new ChangeStaff();
                }
                else if (Regex.IsMatch(tempWithoutSpaces, histConditionPattern))
                {
                    ParseHistoryCondition(problem, sr);
                    break;
                }
            }
        }

        private void ParseHistoryCondition(ProblemDAL problem, StreamReader sr)
        {
            string s;
            var historyCondition = new ChangeCondition();
            while ((s = sr.ReadLine()) != null)
            {
                var tempWithoutSpaces = s.Replace(" ", string.Empty);
                if (Regex.IsMatch(tempWithoutSpaces, time))
                {
                    historyCondition.Time = DateTime.Parse(s);
                    historyCondition.NewCondition = stringToEnum(s = sr.ReadLine());
                    historyCondition.Who = _employees.Get(int.Parse(s = sr.ReadLine()));
                    problem.HistoryOfChangesOfCondition.Add(historyCondition);
                    historyCondition = new ChangeCondition();
                }
                else if (Regex.IsMatch(tempWithoutSpaces, histCommentPattern))
                {
                    ParseHistoryComment(problem, sr);
                    break;
                }
            }
        }

        private void ParseHistoryComment(ProblemDAL problem, StreamReader sr)
        {
            string s;
            var historyComment = new ChangeComment();
            while ((s = sr.ReadLine()) != null)
            {
                var tempWithoutSpaces = s.Replace(" ", string.Empty);
                if (Regex.IsMatch(tempWithoutSpaces, time))
                {
                    historyComment.Time = DateTime.Parse(s);
                    historyComment.NewComment = s = sr.ReadLine();
                    historyComment.Who = _employees.Get(int.Parse(s = sr.ReadLine()));
                    problem.HistoryOfChangesOfComment.Add(historyComment);
                    historyComment = new ChangeComment();
                }
                else
                    break;
            }
        }

        private ConditionOfProblem stringToEnum(string s)
        {
            if (s == "Open")
                return ConditionOfProblem.Open;
            else if (s == "Resolved")
                return ConditionOfProblem.Resolved;
            else if (s == "Active")
                return ConditionOfProblem.Active;
            else
                throw new ApplicationException("No such enum value");
        }

        public ProblemDAL Get(int id)
        {
            return GetEnumerable().Single(problem => problem.Id == id);
        }

        public void Add(ProblemDAL problem, bool created = false)
        {
            var allLines = File.ReadAllLines(_pathDB).ToList();
            var all = GetEnumerable();
            allLines.Add("");
            allLines.Add("[Problem]");
            if (!created)
                allLines.Add($"id = {all.Count()}");
            else
                allLines.Add($"id = {problem.Id}");
            allLines.Add($"name = {problem.Name}");
            allLines.Add($"description = {problem.Description}");
            allLines.Add($"staffid = {problem.Staff.Id}");
            allLines.Add($"condition = {EnumToString(problem.Condition)}");
            allLines.Add($"comment = {problem.Comment}");
            allLines.Add($"timeOfCreation = {problem.TimeOfCreation.ToString()}");
            allLines.Add("historyOfChangesOfStaff:");
            foreach (var change in problem.HistoryOfChangesOfStaff)
            {
                allLines.Add($"{change.Time.ToString()}");
                allLines.Add($"{change.NewStaff.Id}");
                allLines.Add($"{change.Who.Id}");
            }
            allLines.Add("historyOfChangesOfCondition:");
            foreach (var change in problem.HistoryOfChangesOfCondition)
            {
                allLines.Add($"{change.Time.ToString()}");
                allLines.Add($"{EnumToString(change.NewCondition)}");
                allLines.Add($"{change.Who.Id}");
            }
            allLines.Add("historyOfChangesOfComment:");
            foreach (var change in problem.HistoryOfChangesOfComment)
            {
                allLines.Add($"{change.Time.ToString()}");
                allLines.Add($"{change.NewComment}");
                allLines.Add($"{change.Who.Id}");
            }
            allLines.Add("");
            RemoveNewLines(allLines);
            File.WriteAllLines(_pathDB, allLines.ToArray());
            problem.Id = all.Count();
        }

        private string EnumToString(ConditionOfProblem condition)
        {
            switch (condition)
            {
                case ConditionOfProblem.Active:
                    return "Active";
                case ConditionOfProblem.Open:
                    return "Open";
                case ConditionOfProblem.Resolved:
                    return "Resolved";
                default:
                    return string.Empty;
            }
        }

        public void Replace(ProblemDAL problem)
        {
            var allLines = File.ReadAllLines(_pathDB).ToList();
            var linesToDel = 2;
            var startPos = 0;
            for (int i = 0; i < allLines.Count; i++)
            {
                if (Regex.IsMatch(allLines[i].Replace(" ", string.Empty), _idPattern))
                {
                    if (int.Parse(allLines[i].Replace(" ", string.Empty).Split('=')[1]) == problem.Id)
                    {
                        startPos = i - 1;
                        for (int j = i + 1; j < allLines.Count; j++)
                        {
                            if (allLines[j] == string.Empty || allLines[j] == "[Problem]")
                                break;
                            else
                                linesToDel++;
                        }
                        break;
                    }
                }
            }
            if (linesToDel == 2)
                throw new ApplicationException($"No problem with id = {problem.Id}");
            for (int i = startPos; i < linesToDel + startPos; i++)
            {
                allLines.RemoveAt(startPos);
            }
            File.WriteAllLines(_pathDB, allLines.ToArray());
            Add(problem, true);
        }

        public void RemoveNewLines(List<string> allLines)
        {
            for (int i = 0; i < allLines.Count; i++)
            {
                if (allLines.Count + 1 < allLines.Count && allLines[i] == "" && allLines[i + 1] == "")
                    allLines.Remove(allLines[i]);
            }
        }
    }
}