using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DAL.Staff;

namespace DAL
{
    public class Employees : IEmployees<IStaff>
    {
        private string _pathDB;

        public Employees(string pathDB)
        {
            _pathDB = pathDB;
            if (!File.Exists(_pathDB)) File.Create(_pathDB).Close();
        }

        public void Add(IStaff person)
        {
            var all = GetEnumerable();
            new ParserDataBaseWrite(_pathDB).AddPerson(person, all);
        }

        public void Add(IStaff person, List<int> managerIds)
        {
            var all = GetEnumerable();
            new ParserDataBaseWrite(_pathDB).AddPerson(person, all, managerIds);
        }

        public void Delete(int id)
        {
            var allLinesOfDB = File.ReadAllLines(_pathDB).ToList();
            var numberOfIndexID = allLinesOfDB.FindIndex(
                person => person.Replace(" ", string.Empty) == $"id={id.ToString()}") - 1;
            if (numberOfIndexID == -2)
                throw new DeleteException($"id = {id} was not found");

            var staff = GetEnumerable().Single(item => item.Id == id);
            if (staff as DALStaff != null)
            {
                for (int i = 0; i < 3; i++)
                    allLinesOfDB.RemoveAt(numberOfIndexID);
                var manager = (staff as DALStaff).Manager;
                var indexOfManager = allLinesOfDB.FindIndex(
                    person => person.Replace(" ", string.Empty) == $"id={manager.Id.ToString()}");
                string s = string.Empty;
                if (manager as TeamLead != null)
                {
                    var teamLead = manager as TeamLead;
                    foreach (var slave in teamLead.Slaves)
                    {
                        if (slave.Id == id)
                            continue;
                        if (slave != teamLead.Slaves.Last())
                            s += slave.Id.ToString() + ',';
                        else
                            s += slave.Id.ToString();
                    }
                }
                else
                {
                    var director = manager as Director;
                    foreach (var slave in director.Slaves)
                    {
                        if (slave.Id == id)
                            continue;
                        if (slave != director.Slaves.Last())
                            s += slave.Id.ToString() + ',';
                        else
                            s += slave.Id.ToString();
                    }
                }
                if (s[s.Length - 1] == ',')
                    s = s.Remove(s.Length - 1);
                allLinesOfDB[indexOfManager + 2] = "slaves = " + s;
            }
            else if (staff as TeamLead != null)
            {
                throw new DeleteException($"TeamLead can not be deleted");
            }
            else
            {
                var director = staff as Director;
                if (director.Slaves.Count != 0)
                {
                    var indexOfManager = allLinesOfDB.FindIndex(
                        person => person.Replace(" ", string.Empty) == $"id={director.Manager.Id.ToString()}");
                    allLinesOfDB[indexOfManager + 2] += ',';
                    foreach (var slave in director.Slaves)
                    {
                        if (slave != director.Slaves.Last())
                            allLinesOfDB[indexOfManager + 2] += slave.Id.ToString() + ',';
                        else
                            allLinesOfDB[indexOfManager + 2] += slave.Id.ToString();
                    }
                }
                for (int i = 0; i < 4; i++)
                    allLinesOfDB.RemoveAt(numberOfIndexID);
            }

            File.WriteAllLines(_pathDB, allLinesOfDB.ToArray());
        }

        public IStaff Get(int id)
        {
            return GetEnumerable().Single(person => person.Id == id);
        }

        public IEnumerable<IStaff> GetEnumerable()
        {
            return new ParserDataBase(_pathDB).GetStaffs();
        }

        public TeamLead GetTreeOfEmployees()
        {
            return GetEnumerable().Single(item => item as TeamLead != null) as TeamLead;
        }

        public void UpdateManager(IStaff person, IStaff managerToChange)
        {
            if (person as TeamLead != null)
                throw new UpdatePersonException("TeamLead can not have a manager");
            if (managerToChange as DALStaff != null)
                throw new UpdatePersonException("Slave can not be a manager");
            var allLinesOfDB = File.ReadAllLines(_pathDB).ToList();
            var numberOfIndexID = allLinesOfDB.FindIndex(
                person => person.Replace(" ", string.Empty) == $"id={managerToChange.Id.ToString()}");
            if (numberOfIndexID == -1)
                throw new DeleteException($"id = {managerToChange.Id} was not found");
            var temp = allLinesOfDB[numberOfIndexID + 2].Replace(" ", string.Empty).Split('=');
            if (temp[1] != string.Empty)
                allLinesOfDB[numberOfIndexID + 2] += ',' + person.Id.ToString();
            else
                allLinesOfDB[numberOfIndexID + 2] += person.Id.ToString();

            if (person as DALStaff != null)
            {
                var manager = (person as DALStaff).Manager;
                var indexOfManager = allLinesOfDB.FindIndex(
                    person => person.Replace(" ", string.Empty) == $"id={manager.Id.ToString()}");
                string s = string.Empty;
                if (manager as TeamLead != null)
                {
                    var teamLead = manager as TeamLead;
                    foreach (var slave in teamLead.Slaves)
                    {
                        if (slave.Id == person.Id)
                            continue;
                        if (slave != teamLead.Slaves.Last())
                            s += slave.Id.ToString() + ',';
                        else
                            s += slave.Id.ToString();
                    }
                }
                else
                {
                    var director = manager as Director;
                    foreach (var slave in director.Slaves)
                    {
                        if (slave.Id == person.Id)
                            continue;
                        if (slave != director.Slaves.Last())
                            s += slave.Id.ToString() + ',';
                        else
                            s += slave.Id.ToString();
                    }
                }
                if (s[s.Length - 1] == ',')
                    s = s.Remove(s.Length - 1);
                allLinesOfDB[indexOfManager + 2] = "slaves = " + s;
            }
            else
            {
                var manager = (person as Director).Manager;
                var indexOfManager = allLinesOfDB.FindIndex(
                    person => person.Replace(" ", string.Empty) == $"id={manager.Id.ToString()}");
                string s = string.Empty;
                if (manager as TeamLead != null)
                {
                    var teamLead = manager as TeamLead;
                    foreach (var slave in teamLead.Slaves)
                    {
                        if (slave.Id == person.Id)
                            continue;
                        if (slave != teamLead.Slaves.Last())
                            s += slave.Id.ToString() + ',';
                        else
                            s += slave.Id.ToString();
                    }
                }
                else
                {
                    var director = manager as Director;
                    foreach (var slave in director.Slaves)
                    {
                        if (slave.Id == person.Id)
                            continue;
                        if (slave != director.Slaves.Last())
                            s += slave.Id.ToString() + ',';
                        else
                            s += slave.Id.ToString();
                    }
                }
                if (s[s.Length - 1] == ',')
                    s = s.Remove(s.Length - 1);
                allLinesOfDB[indexOfManager + 2] = "slaves = " + s;
            }
            File.WriteAllLines(_pathDB, allLinesOfDB.ToArray());
        }

    }
}