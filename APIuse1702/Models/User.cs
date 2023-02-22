using System.ComponentModel.DataAnnotations;

namespace APIuse1702.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
