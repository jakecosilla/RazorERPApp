using MediatR;
using Microsoft.AspNetCore.Mvc;
using RazorERP.Core.Application.Companies.Get;
using RazorERP.Core.Application.Users.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace RazorERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IValidator<UserRequest> _validator;

        public CompaniesController(ISender mediator, IValidator<UserRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<CompanyResponse>>> GetCompanies()
        {
            var query = new GetCompaniesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
