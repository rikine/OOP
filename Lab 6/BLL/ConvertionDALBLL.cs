using BLL.Staff;

namespace BLL.Convertion
{
    class ConvertionDALBLL
    {

        public DAL.Staff.TeamLead FindTeamLeadDAL(DAL.Staff.Director directorDAL)
        {
            if (directorDAL.Manager as DAL.Staff.TeamLead != null)
                return directorDAL.Manager as DAL.Staff.TeamLead;
            else if (directorDAL.Manager as DAL.Staff.Director != null)
                return FindTeamLeadDAL(directorDAL.Manager as DAL.Staff.Director);
            return FindTeamLeadDAL(directorDAL.Manager as DAL.Staff.DALStaff);
        }

        public DAL.Staff.TeamLead FindTeamLeadDAL(DAL.Staff.DALStaff staffDAL)
        {
            if (staffDAL.Manager as DAL.Staff.TeamLead != null)
                return staffDAL.Manager as DAL.Staff.TeamLead;
            else if (staffDAL.Manager as DAL.Staff.Director != null)
                return FindTeamLeadDAL(staffDAL.Manager as DAL.Staff.Director);
            return FindTeamLeadDAL(staffDAL.Manager as DAL.Staff.DALStaff);
        }

        public TeamLeadBLL FindTeamLeadBLL(DirectorBLL directorBLL)
        {
            if (directorBLL.Manager as TeamLeadBLL != null)
                return directorBLL.Manager as TeamLeadBLL;
            else if (directorBLL.Manager as DirectorBLL != null)
                return FindTeamLeadBLL(directorBLL.Manager as DirectorBLL);
            return FindTeamLeadBLL(directorBLL.Manager as StaffBLL);
        }

        public TeamLeadBLL FindTeamLeadBLL(StaffBLL staffBLL)
        {
            if (staffBLL.Manager as TeamLeadBLL != null)
                return staffBLL.Manager as TeamLeadBLL;
            else if (staffBLL.Manager as DirectorBLL != null)
                return FindTeamLeadBLL(staffBLL.Manager as DirectorBLL);
            return FindTeamLeadBLL(staffBLL.Manager as StaffBLL);
        }

        public DAL.Staff.TeamLead ConvertBLLToDALPart(TeamLeadBLL teamLeadBLL)
        {
            var teamLeadDAL = new DAL.Staff.TeamLead();
            teamLeadDAL.Id = teamLeadBLL.Id;
            if (teamLeadDAL.Name != null)
                return teamLeadDAL;
            teamLeadDAL.Name = teamLeadBLL.Name;
            foreach (var slave in teamLeadBLL.Slaves)
            {
                if (slave as StaffBLL != null)
                {
                    teamLeadDAL.AddSlave(ConvertBLLToDALPart(slave as StaffBLL));
                }
                else
                {
                    teamLeadDAL.AddSlave(ConvertBLLToDALPart(slave as DirectorBLL));
                }
            }
            return teamLeadDAL;
        }

        public TeamLeadBLL ConvertDALToBLLPart(DAL.Staff.TeamLead teamLeadDAL)
        {
            var teamLeadBLL = new TeamLeadBLL();
            teamLeadBLL.Id = teamLeadDAL.Id.Value;
            if (teamLeadBLL.Name != null)
                return teamLeadBLL;
            teamLeadBLL.Name = teamLeadDAL.Name;
            foreach (var slave in teamLeadDAL.Slaves)
            {
                if (slave as DAL.Staff.DALStaff != null)
                {
                    teamLeadBLL.AddSlave(ConvertDALToBLLPart(slave as DAL.Staff.DALStaff));
                }
                else
                {
                    teamLeadBLL.AddSlave(ConvertDALToBLLPart(slave as DAL.Staff.Director));
                }
            }
            return teamLeadBLL;
        }

        public DAL.Staff.Director ConvertBLLToDALPart(DirectorBLL directorBLL)
        {
            var directorDAL = new DAL.Staff.Director();
            directorDAL.Id = directorBLL.Id;
            if (directorDAL.Name != null)
                return directorDAL;
            directorDAL.Name = directorBLL.Name;
            foreach (var slave in directorBLL.Slaves)
            {
                if (slave as StaffBLL != null)
                {
                    directorDAL.AddSlave(ConvertBLLToDALPart(slave as StaffBLL));
                }
                else
                {
                    directorDAL.AddSlave(ConvertBLLToDALPart(slave as DirectorBLL));
                }
            }
            return directorDAL;
        }

