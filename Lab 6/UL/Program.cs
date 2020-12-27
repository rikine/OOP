using System;
using DAL;
using BLL;
using UL.Staff;
using UL.Report;
using BLL.Condtion;
using System.Collections.Generic;

namespace UL
{
    class Program
    {
        static void Main(string[] args)
        {
            Employees employees = new Employees("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab6/DataBase");
            EmployeesService employeesService = new EmployeesService(employees);
            EmployeeController employeeController = new EmployeeController(employeesService);

            ProblemRepositoryDAL problemRepositoryDAL = new ProblemRepositoryDAL("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab6/DataBaseProblems", employees);
            ProblemService problemService = new ProblemService(problemRepositoryDAL);
            ProblemController problemController = new ProblemController(problemService);

            DayReportRepository dayReportRepository = new DayReportRepository("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab6/DateBaseDayReport", employees, problemRepositoryDAL);
            DayReportService dayReportService = new DayReportService(dayReportRepository);
            ReportDayController reportDayController = new ReportDayController(dayReportService);

            SprintReportRepository sprintReportRepository = new SprintReportRepository("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab6/DateBaseSprintReport", employees, dayReportRepository);
            SprintReportService sprintReportService = new SprintReportService(sprintReportRepository);
            ReportSprintController reportSprintController = new ReportSprintController("COMMAND_REPORT_DO_YOUR_BEST", sprintReportService);

            CommandSprintReportRepository commandSprintReportRepository = new CommandSprintReportRepository("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab6/DateBaseCommandSprintReport", sprintReportRepository);
            CommandSprintReportSevice commandSprintReportSevice = new CommandSprintReportSevice(commandSprintReportRepository);
            ReportCommandController reportCommandController = new ReportCommandController(commandSprintReportSevice, reportSprintController);

            var dayReport1 = new DayReport(employeeController.Get(4));
            reportDayController.Add(dayReport1);
            reportDayController.AddComment(dayReport1, "greuahgeirugh");
            var problem1 = problemController.GetProblem(0);
            problemController.ChangeConditionOfProblem(problem1, ConditionOfProblem.Resolved, employeeController.Get(4));
            reportDayController.AddReadyProblem(dayReport1, problem1);
            var listOfReports = new List<DayReport>();
            listOfReports.Add(dayReport1);
            var sprintReport1 = new SprintReport(employeeController.Get(4), listOfReports);
            reportSprintController.Add(sprintReport1);
            reportSprintController.AddComment(sprintReport1, "fjghruieajewrg");
            reportSprintController.CloseAndSaveSprint(sprintReport1);
            var commandReport = new CommandSprintReport();
            reportCommandController.Add(commandReport);
            reportCommandController.AddSprintReport(commandReport, sprintReport1);

            var dayReport2 = new DayReport(employeeController.Get(3));
            reportDayController.Add(dayReport2);
            var problem2 = problemController.GetProblem(1);
            problemController.AddComment(problem2, "fguhreiawejgorae", employeeController.Get(3));
            problemController.ChangeConditionOfProblem(problem2, ConditionOfProblem.Resolved, employeeController.Get(3));
            reportDayController.AddReadyProblem(dayReport2, problem2);
            var listOfReports1 = new List<DayReport>();
            listOfReports1.Add(dayReport2);
            var sprintReport2 = new SprintReport(employeeController.Get(3), listOfReports1);
            reportSprintController.Add(sprintReport2);
            reportSprintController.CloseAndSaveSprint(sprintReport2);
            reportCommandController.AddSprintReport(commandReport, sprintReport2);
            reportCommandController.CloseCommandSprintReport(commandReport);
            /*var slave1 = new StaffUL("ahhahaha", employeeController.Get(1));
            employeeController.Add(slave1);
            var slave2 = new StaffUL("Fucking_Slave", employeeController.Get(2));
            employeeController.Add(slave2);
            var person = new DirectorUL("Fucking_director", employeeController.Get(1));
            person.AddSlave(employeeController.Get(7));
            person.AddSlave(employeeController.Get(8));
            employeeController.Add(person);
            Console.WriteLine(employeeController.GetTreeOfEmployees().Print());
            employeesService.UpdateManager(employeesService.Get(7), employeesService.Get(1));
            var problemService = new ProblemService();
            var problemController = new ProblemController(problemService);
            var problem1 = problemController.MakeProblem("OOP LAB 6", "REPORTS AND MULTILAYER STRUCTURE", slave1);
            var staff2 = employeeController.Get(2);
            var problem2 = problemController.MakeProblem("OOP LAB 5", "BANKS AND INFO STRUCTURE", staff2);
            problemController.AddComment(problem1, "DONEEEEEEE!!!!! YUHU -3", slave1);
            problemController.ChangeConditionOfProblem(problem1, ConditionOfProblem.Resolved, slave1);
            slave1.AddReadyProblem(problem1);
            slave1.AddCommentToReport("Cdelal to to toto");
            problemController.AddComment(problem2, "Hiche ne polychaetsya((((", staff2);
            problemController.AddComment(problem2, "YAY SMOG", staff2);
            problemController.ChangeConditionOfProblem(problem2, ConditionOfProblem.Resolved, staff2);
            staff2.AddReadyProblem(problem2);
            staff2.AddCommentToReport("LALlalaal");
            //ok, go sprint
            var reportController = new ReportController();
            reportController.MakeSprintReport(slave1);
            reportController.MakeSprintReport(staff2);
            reportController.AddCommentSprint(slave1, "ZABIL DODELAT'");
            reportController.SaveAndCloseSprintReport(slave1);
            reportController.SaveAndCloseSprintReport(staff2);
            reportController.CreateCommandSprintReport();*/
        }
    }
}
