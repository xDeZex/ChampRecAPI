using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lolapi.Models;
using lolapi.Contracts.summoners;
using lolapi.Services.Summoners;
using System;
using lolapi.Exceptions;
using Amazon.DynamoDBv2.Model;

namespace lolapi.Controllers;

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
        ResponseSummoner.summoner = summoner.ToLower();
       

        Task<string[]> getSummoner = _summonerService.GetSummoner(ResponseSummoner);

        try{
            getSummoner.Wait();
        }
        catch{}

        if(getSummoner.Exception is not null){
            return createProblem(getSummoner.Exception);
        }
        var request = new SummonerRequest(getSummoner.Result[0],getSummoner.Result[1],getSummoner.Result[2]);
        var response = new WrapperGetRequest(request);
        return Ok(response);
    }
    
    [HttpPost("/post/{summoner}")]
    public IActionResult CreateSummoner(string summoner){

        var ResponseSummoner = new Summoner();
        ResponseSummoner.summoner = summoner.ToLower();
        ResponseSummoner.Start = DateTime.UtcNow;

        Task createSummoner = _summonerService.CreateSummoner(ResponseSummoner);

        try{
            createSummoner.Wait();
        }
        catch{}
        
        if(createSummoner.Exception is not null){
            return createProblem(createSummoner.Exception);
        }
        var response = new CreateSummonerRequest(ResponseSummoner.summoner, ResponseSummoner.Start);
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