        public DirectorBLL ConvertDALToBLLPart(DAL.Staff.Director directorDAL)
        {
            var directorBLL = new DirectorBLL();
            directorBLL.Id = directorDAL.Id.Value;
            if (directorBLL.Name != null)
                return directorBLL;
            directorBLL.Name = directorDAL.Name;
            foreach (var slave in directorDAL.Slaves)
            {
                if (slave as DAL.Staff.DALStaff != null)
                {
                    directorBLL.AddSlave(ConvertDALToBLLPart(slave as DAL.Staff.DALStaff));
                }
                else
                {
                    directorBLL.AddSlave(ConvertDALToBLLPart(slave as DAL.Staff.Director));
                }
            }
            return directorBLL;
        }

        public DAL.Staff.DALStaff ConvertBLLToDALPart(StaffBLL staffBLL)
        {
            var staffDAL = new DAL.Staff.DALStaff();
            staffDAL.Id = staffBLL.Id;
            staffDAL.Name = staffBLL.Name;
            return staffDAL;
        }

        public StaffBLL ConvertDALToBLLPart(DAL.Staff.DALStaff staffDAL)
        {
            var staffBLL = new StaffBLL();
            staffBLL.Id = staffDAL.Id.Value;
            staffBLL.Name = staffDAL.Name;
            return staffBLL;
        }

        public void UpdateManagersBLL(TeamLeadBLL teamLeadBLL)
        {
            foreach (var slave in teamLeadBLL.Slaves)
            {
                if (slave as DirectorBLL != null)
                {
                    if ((slave as DirectorBLL).Manager != null)
                        break;
                    (slave as DirectorBLL).Manager = teamLeadBLL;
                    UpdateManagersBLL(slave as DirectorBLL);
                }
                else
                {
                    if ((slave as StaffBLL).Manager != null)
                        break;
                    (slave as StaffBLL).Manager = teamLeadBLL;
                }
            }
        }

        public void UpdateManagersBLL(DirectorBLL directorBLL)
        {
            foreach (var slave in directorBLL.Slaves)
            {
                if (slave as DirectorBLL != null)
                {
                    if ((slave as DirectorBLL).Manager != null)
                        break;
                    (slave as DirectorBLL).Manager = directorBLL;
                    UpdateManagersBLL(slave as DirectorBLL);
                }
                else
                {
                    if ((slave as StaffBLL).Manager != null)
                        break;
                    (slave as StaffBLL).Manager = directorBLL;
                }
            }
        }

        public void UpdateManagersDAL(DAL.Staff.TeamLead teamLeadDAL)
        {
            foreach (var slave in teamLeadDAL.Slaves)
            {
                if (slave as DAL.Staff.Director != null)
                {
                    if ((slave as DAL.Staff.Director).Manager != null)
                        break;
                    (slave as DAL.Staff.Director).Manager = teamLeadDAL;
                    UpdateManagersDAL(slave as DAL.Staff.Director);
                }
                else
                {
                    if ((slave as DAL.Staff.DALStaff).Manager != null)
                        break;
                    (slave as DAL.Staff.DALStaff).Manager = teamLeadDAL;
                }
            }
        }

        public void UpdateManagersDAL(DAL.Staff.Director directorDAL)
        {
            foreach (var slave in directorDAL.Slaves)
            {
                if (slave as DAL.Staff.Director != null)
                {
                    if ((slave as DAL.Staff.Director).Manager != null)
                        break;
                    (slave as DAL.Staff.Director).Manager = directorDAL;
                    UpdateManagersDAL(slave as DAL.Staff.Director);
                }
                else
                {
                    if ((slave as DAL.Staff.DALStaff).Manager != null)
                        break;
                    (slave as DAL.Staff.DALStaff).Manager = directorDAL;
                }
            }
        }

        public DAL.Staff.TeamLead ConvertBLLToDAL(TeamLeadBLL teamLeadBLL)
        {
            var staff = ConvertBLLToDALPart(teamLeadBLL);
            UpdateManagersDAL(staff);
            return staff;
        }

        public DAL.Staff.Director ConvertBLLToDAL(DirectorBLL directorBLL)
        {
            if (directorBLL.Manager as DirectorBLL != null)
            {
                if ((directorBLL.Manager as DirectorBLL).Slaves.Find(pers => pers.Id == directorBLL.Id) == null)
                    (directorBLL.Manager as DirectorBLL).AddSlave(directorBLL);
            }
            else
            {
                if ((directorBLL.Manager as TeamLeadBLL).Slaves.Find(pers => pers.Id == directorBLL.Id) == null)
                    (directorBLL.Manager as TeamLeadBLL).AddSlave(directorBLL);
            }
            var teamLeadBLL = FindTeamLeadBLL(directorBLL);
            var staff = ConvertBLLToDALPart(teamLeadBLL);
            UpdateManagersDAL(staff);
            return FindByDAL(staff, directorBLL) as DAL.Staff.Director;
        }

