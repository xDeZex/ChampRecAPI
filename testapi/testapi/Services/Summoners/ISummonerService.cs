namespace testapi.Services.Summoners;

using testapi.Models;

public interface ISummonerService{
    Task CreateSummoner(Summoner summoner);
}