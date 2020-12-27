using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using DAL.Condition;
using DAL.Reports;

namespace DAL
{
    public class CommandSprintReportRepository
    {
        string _pathDB;
        SprintReportRepository _sprintReportRepository;
        readonly string _idPattern = @"^id=\d+";
        readonly string conditionPattern = @"^condition=\w+";
        readonly string timeOfCreationPattern = @"^timeOfCreation=\d{2}\.\d{2}\.\d{4}\d{2}:\d{2}:\d{2}";
        readonly string spritReportsPattern = @"^sprintReports = [\d,]+";
        readonly string report = @"^REPORT:";

        public CommandSprintReportRepository(string pathDB, SprintReportRepository sprintReportRepository)
        {
            _pathDB = pathDB;
            _sprintReportRepository = sprintReportRepository;
        }

        public IEnumerable<CommandSprintReport> GetEnumerable()
        {
            List<CommandSprintReport> sprintReports = new List<CommandSprintReport>();
            using (var sr = new StreamReader(_pathDB))
            {
                string s;
                CommandSprintReport sprintReport = null;
                while ((s = sr.ReadLine()) != null)
                {
                    var tempWithoutSpaces = s.Replace(" ", string.Empty);
                    if ("[CommandSprintReport]" == s.Replace(" ", string.Empty))
                        sprintReport = new CommandSprintReport();
                    else if (Regex.IsMatch(tempWithoutSpaces, _idPattern))
                    {
                        sprintReport.Id = int.Parse(tempWithoutSpaces.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, conditionPattern))
                    {
                        sprintReport.Condition = stringToEnum(tempWithoutSpaces.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, timeOfCreationPattern))
                    {
                        sprintReport.TimeOfCreation = DateTime.Parse(s.Split('=')[1]);
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, spritReportsPattern))
                    {
                        var idsOfReports = tempWithoutSpaces.Split('=')[1].Split(',');
                        foreach (var id in idsOfReports)
                        {
                            sprintReport.SprintReports.Add(_sprintReportRepository.Get(int.Parse(id)));
                        }
                    }
                    else if (Regex.IsMatch(tempWithoutSpaces, report))
                    {
                        if (!ParseReport(sprintReport, sr))
                            sprintReports.Add(sprintReport);
                        else
                        {
                            sprintReports.Add(sprintReport);
                            sprintReport = new CommandSprintReport();
                        }
                    }
                }
            }
            return sprintReports.ToArray();
        }

        private bool ParseReport(CommandSprintReport report, StreamReader sr)
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                if (s == "[CommandSprintReport]")
                    return false;
                else
                {
                    report.Report += s;
                }
            }
            return true;
        }

        public CommandSprintReport Get(int id)
        {
            return GetEnumerable().Single(item => item.Id == id);
        }

        public void Replace(CommandSprintReport report)
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

        public void Add(CommandSprintReport report, bool created = false)
        {
            var all = GetEnumerable();
            var allLines = File.ReadAllLines(_pathDB).ToList();
            allLines.Add("");
            allLines.Add("[CommandSprintReport]");
            if (created)
                allLines.Add($"id = {report.Id}");
            else
                allLines.Add($"id = {all.Count()}");
            allLines.Add($"condition = {EnumToString(report.Condition)}");
            allLines.Add($"timeOfCreation = {report.TimeOfCreation}");
            string sprintReports = string.Empty;
            foreach (var reportId in report.SprintReports)
            {
                sprintReports += reportId.ToString() + ',';
                if (reportId == report.SprintReports.Last())
                    sprintReports = sprintReports.Remove(sprintReports.Count() - 1);
            }
            allLines.Add($"sprintReports = {sprintReports}");
            allLines.Add("REPORT:");
            allLines.Add(report.Report);
            allLines.Add("");
            File.WriteAllLines(_pathDB, allLines.ToArray());
            report.Id = all.Count();
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