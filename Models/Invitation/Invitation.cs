using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovinderAPI.Models;
using System.Collections.Generic;

namespace MovinderAPI.Models
{
    public class Invitation
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string cinema { get; set; }
        [Required]
        public string movie { get; set; }
        [Required]
        public long inviterId { get; set; }

        //related
        public User inviter { get; set; }
        public List<Respond> responders { get; set; }
    }
}