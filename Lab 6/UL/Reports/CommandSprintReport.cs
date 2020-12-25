using System;
using System.Collections.Generic;

namespace UL.Report
{
    public class CommandSprintReport
    {
        public ConditionOfSprint condition { get; private set; }
        public DateTime timeOfCreation { get; }
        List<SprintReport> sprintReports = new List<SprintReport>();

        public CommandSprintReport()
        {
            timeOfCreation = DateTime.Now;
            condition = ConditionOfSprint.Open;
        }

        public void AddSprintReport(SprintReport sprint)
        {
            if (condition == ConditionOfSprint.Closed)
                throw new ApplicationException("CommandSprintReport sprint is closed");
            if (sprint.Condition == ConditionOfSprint.Closed)
                sprintReports.Add(sprint);
            else
                throw new ApplicationException("Can not add not ready sprint in command report");
        }

        public void CloseCommandReport()
        {
            condition = ConditionOfSprint.Closed;
        }
    }
}