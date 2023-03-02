namespace testapi.Contracts.summoners;

public record CreateSummonerRequest(
    string Name,
    DateTime Created
);