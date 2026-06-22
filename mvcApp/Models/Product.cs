using System.ComponentModel.DataAnnotations;
namespace mvcApp.Models
{
    public class Product
    {
        [Key]
        public int ID {get; set;}

        public string ProductName {get; set;} = null!;

        public string Color {get; set;} = null!;

        public decimal Price {get; set;} = 0.00m;
        
        public string Description {get; set;} = null!;


        
    }
}