namespace MovinderAPI.Models
{
    public class Respond
    {
        public long responderId { get; set; }
        public long invitationId { get; set; }
        public int status { get; set; }

        //related

        public User responder {get; set; }

        public Invitaiton invitaiton {get; set; }
    }
}