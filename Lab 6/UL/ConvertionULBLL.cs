using UL.Staff;

namespace UL.Convertion
{
    public class ConvertionULBLL
    {
        private BLL.Staff.TeamLeadBLL FindTeamLeadBLL(BLL.Staff.DirectorBLL directorBLL)
        {
            if (directorBLL.Manager as BLL.Staff.TeamLeadBLL != null)
                return directorBLL.Manager as BLL.Staff.TeamLeadBLL;
            else if (directorBLL.Manager as BLL.Staff.DirectorBLL != null)
                return FindTeamLeadBLL(directorBLL.Manager as BLL.Staff.DirectorBLL);
            return FindTeamLeadBLL(directorBLL.Manager as BLL.Staff.StaffBLL);
        }

        private BLL.Staff.TeamLeadBLL FindTeamLeadBLL(BLL.Staff.StaffBLL staffBLL)
        {
            if (staffBLL.Manager as BLL.Staff.TeamLeadBLL != null)
                return staffBLL.Manager as BLL.Staff.TeamLeadBLL;
            else if (staffBLL.Manager as BLL.Staff.DirectorBLL != null)
                return FindTeamLeadBLL(staffBLL.Manager as BLL.Staff.DirectorBLL);
            return FindTeamLeadBLL(staffBLL.Manager as BLL.Staff.StaffBLL);
        }

        private TeamLeadUL FindTeamLeadUL(DirectorUL directorUL)
        {
            if (directorUL.Manager as TeamLeadUL != null)
                return directorUL.Manager as TeamLeadUL;
            else if (directorUL.Manager as DirectorUL != null)
                return FindTeamLeadUL(directorUL.Manager as DirectorUL);
            return FindTeamLeadUL(directorUL.Manager as StaffUL);
        }

        private TeamLeadUL FindTeamLeadUL(StaffUL staffUL)
        {
            if (staffUL.Manager as TeamLeadUL != null)
                return staffUL.Manager as TeamLeadUL;
            else if (staffUL.Manager as DirectorUL != null)
                return FindTeamLeadUL(staffUL.Manager as DirectorUL);
            return FindTeamLeadUL(staffUL.Manager as StaffUL);
        }

        private BLL.Staff.TeamLeadBLL ConvertULToBLLPart(TeamLeadUL teamLeadUL)
        {
            var teamLeadBLL = new BLL.Staff.TeamLeadBLL();
            teamLeadBLL.Id = teamLeadUL.Id;
            teamLeadBLL.Name = teamLeadUL.Name;
            foreach (var slave in teamLeadUL.Slaves)
            {
                if (slave as StaffUL != null)
                {
                    teamLeadBLL.AddSlave(ConvertULToBLLPart(slave as StaffUL));
                }
                else
                {
                    teamLeadBLL.AddSlave(ConvertULToBLLPart(slave as DirectorUL));
                }
            }
            return teamLeadBLL;
        }

        private TeamLeadUL ConvertBLLToULPart(BLL.Staff.TeamLeadBLL teamLeadBLL)
        {
            var teamLeadUL = new TeamLeadUL();
            teamLeadUL.Id = teamLeadBLL.Id.Value;
            teamLeadUL.Name = teamLeadBLL.Name;
            foreach (var slave in teamLeadBLL.Slaves)
            {
                if (slave as BLL.Staff.StaffBLL != null)
                {
                    teamLeadUL.AddSlave(ConvertBLLToULPart(slave as BLL.Staff.StaffBLL));
                }
                else
                {
                    teamLeadUL.AddSlave(ConvertBLLToULPart(slave as BLL.Staff.DirectorBLL));
                }
            }
            return teamLeadUL;
        }

        private BLL.Staff.DirectorBLL ConvertULToBLLPart(DirectorUL directorUL)
        {
            var directorBLL = new BLL.Staff.DirectorBLL();
            directorBLL.Id = directorUL.Id;
            directorBLL.Name = directorUL.Name;
            foreach (var slave in directorUL.Slaves)
            {
                if (slave as StaffUL != null)
                {
                    directorBLL.AddSlave(ConvertULToBLLPart(slave as StaffUL));
                }
                else
                {
                    directorBLL.AddSlave(ConvertULToBLLPart(slave as DirectorUL));
                }
            }
            return directorBLL;
        }

        private DirectorUL ConvertBLLToULPart(BLL.Staff.DirectorBLL directorBLL)
        {
            var directorUL = new DirectorUL();
            directorUL.Id = directorBLL.Id.Value;
            directorUL.Name = directorBLL.Name;
            foreach (var slave in directorBLL.Slaves)
            {
                if (slave as BLL.Staff.StaffBLL != null)
                {
                    directorUL.AddSlave(ConvertBLLToULPart(slave as BLL.Staff.StaffBLL));
                }
                else
                {
                    directorUL.AddSlave(ConvertBLLToULPart(slave as BLL.Staff.DirectorBLL));
                }
            }
            return directorUL;
        }

