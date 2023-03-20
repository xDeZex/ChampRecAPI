namespace lolapi.Services.Summoners;

using lolapi.Models;

public interface ISummonerService{
    Task CreateSummoner(Summoner summoner);
    Task<string[]> GetSummoner(Summoner summoner);
}