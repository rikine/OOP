using System;
using DAL;
using BLL;
using UL.Staff;
using BLL.Task;

namespace UL
{
    class Program
    {
        static void Main(string[] args)
        {
            Employees employees = new Employees("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab6/DataBase");
            EmployeesService employeesService = new EmployeesService(employees);
            EmployeeController employeeController = new EmployeeController(employeesService);
            var slave1 = new StaffUL("ahhahaha", employeeController.Get(1));
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
            reportController.CreateCommandSprintReport();
        }
    }
}