        private BLL.Staff.StaffBLL ConvertULToBLLPart(StaffUL staffUL)
        {
            var staffBLL = new BLL.Staff.StaffBLL();
            staffBLL.Id = staffUL.Id;
            staffBLL.Name = staffUL.Name;
            return staffBLL;
        }

        private StaffUL ConvertBLLToULPart(BLL.Staff.StaffBLL staffBLL)
        {
            var staffUL = new StaffUL();
            staffUL.Id = staffBLL.Id.Value;
            staffUL.Name = staffBLL.Name;
            return staffUL;
        }

        private void UpdateManagersUL(TeamLeadUL teamLeadUL)
        {
            foreach (var slave in teamLeadUL.Slaves)
            {
                if (slave as DirectorUL != null)
                {
                    if ((slave as DirectorUL).Manager != null)
                        break;
                    (slave as DirectorUL).Manager = teamLeadUL;
                    UpdateManagersUL(slave as DirectorUL);
                }
                else
                {
                    if ((slave as StaffUL).Manager != null)
                        break;
                    (slave as StaffUL).Manager = teamLeadUL;
                }
            }
        }

        private void UpdateManagersUL(DirectorUL directorUL)
        {
            foreach (var slave in directorUL.Slaves)
            {
                if (slave as DirectorUL != null)
                {
                    if ((slave as DirectorUL).Manager != null)
                        break;
                    (slave as DirectorUL).Manager = directorUL;
                    UpdateManagersUL(slave as DirectorUL);
                }
                else
                {
                    if ((slave as StaffUL).Manager != null)
                        break;
                    (slave as StaffUL).Manager = directorUL;
                }
            }
        }

        private void UpdateManagersBLL(BLL.Staff.TeamLeadBLL teamLeadDAL)
        {
            foreach (var slave in teamLeadDAL.Slaves)
            {
                if (slave as BLL.Staff.DirectorBLL != null)
                {
                    if ((slave as BLL.Staff.DirectorBLL).Manager != null)
                        break;
                    (slave as BLL.Staff.DirectorBLL).Manager = teamLeadDAL;
                    UpdateManagersBLL(slave as BLL.Staff.DirectorBLL);
                }
                else
                {
                    if ((slave as BLL.Staff.StaffBLL).Manager != null)
                        break;
                    (slave as BLL.Staff.StaffBLL).Manager = teamLeadDAL;
                }
            }
        }

        private void UpdateManagersBLL(BLL.Staff.DirectorBLL directorBLL)
        {
            foreach (var slave in directorBLL.Slaves)
            {
                if (slave as BLL.Staff.DirectorBLL != null)
                {
                    if ((slave as BLL.Staff.DirectorBLL).Manager != null)
                        break;
                    (slave as BLL.Staff.DirectorBLL).Manager = directorBLL;
                    UpdateManagersBLL(slave as BLL.Staff.DirectorBLL);
                }
                else
                {
                    if ((slave as BLL.Staff.StaffBLL).Manager != null)
                        break;
                    (slave as BLL.Staff.StaffBLL).Manager = directorBLL;
                }
            }
        }

        public BLL.Staff.TeamLeadBLL ConvertULToBLL(TeamLeadUL teamLeadUL)
        {
            var staff = ConvertULToBLLPart(teamLeadUL);
            UpdateManagersBLL(staff);
            return staff;
        }

        public BLL.Staff.DirectorBLL ConvertULToBLL(DirectorUL directorUL)
        {
            if (directorUL.Manager as DirectorUL != null)
            {
                if ((directorUL.Manager as DirectorUL).Slaves.Find(pers => pers.Id == directorUL.Id) == null)
                    (directorUL.Manager as DirectorUL).AddSlave(directorUL);
            }
            else
            {
                if ((directorUL.Manager as TeamLeadUL).Slaves.Find(pers => pers.Id == directorUL.Id) == null)
                    (directorUL.Manager as TeamLeadUL).AddSlave(directorUL);
            }
            var teamLeadBLL = FindTeamLeadUL(directorUL);
            var staff = ConvertULToBLLPart(teamLeadBLL);
            UpdateManagersBLL(staff);
            return FindByBLL(staff, directorUL) as BLL.Staff.DirectorBLL;
        }

