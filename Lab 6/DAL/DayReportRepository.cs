using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using DAL.Reports;
using DAL.Staff;
using System;

namespace DAL
{
    public class DayReportRepository
    {
        private string _pathDB;
        IEmployees<IStaff> _employees;
        ProblemRepositoryDAL _problemRepository;
        readonly string _idPattern = @"^id=\d+";
        readonly string staffIdPattern = @"^staffid=\d+";
        readonly string timeOfCreationPattern = @"^timeOfCreation=\d{2}\.\d{2}\.\d{4}\d{2}:\d{2}:\d{2}";
        readonly string readyProblems = @"^readyProblems=[\d,]+";
        readonly string withoutReadyProblems = @"^readyProblems=";
        readonly string comment = @"^comments:";
        readonly string time = @"\d{2}\.\d{2}\.\d{4}\d{2}:\d{2}:\d{2}";
        readonly string commentPattern = @"\S";

        public DayReportRepository(string pathDB, IEmployees<IStaff> employees, ProblemRepositoryDAL problemRepository)
        {
            _pathDB = pathDB;
            _employees = employees;
            _problemRepository = problemRepository;
        }

        public IEnumerable<DayReport> GetEnumerable()
        {
            List<DayReport> dayReports = new List<DayReport>();
            using (var sr = new StreamReader(_pathDB))
            {
                string s;
                DayReport dayReport = null;
                while ((s = sr.ReadLine()) != null)
                {
                    var tempWithoutSpaces = s.Replace(" ", string.Empty);
                    if ("[DayReport]" == s.Replace(" ", string.Empty))
                        dayReport = new DayReport();
                    else if (Regex.IsMatch(tempWithoutSpaces, _idPattern))
                    {
                        dayReport.Id = int.Parse(tempWithoutSpaces.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, staffIdPattern))
                    {
                        dayReport.staff = _employees.Get(int.Parse(tempWithoutSpaces.Split('=')[1]));
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, timeOfCreationPattern))
                    {
                        dayReport.TimeOfCreation = DateTime.Parse(s.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, withoutReadyProblems))
                    {
                        continue;
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, readyProblems))
                    {
                        var idsOfProblems = tempWithoutSpaces.Split('=')[1].Split(',');
                        foreach (var id in idsOfProblems)
                        {
                            dayReport.ReadyProblems.Add(_problemRepository.Get(int.Parse(id)));
                        }
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, comment))
                    {
                        ParseComments(dayReport, sr);
                        dayReports.Add(dayReport);
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
            return dayReports.ToArray();
        }

        private void ParseComments(DayReport dayReport, StreamReader sr)
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                var tempWithoutSpaces = s.Replace(" ", string.Empty);
                if (Regex.IsMatch(tempWithoutSpaces, time))
                {
                    var time = DateTime.Parse(s);
                    var comment = s = sr.ReadLine();
                    dayReport.Comments.Add((time, comment));
                }
                else
                {
                    break;
                }
            }
        }

        public DayReport Get(int id)
        {
            return GetEnumerable().Single(rep => rep.Id == id);
        }

        public void Add(DayReport report, bool created = false)
        {
            var allLines = File.ReadAllLines(_pathDB).ToList();
            var allReports = GetEnumerable();
            allLines.Add("");
            allLines.Add("[DayReport]");
            if (!created)
                allLines.Add($"id = {allReports.Count()}");
            else
                allLines.Add($"id = {report.Id}");
            allLines.Add($"staffid = {report.staff.Id}");
            allLines.Add($"timeOfCreation = {report.TimeOfCreation.ToString()}");
            string readyProblems = string.Empty;
            foreach (var reportId in report.ReadyProblems)
            {
                readyProblems += reportId.ToString() + ',';
                if (reportId == report.ReadyProblems.Last())
                    readyProblems = readyProblems.Remove(readyProblems.Count() - 1);
            }
            allLines.Add($"readyProblems = {readyProblems}");
            allLines.Add("comments:");
            foreach (var comment in report.Comments)
            {
                allLines.Add(comment.Time.ToString());
                allLines.Add(comment.comment);
            }
            allLines.Add("");
            File.WriteAllLines(_pathDB, allLines.ToArray());
            report.Id = allReports.Count();
        }

        public void Replace(DayReport report)
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
                            if (allLines[j] == string.Empty || allLines[j] == "[DayReport]")
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

    }
}