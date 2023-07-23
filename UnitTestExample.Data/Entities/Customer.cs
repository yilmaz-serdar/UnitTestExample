using System.ComponentModel.DataAnnotations;

namespace UnitTestExample.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Job { get; set; }
    }
}
