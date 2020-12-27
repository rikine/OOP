using DAL.Staff;
using DAL.Reports;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using DAL.Condition;
using System.Linq;

namespace DAL
{
    public class SprintReportRepository
    {
        private string _pathDB;
        IEmployees<IStaff> _employees;
        DayReportRepository _dayReportRepository;
        readonly string _idPattern = @"^id=\d+";
        readonly string staffIdPattern = @"^staffid=\d+";
        readonly string timeOfCreationPattern = @"^timeOfCreation=\d{2}\.\d{2}\.\d{4}\d{2}:\d{2}:\d{2}";
        readonly string conditionPattern = @"^condition=\w+";
        readonly string dayReports = @"^dayReports=[\d,]+";
        readonly string withoutDayReports = @"^dayReports=";
        readonly string comment = @"^comments:";
        readonly string time = @"\d{2}\.\d{2}\.\d{4}\d{2}:\d{2}:\d{2}";
        readonly string commentPattern = @"\S";

        public SprintReportRepository(string pathDB, IEmployees<IStaff> employees, DayReportRepository dayReportRepository)
        {
            _pathDB = pathDB;
            _employees = employees;
            _dayReportRepository = dayReportRepository;
        }

        public IEnumerable<SprintReport> GetEnumerable()
        {
            List<SprintReport> sprintReports = new List<SprintReport>();
            using (var sr = new StreamReader(_pathDB))
            {
                string s;
                SprintReport sprintReport = null;
                while ((s = sr.ReadLine()) != null)
                {
                    var tempWithoutSpaces = s.Replace(" ", string.Empty);
                    if ("[SprintReport]" == s.Replace(" ", string.Empty))
                        sprintReport = new SprintReport();
                    else if (Regex.IsMatch(tempWithoutSpaces, _idPattern))
                    {
                        sprintReport.Id = int.Parse(tempWithoutSpaces.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, staffIdPattern))
                    {
                        sprintReport.staff = _employees.Get(int.Parse(tempWithoutSpaces.Split('=')[1]));
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, timeOfCreationPattern))
                    {
                        sprintReport.TimeOfCreation = DateTime.Parse(s.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, conditionPattern))
                    {
                        sprintReport.Condition = stringToEnum(tempWithoutSpaces.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, withoutDayReports))
                    {
                        continue;
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, dayReports))
                    {
                        var idsOfProblems = tempWithoutSpaces.Split('=')[1].Split(',');
                        foreach (var id in idsOfProblems)
                        {
                            sprintReport.DayReports.Add(_dayReportRepository.Get(int.Parse(id)));
                        }
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, comment))
                    {
                        ParseComments(sprintReport, sr);
                        sprintReports.Add(sprintReport);
                    }
                    else if (tempWithoutSpaces == string.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        throw new ApplicationException("Day report parse error");
                    }
                }
            }
            return sprintReports.ToArray();
        }

        private void ParseComments(SprintReport sprintReport, StreamReader sr)
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                var tempWithoutSpaces = s.Replace(" ", string.Empty);
                if (Regex.IsMatch(tempWithoutSpaces, time))
                {
                    var time = DateTime.Parse(s);
                    var comment = s = sr.ReadLine();
                    sprintReport.Comments.Add((time, comment));
                }
                else
                {
                    break;
                }
            }
        }

        public SprintReport Get(int id)
        {
            return GetEnumerable().Single(rep => rep.Id == id);
        }

        public void Add(SprintReport report, bool created = false)
        {
            var allLines = File.ReadAllLines(_pathDB).ToList();
            var allReports = GetEnumerable();
            allLines.Add("");
            allLines.Add("[SprintReport]");
            if (!created)
                allLines.Add($"id = {allReports.Count()}");
            else
                allLines.Add($"id = {report.Id}");
            allLines.Add($"staffid = {report.staff.Id}");
            allLines.Add($"timeOfCreation = {report.TimeOfCreation.ToString()}");
            allLines.Add($"condition = {EnumToString(report.Condition)}");
            string dayReports = string.Empty;
            foreach (var reportId in report.DayReports)
            {
                dayReports += reportId.ToString() + ',';
                if (reportId == report.DayReports.Last())
                    dayReports = dayReports.Remove(dayReports.Count() - 1);
            }
            allLines.Add($"dayReports = {dayReports}");
            allLines.Add("comments:");
            foreach (var comment in report.Comments)
            {
                allLines.Add(comment.time.ToString());
                allLines.Add(comment.comments);
            }
            allLines.Add("");
            File.WriteAllLines(_pathDB, allLines.ToArray());
            report.Id = allReports.Count();
        }

        public void Replace(SprintReport report)
        {
            var allLines = File.ReadAllLines(_pathDB).ToList();
            var linesToDel = 2;
            var startPos = 0;
            for (int i = 0; i < allLines.Count; i++)
            {
                if (Regex.IsMatch(allLines[i].Replace(" ", string.Empty), _idPattern))
                {
                    if (int.Parse(allLines[i].Replace(" ", string.Empty).Split('=')[1]) == report.Id)
                    {
                        startPos = i - 1;
                        for (int j = i + 1; j < allLines.Count; j++)
                        {
                            if (allLines[j] == string.Empty || allLines[j] == "[SprintReport]")
                                break;
                            else
                                linesToDel++;
                        }
                        break;
                    }
                }
            }
            if (linesToDel == 2)
                throw new ApplicationException($"No report with id = {report.Id}");
            for (int i = startPos; i < linesToDel + startPos; i++)
            {
                allLines.RemoveAt(startPos);
            }
            File.WriteAllLines(_pathDB, allLines.ToArray());
            Add(report, true);
        }

        private string EnumToString(ConditionOfSprint condition)
        {
            switch (condition)
            {
                case ConditionOfSprint.Close:
                    return "Close";
                case ConditionOfSprint.Open:
                    return "Open";
                default:
                    return string.Empty;
            }
        }

        private ConditionOfSprint stringToEnum(string s)
        {
            switch (s)
            {
                case "Open":
                    return ConditionOfSprint.Open;
                case "Close":
                    return ConditionOfSprint.Close;
                default:
                    throw new ApplicationException("No such enum value");
            }
        }

    }
}