using API;
using Application.Dto.Portfolio;
using Application.Dto.Student;
using Application.Features.Portfolio.Command;
using Application.Features.Student.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StudentPortfolio_Management_System.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> AddPortfolio( CreatePortfolioDTO portfolioDto)
        {

            // If validation passes, proceed with adding the student
            var portfolioId = await Mediator.Send(new CreatePortfolioCommand { Portfolio = portfolioDto });
            return Ok(portfolioId);
        }
    }
}
