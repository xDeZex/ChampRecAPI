namespace testapi.Contracts.summoners;

public record SummonerResponse(
    string Name,
    DateTime Created,
    List<string> Champions
);