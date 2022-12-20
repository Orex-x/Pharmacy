namespace Pharmacy.Models;

public class Product
{
    public int Id { get; set; }
    public double Price { get; set; }
    public string ExpirationDate { get; set; }
    public string ProductName { get; set; }

    public Category? CategoryChanges { get; set; }
    public Form? FormChanges { get; set; }
    public Manufacturer? ManufacturerChanges { get; set; }
}