namespace lolapi.Services.Summoners;

using lolapi.Models;

public interface ISummonerService{
    Task CreateSummoner(Summoner summoner);
    string[] GetSummoner(Summoner summoner);
}