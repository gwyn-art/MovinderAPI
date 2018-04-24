namespace MovinderAPI.Dtos
{
    public class RespondDto
    {
        public long respondentId { get; set; }
        public long invitationId { get; set; }
        public int status { get; set; }

        //related

        public UserDto responder {get; set; }

        public InvitationDto invitaiton {get; set; }
    }
}