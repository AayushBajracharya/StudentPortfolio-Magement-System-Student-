using System.Drawing.Printing;
using API;
using Application.Dto.Portfolio;
using Application.Features.Portfolio.Command;
using Application.Features.Portfolio.Query;
using Application.Features.Student.Query;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace StudentPortfolio_Management_System.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IQueryable<PortfolioDTO>>> GetAll()
        {
            //var portfolios = await _portfolioService.GetAllPortfolioAsync();
            //return Ok(portfolios);
            var portfolios = await Mediator.Send(new GetAllPortfolioQuery());
            return Ok(portfolios);
        }

        [HttpPost]
        public async Task<IActionResult> AddPortfolio( CreatePortfolioDTO portfolioDto)
        {

            // If validation passes, proceed with adding the student
            var portfolioId = await Mediator.Send(new CreatePortfolioCommand { Portfolio = portfolioDto });
            return Ok(portfolioId);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePortfolio(int id, [FromForm] UpdatePortfolioCommand portfolioDto)
        {
            var result = await Mediator.Send(portfolioDto);
            return result ? Ok(result) : NotFound();
        }
    }
}
