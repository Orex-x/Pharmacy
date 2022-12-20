namespace Pharmacy.Models;

public class Buyer
{
    public int Id { get; set; }
    public string SurnameBuyer { get; set; }
    public string NameBuyer { get; set; }
    public string MiddleNameBuyer { get; set; }
    public string PhoneNumberBuyer { get; set; }

    public int LoyaltyCard { get; set; }
    public User? UserChanges { get; set; }
}