using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MovinderAPI.Models;
using System.Linq;

namespace MovinderAPI.Controllers
{
    [Route("api/invitation")]
    public class InvitationController : Controller
    {
        private readonly InvitationContext _context;

        public InvitationController(InvitationContext context)
        {
            _context = context;

            if (_context.Invitaitons.Count() == 0)
            {
                _context.Invitaitons.Add(new Invitaiton { 
                    city = "Kyiv",
                    name = "Ruslan",
                    cinema = "Jovten",
                    movie = "some" 
                    });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Invitaiton> GetAll()
        {
            return _context.Invitaitons.ToList();
        }

        [HttpGet("{id}", Name = "getInvitation")]
        public IActionResult GetById(long id)
        {
            var item = _context.Invitaitons.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Invitaiton invitaiton)
        {
            if (invitaiton == null)
            {
                return BadRequest();
            }

            _context.Invitaitons.Add(invitaiton);
            _context.SaveChanges();

            return CreatedAtRoute("getInvitation", new { id = invitaiton.Id }, invitaiton);
        }
    }

}