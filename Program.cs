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
    private Manager mgr {  get; set; }

    public Owner(string name, int age, string title)
    {
        this.name = name;
        this.age = age;
        this.title = title;
    }
}

public abstract class Employee : XYZ_P
{
    private double salary; // Private salary field
    private Blacksmith blacksmith;

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
}

public class Blacksmith : Employee
{
    private static readonly int STIPEND = 35000;
    private Task task { get; set; }
    public Blacksmith(string name, int age, string title, double salary) : base(name, age, title, salary)
    {
    }

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
}

public class Task
{
    private int id { get; set; }
    private DateTime date { get; set; }
    private string description { get; set; }

    public Task(int id, DateTime date, string description)
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
        Owner owner = new Owner("Craig", 1, "The Owner");
        Employee manager = new Manager("Alice", 1, "IT Manager", 100000, owner);
        Blacksmith blacksmith = new Blacksmith("Bob", 2, "Blacksmith", 50000);
        Accountant accountant = new Accountant("Charlie", 2, "Senior Accountant", 10000, owner);

        Console.WriteLine($"Manager's Salary (Before): {manager.GetSalary()}");

        // Accountant changes the Manager's salary
        accountant.ChangeSalary(120000, manager);
        manager.ChangeSalary(12, blacksmith);
        Console.WriteLine($"Manager's Salary (After): {manager.GetSalary()}");

    }
}
