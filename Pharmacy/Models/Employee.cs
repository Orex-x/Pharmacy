namespace Pharmacy.Models;

public class Employee
{
    public int Id { get; set; }
    public string SurnameEmployee { get; set; }
    public string NameEmployee { get; set; }
    public string MiddleNameEmployee { get; set; }
    public string PhoneNumberEmployee { get; set; }

    public User? UserChanges { get; set; }
    public Post? PostChanges { get; set; }
}