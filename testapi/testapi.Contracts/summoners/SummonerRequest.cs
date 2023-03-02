namespace testapi.Contracts.summoners;

public record SummonerRequest(
    string Name,
    DateTime Created,
    List<string> Champions
);