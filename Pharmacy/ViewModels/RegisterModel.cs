using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels;

public class RegisterModel
{
    [Required(ErrorMessage ="First name not specified")]
    public string Name { get; set; }
    
    [Required(ErrorMessage ="Login not specified")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "Password not specified")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
         
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password entered incorrectly")]
    public string ConfirmPassword { get; set; }
}