using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessengerServer.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(20), Required, Index(IsUnique = true)]
        public string Login { get; set; }

        [Required]
        public string Hash { get; set; }

        [MaxLength(40)]
        public string FirstName { get; set; }

        [MaxLength(40)]
        public string LastName { get; set; }

        public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();


        public string GetFullNameOrLogin()
        {
            var fullName = FirstName + " " + LastName;
            return string.IsNullOrEmpty(fullName) ? Login : fullName;
        }
    }

}