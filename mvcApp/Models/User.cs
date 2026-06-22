using System.ComponentModel.DataAnnotations;

namespace mvcApp.Models;

public class User
{
    [Key]
    public int ID {get; set;}

    public string UserName {get; set;} = null!;

    public string Email {get; set;} = null!;

    public string Password {get; set;} = null!;
        
}