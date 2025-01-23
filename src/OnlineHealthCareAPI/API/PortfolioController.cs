﻿using API;
using Application.Dto.Portfolio;
using Application.Features.Portfolio.Command;
using Application.Features.Portfolio.Query;
using Microsoft.AspNetCore.Mvc;

namespace StudentPortfolio_Management_System.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IQueryable<PortfolioDTO>>> GetAll()
        {
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

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePortfolio(int id)
        {
            var result = await Mediator.Send(new DeletePortfolioCommand { Id = id });
            return result ? Ok(result) : NotFound();
        }
    }
}
