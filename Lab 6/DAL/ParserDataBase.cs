using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAL.Staff;

namespace DAL
{
    class ParserDataBase
    {
        private string _pathDB;
        private List<IStaff> allStaff = new List<IStaff>();
        private Dictionary<int, List<int>> slaves = new Dictionary<int, List<int>>();

        public ParserDataBase(string pathDB)
        {
            _pathDB = pathDB;
        }

        public IEnumerable<IStaff> GetStaffs()
        {
            using (var sr = new StreamReader(_pathDB))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    switch (s.Replace(" ", string.Empty))
                    {
                        case "[TeamLead]":
                            allStaff.Add(CreatePerson(new TeamLead(), slaves, sr));
                            break;
                        case "[Director]":
                            allStaff.Add(CreatePerson(new Director(), slaves, sr));
                            break;
                        case "[Staff]":
                            allStaff.Add(CreatePerson(new DALStaff(), sr));
                            break;
                        case "":
                            continue;
                        default:
                            throw new ParseException($"Unknown staff in DataBase");
                    }
                }
            }

            foreach (var key in slaves.Keys)
            {
                foreach (var id in slaves[key])
                {
                    IStaff staff = allStaff.Find(person => person.Id == key);
                    if (staff as TeamLead != null)
                    {
                        UpdatePerson(staff as TeamLead, id, slaves, allStaff);
                    }
                    else if (staff as Director != null)
                    {
                        UpdatePerson(staff as Director, id, slaves, allStaff);
                    }
                    else
                    {
                        throw new ParseException("TeamLead can't be a slave");
                    }
                }
            }
            return allStaff;
        }

        private IStaff CreatePerson(TeamLead teamLead, Dictionary<int, List<int>> slaves, StreamReader sr)
        {
            teamLead.Id = int.Parse(sr.ReadLine().Replace(" ", string.Empty).Split('=')[1]);
            teamLead.Name = sr.ReadLine().Replace(" ", string.Empty).Split('=')[1];
            var slaves1 = sr.ReadLine().Replace(" ", string.Empty).Split('=');
            if (slaves1[1] != string.Empty)
            {
                slaves.Add(teamLead.Id.Value, new List<int>());
                slaves[teamLead.Id.Value].AddRange(from id in slaves1[1].Split(',')
                                                   select int.Parse(id));
            }
            return teamLead;
        }

        private IStaff CreatePerson(Director director, Dictionary<int, List<int>> slaves, StreamReader sr)
        {
            director.Id = int.Parse(sr.ReadLine().Replace(" ", string.Empty).Split('=')[1]);
            director.Name = sr.ReadLine().Replace(" ", string.Empty).Split('=')[1];
            var slaves1 = sr.ReadLine().Replace(" ", string.Empty).Split('=');
            if (slaves1[1] != string.Empty)
            {
                slaves.Add(director.Id.Value, new List<int>());
                slaves[director.Id.Value].AddRange(from id in slaves1[1].Split(',')
                                                   select int.Parse(id));
            }
            return director;
        }

        private IStaff CreatePerson(DALStaff staff, StreamReader sr)
        {
            staff.Id = int.Parse(sr.ReadLine().Replace(" ", string.Empty).Split('=')[1]);
            staff.Name = sr.ReadLine().Replace(" ", string.Empty).Split('=')[1];
            return staff;
        }

        private void UpdatePerson(TeamLead teamLead, int id, Dictionary<int, List<int>> slaves, List<IStaff> allStaff)
        {
            var slave = allStaff.Find(person => person.Id == id);
            teamLead.AddSlave(slave);
            if (slave as DALStaff != null)
            {
                var slave1 = slave as DALStaff;
                slave1.Manager = teamLead;
            }
            else if (slave as Director != null)
            {
                var slave1 = slave as Director;
                slave1.Manager = teamLead;
            }
            else
            {
                throw new ParseException($"Some error with id{id}");
            }
        }

        private void UpdatePerson(Director director, int id, Dictionary<int, List<int>> slaves, List<IStaff> allStaff)
        {
            var slave = allStaff.Find(person => person.Id == id);
            director.AddSlave(slave);
            if (slave as DALStaff != null)
            {
                var slave1 = slave as DALStaff;
                slave1.Manager = director;
            }
            else if (slave as Director != null)
            {
                var slave1 = slave as Director;
                slave1.Manager = director;
            }
            else
            {
                throw new ParseException($"Some error with id{id}");
            }
        }

    }
}