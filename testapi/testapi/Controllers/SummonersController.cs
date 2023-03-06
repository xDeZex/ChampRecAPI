using Microsoft.AspNetCore.Mvc;
using testapi.Models;
using testapi.Contracts.summoners;
using testapi.Services.Summoners;
using System;

namespace testapi.Controllers;

[ApiController]
public class SummonersController : ControllerBase{

    private readonly ISummonerService _summonerService;

    public SummonersController(ISummonerService summonerService){
        _summonerService = summonerService;
    }

    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();

    [HttpGet("/get/{summoner}")]
    public IActionResult GetSummoner(string summoner){
        return Ok(summoner);
    }
    
    [HttpPost("/post/{summoner}")]
    public IActionResult CreateSummoner(string summoner){

        var ResponseSummoner = new Summoner(summoner);

        // TODO database

        Task createSummoner = _summonerService.CreateSummoner(ResponseSummoner);

        if(createSummoner.Exception is not null)
            return Problem(createSummoner.Exception.InnerException.Message);
        List<string> temp = new List<string>() {"Test1", "Test2"};
        var response = new SummonerResponse(ResponseSummoner.Name, ResponseSummoner.Start, temp);

        return CreatedAtAction(nameof(GetSummoner), new {summoner = ResponseSummoner.Name}, response);
    }
}