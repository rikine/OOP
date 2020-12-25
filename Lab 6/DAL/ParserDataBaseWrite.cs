using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DAL.Staff;

namespace DAL
{
    class ParserDataBaseWrite
    {
        private string _pathDB;

        public ParserDataBaseWrite(string pathDB)
        {
            _pathDB = pathDB;
        }

        public void AddPerson(IStaff person, IEnumerable<IStaff> allStaff, List<int> managerIds = null)
        {
            if (person as Director != null)
            {
                (person as Director).Id = allStaff.Count();
                UpdateWhileAddingManager(person as Director, managerIds);
            }
            else if (person as DALStaff != null)
            {
                (person as DALStaff).Id = allStaff.Count();
                UpdateSlavesOfManagerWhileAdding(person as DALStaff, allStaff);
            }
            using (var sw = new StreamWriter(_pathDB, true))
            {
                sw.WriteLine();
                if (person as TeamLead != null)
                {
                    AddPerson(person as TeamLead, allStaff.Count(), sw);
                }
                else if (person as Director != null)
                {
                    AddPerson(person as Director, sw, managerIds);
                }
                else
                {
                    AddPerson(person as DALStaff, sw);
                }
            }
        }

        private void AddPerson(TeamLead teamLead, int countOfAllStuff, StreamWriter sw)
        {
            teamLead.Id = countOfAllStuff;
            sw.WriteLine();
            sw.WriteLine("[TeamLead]");
            sw.WriteLine($"id = {teamLead.Id}");
            sw.WriteLine($"name = {teamLead.Name}");
            string s = string.Empty;
            foreach (var slave in teamLead.Slaves)
            {
                if (slave != teamLead.Slaves.Last())
                    s += slave.Id.ToString() + ',';
                else
                    s += slave.Id.ToString();
            }
            sw.WriteLine($"slaves = {s}");
        }

        private void AddPerson(Director director, StreamWriter sw, List<int> managerIds)
        {
            sw.WriteLine();
            sw.WriteLine("[Director]");
            sw.WriteLine($"id = {director.Id}");
            sw.WriteLine($"name = {director.Name}");
            string s = string.Empty;
            foreach (var slave in director.Slaves)
            {
                if (slave != director.Slaves.Last())
                    s += slave.Id.ToString() + ',';
                else
                    s += slave.Id.ToString();
            }
            sw.WriteLine($"slaves = {s}");
        }

        private void UpdateWhileAddingManager(Director director, List<int> managerIds)
        {
            var allLinesOfFile = File.ReadAllLines(_pathDB).ToList();
            var indexOfManager = allLinesOfFile.FindIndex(
                person => person.Replace(" ", string.Empty) == $"id={director.Manager.Id.ToString()}");
            var temp = allLinesOfFile[indexOfManager + 2].Replace(" ", string.Empty).Split('=');
            if (temp.Count() == 2)
                allLinesOfFile[indexOfManager + 2] += ',' + director.Id.ToString();
            else
                allLinesOfFile[indexOfManager + 2] += director.Id.ToString();

            foreach (var managerId in managerIds)
            {
                foreach (var slave in director.Slaves)
                {
                    var indexOfManagerStr = allLinesOfFile.FindIndex(
                        person => person.Replace(" ", string.Empty) == $"id={managerId.ToString()}");
                    allLinesOfFile[indexOfManagerStr + 2] = allLinesOfFile[indexOfManagerStr + 2].Replace($"{slave.Id},", string.Empty);
                    allLinesOfFile[indexOfManagerStr + 2] = allLinesOfFile[indexOfManagerStr + 2].Replace($",{slave.Id}", string.Empty);
                    allLinesOfFile[indexOfManagerStr + 2] = allLinesOfFile[indexOfManagerStr + 2].Replace($"{slave.Id}", string.Empty);
                }
            }
            File.WriteAllLines(_pathDB, allLinesOfFile.ToArray());
        }

        private void AddPerson(DALStaff staff, StreamWriter sw)
        {
            sw.WriteLine();
            sw.WriteLine("[Staff]");
            sw.WriteLine($"id = {staff.Id}");
            sw.WriteLine($"name = {staff.Name}");
        }

        private void UpdateSlavesOfManagerWhileAdding(DALStaff staff, IEnumerable<IStaff> allStaffs)
        {
            var allLinesOfFile = File.ReadAllLines(_pathDB).ToList();
            var indexOfManager = allLinesOfFile.FindIndex(
                person => person.Replace(" ", string.Empty) == $"id={staff.Manager.Id.ToString()}");
            var temp = allLinesOfFile[indexOfManager + 2].Replace(" ", string.Empty).Split('=');
            if (temp.Count() == 2)
                allLinesOfFile[indexOfManager + 2] += ',' + staff.Id.ToString();
            else
                allLinesOfFile[indexOfManager + 2] += staff.Id.ToString();
            File.WriteAllLines(_pathDB, allLinesOfFile.ToArray());
        }
    }
}