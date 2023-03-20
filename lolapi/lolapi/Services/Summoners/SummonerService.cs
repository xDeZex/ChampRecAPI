using System.Data.Common;
namespace testapi.Services.Summoners;
using testapi.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using OfficeOpenXml;
using testapi.Exceptions;
using System.Data.Odbc;

public class SummonerService : ISummonerService{

    List<string> clist = new List<string>{"Aatrox", "Ahri", "Akali", "Alistar", "Amumu", "Anivia", "Annie", "Aphelios", "Ashe", "AurelionSol", "Azir", "Bard", "Blitzcrank", "Brand", "Braum", "Caitlyn", "Camille", "Cassiopeia", "Chogath", "Corki", "Darius", "Diana", "Draven", "DrMundo", "Ekko", "Elise", "Evelynn", "Ezreal", "Fiddlesticks", "Fiora", "Fizz", "Galio", "Gangplank", "Garen", "Gnar", "Gragas", "Graves", "Gwen", "Hecarim", "Heimerdinger", "Illaoi", "Irelia", "Ivern", "Janna", "JarvanIV", "Jax", "Jayce", "Jhin", "Jinx", "Kaisa", "Kalista", "Karma", "Karthus", "Kassadin", "Katarina", "Kayle", "Kayn", "Kennen", "Khazix", "Kindred", "Kled", "KogMaw", "Leblanc", "LeeSin", "Leona", "Lillia", "Lissandra", "Lucian", "Lulu", "Lux", "Malphite", "Malzahar", "Maokai", "MasterYi", "MissFortune", "MonkeyKing", "Mordekaiser", "Morgana", "Nami", "Nasus", "Nautilus", "Neeko", "Nidalee", "Nocturne", "Nunu", "Olaf", "Orianna", "Ornn", "Pantheon", "Poppy", "Pyke", "Qiyana", "Quinn", "Rakan", "Rammus", "RekSai", "Rell", "Renekton", "Rengar", "Riven", "Rumble", "Ryze", "Samira", "Sejuani", "Senna", "Seraphine", "Sett", "Shaco", "Shen", "Shyvana", "Singed", "Sion", "Sivir", "Skarner", "Sona", "Soraka", "Swain", "Sylas", "Syndra", "TahmKench", "Taliyah", "Talon", "Taric", "Teemo", "Thresh", "Tristana", "Trundle", "Tryndamere", "TwistedFate", "Twitch", "Udyr", "Urgot", "Varus", "Vayne", "Veigar", "Velkoz", "Vi", "Viego", "Viktor", "Vladimir", "Volibear", "Warwick", "Xayah", "Xerath", "XinZhao", "Yasuo", "Yone", "Yorick", "Yuumi", "Zac", "Zed", "Ziggs", "Zilean", "Zoe", "Zyra"};

    public async Task<string[]> GetSummoner(Summoner summoner){
        try{ 
            SummonerList summonerList = await GetSummoner("summoners", summoner.summoner);
            List<Cluster> clusters = await GetClusters();
            string[] recommendation = new Recommendation(summonerList.mList, clusters).recommendation;
            return recommendation;
        }
        catch (Exception e){
            throw e;
        }

        
    }

    private static async Task<List<Dictionary<string, AttributeValue>>> ScanDBAsync(string table)
    {
        using var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.EUNorth1);

        var response = await client.ScanAsync(new ScanRequest(table));

        var items = response.Items;


        if(response.LastEvaluatedKey != null){
            var request = new ScanRequest(table);
            
            request.ExclusiveStartKey = response.LastEvaluatedKey;

            response = await client.ScanAsync(request);

            items.AddRange(response.Items);
        }

        return items;
    }

    private static async Task<Document> GetDBAsync(string tableName, string key)
    {
        using var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.EUNorth1);

        Table table = Table.LoadTable(client, tableName);
        

        Document document = await table.GetItemAsync(key);
        return document;
    }

    private async Task<Document> PutSummonerDBAsync(string tableName, string key)
    {
        using var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.EUNorth1);

        Table table = Table.LoadTable(client, tableName);

        var summoner = new Document();
        summoner["summoner"] = key.ToLower();

        foreach (string c in this.clist)
        {
            summoner[c.ToLower()] = 0;
        }
        summoner["lastupdated"] = DateTime.UtcNow;


        Expression expr = new Expression();
        expr.ExpressionStatement = "attribute_not_exists(summoner)";

        UpdateItemOperationConfig config = new UpdateItemOperationConfig()
        {
            ConditionalExpression = expr
        };
        Document document = await table.UpdateItemAsync(summoner, config);
        return document;
    }
    static async Task<SummonerList> GetSummoner(string tableName, string name){
        using var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.EUNorth1);

        Table table = Table.LoadTable(client, tableName);

        Document summoner = await table.GetItemAsync(name);

        if(summoner is null){
            var e = new HMException("Didn't find that summoner in the database.");
            e.HTTPCode = 404;
            throw e;
        }

        return new SummonerList(summoner);
    }
    static async Task<List<Cluster>> GetClusters(){

        List<Cluster> returnList = new();

        var ddbList = await ScanDBAsync("clusters");

        foreach (var cluster in ddbList)
        {
            returnList.Add(new Cluster(Document.FromAttributeMap(cluster)));
        }
        return returnList;
    }

    public async Task CreateSummoner(Summoner summoner){
        
        var file  = new FileInfo(@"..\Summoners.xlsx");

        try
        {
            await PutSummonerDBAsync("summoners", summoner.summoner);
        }
        catch (HMException e){
            throw e;
        }
        catch (ConditionalCheckFailedException e){
            HMException exc = new HMException("That summoner is already in the database.");
            exc.HTTPCode = 409;
            throw exc;
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }
}

