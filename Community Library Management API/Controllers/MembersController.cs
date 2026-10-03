using Community_Library_Management_API.DTO.Members;
using Community_Library_Management_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Community_Library_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _service;

        public MembersController(IMemberService service)
        {
            _service = service;
        }

        // GET: api/members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET: api/members/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetById(int id)
        {
            var member = await _service.GetByIdAsync(id);

            if (member == null)
                return NotFound("Member not found.");

            return Ok(member);
        }

        // POST: api/members
        [HttpPost]
        public async Task<ActionResult<MemberDto>> Create(
            CreateMemberDto dto)
        {
            try
            {
                var member = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = member.Id },
                    member);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/members/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MemberDto>> Update(
            int id,
            CreateMemberDto dto)
        {
            try
            {
                var member = await _service.UpdateAsync(id, dto);

                if (member == null)
                    return NotFound("Member not found.");

                return Ok(member);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/members/1
        // This deactivates the member.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound("Member not found.");

            return NoContent();
        }
    }
}