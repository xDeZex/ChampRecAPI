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
        var ResponseSummoner = new Summoner();
        ResponseSummoner.summoner = summoner;
       

        Task<string[]> getSummoner = _summonerService.GetSummoner(ResponseSummoner);

        
        if(getSummoner.Exception is not null){
            return createProblem(getSummoner.Exception);
        }
        Console.WriteLine(getSummoner.Result);
        Console.WriteLine(getSummoner.Result.Length);
        var response = new SummonerRequest(getSummoner.Result[0],getSummoner.Result[1],getSummoner.Result[2]);
        return Ok(response);
    }
    
    [HttpPost("/post/{summoner}")]
    public IActionResult CreateSummoner(string summoner){

        var ResponseSummoner = new Summoner();
        ResponseSummoner.summoner = summoner;
        ResponseSummoner.Start = DateTime.UtcNow;

        Task createSummoner = _summonerService.CreateSummoner(ResponseSummoner);

        
        if(createSummoner.Exception is not null){
            return createProblem(createSummoner.Exception);
        }
        var response = new CreateSummonerRequest(ResponseSummoner.summoner, ResponseSummoner.Start);
        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
        return CreatedAtAction(nameof(CreateSummoner), response);
    }
    [Route("QWWQEWQEQWEQWEWQE")]
    public ObjectResult createProblem(AggregateException e){
        if (e.InnerException is HMException){
            HMException taskEx = (HMException)e.InnerException;
                
            return Problem(taskEx.Message, statusCode: taskEx.HTTPCode);
                
        }
        Console.WriteLine("Non-handled Error");
        Console.WriteLine(e.InnerException);
        return Problem();
    }
}