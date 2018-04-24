using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MovinderAPI.Models;
using System.Linq;
using MovinderAPI.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MovinderAPI.Controllers
{
    [Route("api/invitations")]
    public class InvitationController : Controller
    {
        private readonly MovinderContext _context;
        private IMapper _mapper;

        public InvitationController(
            MovinderContext context,
            IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }

        [HttpPost]
        public IActionResult GetAll()
        {
            var invitations = _context.Invitaitons
                .Include(i => i.inviter)
                .ToList();
                
            var invitationsDto = _mapper.Map<IList<InvitationDto>>(invitations);
            

            return Ok(new {
                success = true,
                invitations = invitationsDto
            });
        }

        [HttpPost("get")]
        public IActionResult GetById([FromBody]InvitationDto invitationDto)
        {
            var invitation = _context.Invitaitons
                .Include(i => i.inviter)
                .FirstOrDefault(t => t.Id == invitationDto.Id);
            
            if (invitation == null)
            {
                return Ok(new {
                    success = false,
                    error = "Invintation not found"
                });
            }

            invitationDto = _mapper.Map<InvitationDto>(invitation);

            return Ok(new {
                success = true,
                invite = invitationDto
            });
        }

        [HttpPost("new")]
        public IActionResult Create([FromBody] Invitaiton invitation)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == invitation.inviterId);

            if(user == null)
            {
                return Ok(new {
                    success = false,
                    error = $"Can't find user with id = {invitation.inviterId}"
                });
            }

            _context.Invitaitons.Add(invitation);
            _context.SaveChanges();

            InvitationDto invitationDto = _mapper.Map<InvitationDto>(invitation);

            return Ok(new {
                success = true,
                invitation = invitationDto
            });
        }
    }

}