        public DAL.Staff.DALStaff ConvertBLLToDAL(StaffBLL staff)
        {
            if (staff.Manager as DirectorBLL != null)
            {
                if ((staff.Manager as DirectorBLL).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as DirectorBLL).AddSlave(staff);
            }
            else
            {
                if ((staff.Manager as TeamLeadBLL).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as TeamLeadBLL).AddSlave(staff);
            }
            var teamLeadBLL = FindTeamLeadBLL(staff);
            var staff1 = ConvertBLLToDALPart(teamLeadBLL);
            UpdateManagersDAL(staff1);
            return FindByDAL(staff1, staff) as DAL.Staff.DALStaff;
        }

        public TeamLeadBLL ConvertDALToBLL(DAL.Staff.TeamLead teamLeadDAL)
        {
            var staff = ConvertDALToBLLPart(teamLeadDAL);
            UpdateManagersBLL(staff);
            return staff;
        }

        public DirectorBLL ConvertDALToBLL(DAL.Staff.Director directorDAL)
        {
            if (directorDAL.Manager as DAL.Staff.Director != null)
            {
                if ((directorDAL.Manager as DAL.Staff.Director).Slaves.Find(pers => pers.Id == directorDAL.Id) == null)
                    (directorDAL.Manager as DAL.Staff.Director).AddSlave(directorDAL);
            }
            else
            {
                if ((directorDAL.Manager as DAL.Staff.TeamLead).Slaves.Find(pers => pers.Id == directorDAL.Id) == null)
                    (directorDAL.Manager as DAL.Staff.TeamLead).AddSlave(directorDAL);
            }
            var teamLeadDAL = FindTeamLeadDAL(directorDAL);
            var staff = ConvertDALToBLLPart(teamLeadDAL);
            UpdateManagersBLL(staff);
            return FindByBLL(staff, directorDAL) as DirectorBLL;
        }

        public StaffBLL ConvertDALToBLL(DAL.Staff.DALStaff staff)
        {
            if (staff.Manager as DAL.Staff.Director != null)
            {
                if ((staff.Manager as DAL.Staff.Director).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as DAL.Staff.Director).AddSlave(staff);
            }
            else
            {
                if ((staff.Manager as DAL.Staff.TeamLead).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as DAL.Staff.TeamLead).AddSlave(staff);
            }
            var teamLeadDAL = FindTeamLeadDAL(staff);
            var staff1 = ConvertDALToBLLPart(teamLeadDAL);
            UpdateManagersBLL(staff1);
            return FindByBLL(staff1, staff) as StaffBLL;
        }

        public IStaffBLL FindByBLL(TeamLeadBLL teamLead, DAL.Staff.IStaff staff)
        {
            IStaffBLL staffBLL = null;
            foreach (var slave in teamLead.Slaves)
            {
                if (slave.EqualsDAL(staff))
                {
                    staffBLL = slave;
                    break;
                }
                else if (slave as DirectorBLL != null)
                {
                    staffBLL = FindByBLL(slave as DirectorBLL, staff);
                    if (staffBLL != null)
                        break;
                }
            }
            return staffBLL;
        }

        public IStaffBLL FindByBLL(DirectorBLL directorBLL, DAL.Staff.IStaff staff)
        {
            IStaffBLL staffBLL = null;
            foreach (var slave in directorBLL.Slaves)
            {
                if (slave.EqualsDAL(staff))
                {
                    staffBLL = slave;
                    break;
                }
                else if (slave as DirectorBLL != null)
                {
                    staffBLL = FindByBLL(slave as DirectorBLL, staff);
                    if (staffBLL != null)
                        break;
                }
            }
            return staffBLL;
        }

        public DAL.Staff.IStaff FindByDAL(DAL.Staff.TeamLead teamLead, IStaffBLL staff)
        {
            DAL.Staff.IStaff staffDAL = null;
            foreach (var slave in teamLead.Slaves)
            {
                if (staff.EqualsDAL(slave))
                {
                    staffDAL = slave;
                    break;
                }
                else if (slave as DAL.Staff.Director != null)
                {
                    staffDAL = FindByDAL(slave as DAL.Staff.Director, staff);
                    if (staffDAL != null)
                        break;
                }
            }
            return staffDAL;
        }

        public DAL.Staff.IStaff FindByDAL(DAL.Staff.Director directorDAL, IStaffBLL staff)
        {
            DAL.Staff.IStaff staffDAL = null;
            foreach (var slave in directorDAL.Slaves)
            {
                if (staff.EqualsDAL(slave))
                {
                    staffDAL = slave;
                    break;
                }
                else if (slave as DAL.Staff.Director != null)
                {
                    staffDAL = FindByDAL(slave as DAL.Staff.Director, staff);
                    if (staffDAL != null)
                        break;
                }
            }
            return staffDAL;
        }

    }
}