using Microsoft.AspNetCore.Mvc;
using MovinderAPI.Models;
using MovinderAPI.Dtos;
using MovinderAPI.Helpers;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace MovinderAPI.Controllers
{
    [Route("api/responds")]
    public class RespondController : Controller
    {
        private readonly MovinderContext _context;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public RespondController(
            MovinderContext context, 
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        
        [HttpPost("reply")]
        public IActionResult Reply([FromBody] Respond respond)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == respond.respondentId);
            var invitation = _context.Invitaitons.SingleOrDefault(i => i.Id == respond.invitationId);

            if (user == null || invitation == null)
                return Ok(new {
                    success = false,
                    error = "User or invate not found."
                });

            respond.status = 1;

            _context.Responds.Add(respond);

            return Ok(new {
                success = true,
                respond = respond
            });
        }

        [HttpPost("confirm")]
        public IActionResult Confirm([FromBody] Respond confirm)
        {
            var respond = _context.Responds.SingleOrDefault( r => 
                r.respondentId == confirm.respondentId &&
                r.invitationId == confirm.invitationId
            );

            if(respond == null)
            return Ok(new {
                    success = false,
                    error = "Respond not found."
                });

            respond.status = 2;

            _context.Responds.Update(respond);

            return Ok(new {
                success = true,
                respond = respond
            });
        }

        [HttpPost("getUsersResponses")]
        public IActionResult UsersRepsonses([FromBody] UserDto userDto)
        {
            var invitations = _context.Invitaitons
                .Include(i => i.responders)
                .All(i => i.inviterId == userDto.id);
            
            var invitationsDto = _mapper.Map<InvitationDto>(invitations);

            return Ok(new {
                success = true,
                responds = invitationsDto
            });
        }
    }
}