using Community_Library_Management_API.DTO.Loans;
using Community_Library_Management_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Community_Library_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;

        public LoansController(ILoanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LoanDto>> GetById(int id)
        {
            var loan = await _service.GetByIdAsync(id);

            if (loan == null)
                return NotFound("Loan not found.");

            return Ok(loan);
        }

        [HttpPost]
        public async Task<ActionResult<LoanDto>> Create(
            CreateLoanDto dto)
        {
            try
            {
                var loan = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = loan.Id },
                    loan);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:int}/return")]
        public async Task<ActionResult<LoanDto>> Return(
            int id,
            ReturnLoanDto? dto)
        {
            try
            {
                var loan = await _service.ReturnAsync(id);

                if (loan == null)
                    return NotFound("Loan not found.");

                return Ok(loan);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("/api/members/{memberId:int}/loans")]
        public async Task<ActionResult<IEnumerable<LoanDto>>>
            GetMemberLoans(int memberId)
        {
            try
            {
                var loans =
                    await _service.GetByMemberIdAsync(memberId);

                return Ok(loans);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}