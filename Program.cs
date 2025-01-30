using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

public abstract class XYZ_P
{
    public string name {  get; set; }
    public int age { get; set; }
    public string title { get; set; }
}

public class Company
{
    public Owner owner { get; set; }
    public Employee[] employees { get; }

    public Company(Owner owner, Employee[] employees) 
    {
        this.owner = owner;
        this.employees = employees;
    }
}

public class Owner : XYZ_P
{
    public Manager mgr { get; set; } 

    public Owner(string name, int age, string title)
    {
        this.name = name;
        this.age = age;
        this.title = title;
        this.mgr = new Manager(name, age, title, 0, this);
    }

    public void send(List<Employee> employees, string message)
    {
        Console.WriteLine("Sending message [ " + message + " ] to:");
        foreach(var employee in employees) Console.WriteLine(employee.name);
    }
}

public abstract class Employee : XYZ_P
{
    private double salary; // Private salary field
    public Blacksmith blacksmith;

    // Constructor
    public Employee(string name, int age, string title, double salary)
    {
        this.name = name;
        this.age = age;
        this.title = title;
        this.salary = salary;
    }

    // Public method to get the salary
    public double GetSalary()
    {
        return salary;
    }

    // Method to allow only the Accountant to change salary
    virtual public void ChangeSalary(double newSalary, Employee emp)
    {
        emp.salary = newSalary;
    }

    public void addBlacksmith(Blacksmith b)
    {
        blacksmith = b;
    }
}

public class Manager : Employee
{
    private static readonly int STIPEND = 50000;
    private Owner owner {  get; set; }
    public Manager(string name, int age, string title, double salary, Owner owner) : base(name, age, title, salary)
    {
        this.owner = owner;
    }

    // overridden method that shows an error for lacking permissions
    override public void ChangeSalary(double newSalary, Employee emp)
    {
        Console.WriteLine("ERROR: You do not have permission to change salaries.");
    }

    public void evaluate(int rank,  Employee emp) { Console.WriteLine(name + " has evaluated " + emp.name + "at a " + rank + "."); }
}

public class Blacksmith : Employee
{
    private static readonly int STIPEND = 35000;

    public Blacksmith(string name, int age, string title, double salary) : base(name, age, title, salary)
    {
    }

    public void perform(WorkTask task) { Console.WriteLine(name + " performed a task."); }

    // overridden method that shows an error for lacking permissions
    override public void ChangeSalary(double newSalary, Employee emp)
    {
        Console.WriteLine("ERROR: You do not have permission to change salaries.");
    }
}

public class Accountant : Employee
{
    private static readonly int STIPEND = 45000;
    private Owner owner { get; set; }
    // Constructor
    public Accountant(string name, int age, string title, double salary, Owner owner) : base(name, age, title, salary)
    {
        this.owner = owner;
    }

    // Accountant method to change the salary of an employee
    override public void ChangeSalary(double newSalary, Employee emp)
    {
        base.ChangeSalary(newSalary, emp);
    }

    public void setSalary(Employee employee, double newSalary) { Console.WriteLine("Updated " + employee.name + "'s salary to $" + newSalary); ChangeSalary(newSalary, employee); }
}

public class WorkTask
{
    private int id { get; set; }
    private DateTime date { get; set; }
    private string description { get; set; }

    public WorkTask(int id, DateTime date, string description)
    {
        this.id = id;
        this.date = date;
        this.description = description;
    }
}

// Example Usage
class Program
{
    static void Main(string[] args)
    {
        // Create the owners and managers
        var craig = new Owner("Craig", 67, "The Owner");
        var john = new Manager("John", 46, "IT Manager", 100000, craig);
        var mary = new Manager("Mary", 38, "Accounting Manager", 100000, craig);

        // Create the accountants
        var jane = new Accountant("Jane", 51, "Senior Accountant", 10000, craig);
        var joe = new Accountant("Joe", 24, "Accountant", 10000, craig);

        // Create the blacksmiths
        var jack = new Blacksmith("Jack", 28, "Blacksmith", 50000);
        var katie = new Blacksmith("Katie", 24, "Blacksmith", 50000);
        var amy = new Blacksmith("Amy", 22, "Blacksmith", 50000);
        var lin = new Blacksmith("Lin", 39, "Blacksmith", 50000);
        var greg = new Blacksmith("Greg", 56, "Blacksmith", 50000);


        // Test cases
        // Craig's Greeting Message
        craig.send(new List<Employee>{ john, jane, jack }, "Good job");

        // Greg performs his task t1
        var t1 = new WorkTask(1, DateTime.Now, "Greg's task.");
        greg.perform(t1);

        // Greg helps amy with t2
        var t2 = new WorkTask(2, DateTime.Now, "Amy's task.");
        greg.addBlacksmith(amy);
        greg.blacksmith.perform(t2);

        // Jane increases greg's salary by $1000
        jane.setSalary(greg, greg.GetSalary() + 1000);

        // Jane helps Lin do t3
        var t3 = new WorkTask(3, DateTime.Now, "Lin's task.");
        jane.addBlacksmith(amy);
        jane.blacksmith.perform(t3);

        // Craig evaluates Jack as 4
        craig.mgr.evaluate(4, jack);
         
        // Mary evaluates Katie as 5
        mary.evaluate(5, katie);
    }
}