        public BLL.Staff.StaffBLL ConvertULToBLL(StaffUL staff)
        {
            if (staff.Manager as DirectorUL != null)
            {
                if ((staff.Manager as DirectorUL).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as DirectorUL).AddSlave(staff);
            }
            else
            {
                if ((staff.Manager as TeamLeadUL).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as TeamLeadUL).AddSlave(staff);
            }
            var teamLeadBLL = FindTeamLeadUL(staff);
            var staff1 = ConvertULToBLLPart(teamLeadBLL);
            UpdateManagersBLL(staff1);
            return FindByBLL(staff1, staff) as BLL.Staff.StaffBLL;
        }

        public TeamLeadUL ConvertBLLToUL(BLL.Staff.TeamLeadBLL teamLeadBLL)
        {
            var staff = ConvertBLLToULPart(teamLeadBLL);
            UpdateManagersUL(staff);
            return staff;
        }

        public DirectorUL ConvertBLLToUL(BLL.Staff.DirectorBLL directorBLL)
        {
            if (directorBLL.Manager as BLL.Staff.DirectorBLL != null)
            {
                if ((directorBLL.Manager as BLL.Staff.DirectorBLL).Slaves.Find(pers => pers.Id == directorBLL.Id) == null)
                    (directorBLL.Manager as BLL.Staff.DirectorBLL).AddSlave(directorBLL);
            }
            else
            {
                if ((directorBLL.Manager as BLL.Staff.TeamLeadBLL).Slaves.Find(pers => pers.Id == directorBLL.Id) == null)
                    (directorBLL.Manager as BLL.Staff.TeamLeadBLL).AddSlave(directorBLL);
            }
            var teamLeadDAL = FindTeamLeadBLL(directorBLL);
            var staff = ConvertBLLToULPart(teamLeadDAL);
            UpdateManagersUL(staff);
            return FindByUL(staff, directorBLL) as DirectorUL;
        }

        public StaffUL ConvertBLLToUL(BLL.Staff.StaffBLL staff)
        {
            if (staff.Manager as BLL.Staff.DirectorBLL != null)
            {
                if ((staff.Manager as BLL.Staff.DirectorBLL).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as BLL.Staff.DirectorBLL).AddSlave(staff);
            }
            else
            {
                if ((staff.Manager as BLL.Staff.TeamLeadBLL).Slaves.Find(pers => pers.Id == staff.Id) == null)
                    (staff.Manager as BLL.Staff.TeamLeadBLL).AddSlave(staff);
            }
            var teamLeadDAL = FindTeamLeadBLL(staff);
            var staff1 = ConvertBLLToULPart(teamLeadDAL);
            UpdateManagersUL(staff1);
            return FindByUL(staff1, staff) as StaffUL;
        }

        private IStaff FindByUL(TeamLeadUL teamLead, BLL.Staff.IStaffBLL staff)
        {
            IStaff staffUL = null;
            foreach (var slave in teamLead.Slaves)
            {
                if (slave.EqualsBLL(staff))
                {
                    staffUL = slave;
                    break;
                }
                else if (slave as DirectorUL != null)
                {
                    staffUL = FindByUL(slave as DirectorUL, staff);
                    if (staffUL != null)
                        break;
                }
            }
            return staffUL;
        }

        private IStaff FindByUL(DirectorUL directorBLL, BLL.Staff.IStaffBLL staff)
        {
            IStaff staffUL = null;
            foreach (var slave in directorBLL.Slaves)
            {
                if (slave.EqualsBLL(staff))
                {
                    staffUL = slave;
                    break;
                }
                else if (slave as DirectorUL != null)
                {
                    staffUL = FindByUL(slave as DirectorUL, staff);
                    if (staffUL != null)
                        break;
                }
            }
            return staffUL;
        }

        private BLL.Staff.IStaffBLL FindByBLL(BLL.Staff.TeamLeadBLL teamLead, IStaff staff)
        {
            BLL.Staff.IStaffBLL staffBLL = null;
            foreach (var slave in teamLead.Slaves)
            {
                if (staff.EqualsBLL(slave))
                {
                    staffBLL = slave;
                    break;
                }
                else if (slave as BLL.Staff.DirectorBLL != null)
                {
                    staffBLL = FindByBLL(slave as BLL.Staff.DirectorBLL, staff);
                    if (staffBLL != null)
                        break;
                }
            }
            return staffBLL;
        }

        private BLL.Staff.IStaffBLL FindByBLL(BLL.Staff.DirectorBLL directorBLL, IStaff staff)
        {
            BLL.Staff.IStaffBLL staffBLL = null;
            foreach (var slave in directorBLL.Slaves)
            {
                if (staff.EqualsBLL(slave))
                {
                    staffBLL = slave;
                    break;
                }
                else if (slave as BLL.Staff.DirectorBLL != null)
                {
                    staffBLL = FindByBLL(slave as BLL.Staff.DirectorBLL, staff);
                    if (staffBLL != null)
                        break;
                }
            }
            return staffBLL;
        }
    }
}