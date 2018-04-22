using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovinderAPI.Models;
using System.Collections.Generic;

namespace MovinderAPI.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string city { get; set; }
        [StringLength(15, MinimumLength = 3)]
        [Required]
        public string name { get; set; }
        [Required]
        public string email {get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }

        // related
        public List<Invitaiton> InvaterPosts { get; set; }
        public List<Respond> Responds { get; set; }
    }
}