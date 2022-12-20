namespace Pharmacy.Models;

public class Cart
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }

    public OrderBuyer? OrderBuyerChanges { get; set; }
    public PlaceOfIssue? PlaceOfIssueChanges { get; set; }
    public Buyer? BuyerChanges { get; set; }
}