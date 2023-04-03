using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lolapi.Models;
using lolapi.Contracts.summoners;
using lolapi.Services.Summoners;
using System;
using lolapi.Exceptions;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Diagnostics;

namespace lolapi.Controllers;

[ApiController]
public class SummonersController : ControllerBase{

    private readonly ISummonerService _summonerService;

    public SummonersController(ISummonerService summonerService){
        _summonerService = summonerService;
    }

    [HttpGet("/get/{summoner}")]
    public IActionResult GetSummoner(string summoner){
        var ResponseSummoner = new Summoner();
        ResponseSummoner.summoner = summoner.ToLower();
       

        
        string[] getSummoner = _summonerService.GetSummoner(ResponseSummoner);

        var request = new SummonerRequest(getSummoner[0],getSummoner[1],getSummoner[2]);
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
    

    [Route("/error-development")]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment){
        Console.WriteLine("DEV ERROR");
        if (!hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        var e = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        if (e.Error is AggregateException){
            if (e.Error.InnerException is HMException){
                HMException taskEx = (HMException)e.Error.InnerException;

                return Problem(taskEx.Message, statusCode: taskEx.HTTPCode);
                    
            }
            
        }
        
        return Problem(
            detail: e.Error.StackTrace,
            title: e.Error.Message);
    }

    [Route("/error")]
    public IActionResult HandleError(){

        var e = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
        
        if (e.Error is AggregateException){
            if (e.Error.InnerException is HMException){
                HMException taskEx = (HMException)e.Error.InnerException;
        
                return Problem(taskEx.Message, statusCode: taskEx.HTTPCode);
                    
            }
            
        }

        return Problem();
    }
}