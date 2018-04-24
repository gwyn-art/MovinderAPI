using MovinderAPI.Models;
using System.Collections.Generic;

namespace MovinderAPI.Dtos
{
    public class InvitationDto
    {
        public long Id { get; set; }
        public string city { get; set; }
        public string cinema { get; set; }
        public string movie { get; set; }
        public string time {get; set;}
        public long inviterId { get; set; }

        public UserDto inviter {get; set;}
        public List<RespondDto> responders {get; set; }
    }
}