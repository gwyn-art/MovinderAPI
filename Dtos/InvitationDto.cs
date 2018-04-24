using MovinderAPI.Models;
using System.Collections.Generic;

namespace MovinderAPI.Dtos
{
    public class InvitationDto
    {
        public int Id { get; set; }
        public string city { get; set; }
        public string movie { get; set; }
        public long inviterId { get; set; }
        public string cinema { get; set; }

        public UserDto inviter {get; set;}
        public List<Respond> responds {get; set; }
    }
}