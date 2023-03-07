using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testapi.Models;
using testapi.Contracts.summoners;
using testapi.Services.Summoners;
using System;
using testapi.Exceptions;

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

        var ResponseSummoner = new Summoner();
        ResponseSummoner.summoner = summoner;
        ResponseSummoner.Start = DateTime.UtcNow;

        Task createSummoner = _summonerService.CreateSummoner(ResponseSummoner);

        
        if(createSummoner.Exception is not null){
            if (createSummoner.Exception.InnerException is HMException){
                HMException taskEx = (HMException)createSummoner.Exception.InnerException;
                
                return Problem(taskEx.Message, statusCode: taskEx.HTTPCode);
            }
            Console.WriteLine("Non-handled Error");
            return Problem(createSummoner.Exception.Message);

        }
        var response = new CreateSummonerRequest(ResponseSummoner.summoner, ResponseSummoner.Start);
        Console.WriteLine("Almost Created and Done");

        return CreatedAtAction(nameof(CreateSummoner), response);
    }
}