using System.Data.Common;
namespace testapi.Services.Summoners;
using testapi.Models;
using System.Data.Odbc;
using OfficeOpenXml;
using testapi.Exceptions;

public class SummonerService : ISummonerService{

    public async Task<string[]> GetSummoner(Summoner summoner){
        List<Cluster> clusters = await GetClusters();
        SummonerList summonerList = await GetSummoner(summoner.summoner);
        string[] recommendation = new Recommendation(summonerList.mList, clusters).recommendation;
        return recommendation;
    }

    static async Task<SummonerList> GetSummoner(string name){
        string queryString = $"""SELECT * FROM "Summoners"."Summoner" WHERE summoner = '{name.ToLower()}'""";
        OdbcCommand command = new OdbcCommand(queryString);
        using (OdbcConnection conn = new OdbcConnection("DSN=PostgreSQL35W")){
            conn.Open();
            command.Connection = conn;
            DbDataReader reader = await command.ExecuteReaderAsync();
            object[] meta = new object[156];
            if(!reader.Read()){
                HMException e = new HMException("ERROR: Found no added summoner with that name.");
                e.HTTPCode = 404;
                throw e;
            }
            int i = reader.GetValues(meta);
            reader.Close();
            return new SummonerList(meta);
        }
    }
    static async Task<List<Cluster>> GetClusters(){

        List<Cluster> returnList = new();
        string queryString = $"""SELECT * FROM "Summoners"."Cluster" """;
        OdbcCommand command = new OdbcCommand(queryString);
        using (OdbcConnection conn = new OdbcConnection("DSN=PostgreSQL35W")){
            conn.Open();
            command.Connection = conn;
            DbDataReader reader = await command.ExecuteReaderAsync();
            object[] meta = new object[156];
            while (reader.Read()){
                int i = reader.GetValues(meta);
                returnList.Add(new Cluster(meta));
            }
        }
        return returnList;
    }

    public async Task CreateSummoner(Summoner summoner){
        
        var file  = new FileInfo(@"..\Summoners.xlsx");

        try
        {
            await SaveSummonerDB(summoner);
        }
        catch (HMException e){
            Console.WriteLine("NEw E");
            throw e;
        }
        catch (System.Exception e)
        {
            Console.WriteLine("testFel");

            throw e;
        }
    }
    static async Task SaveSummonerDB(Summoner summoner){
        try
        {

            string queryString = $"""INSERT INTO "Summoners"."Summoner" (summoner, lastupdated) VALUES ('{summoner.summoner.ToLower()}', '{DateTime.MinValue}');""" ;
            Console.WriteLine(queryString);
            OdbcCommand command = new OdbcCommand(queryString);
            using (OdbcConnection conn = new OdbcConnection("DSN=PostgreSQL35W")){
                conn.Open();
                command.Connection = conn;
                await command.ExecuteNonQueryAsync();
            }
        }
        catch (OdbcException e){
            
            if (e.HResult == -2146232009){
                Console.WriteLine(e);
                HMException newE = new HMException("ERROR: That summoner is already in the database.");
                newE.HTTPCode = 409;
                throw newE;

            }
            throw e;

        }
        catch (System.Exception e)
        {
            throw e;
        }
        
    }
    static async Task SaveExcelFile(Summoner summoner, FileInfo file){
        try
        {
            using var package = new ExcelPackage(file);
            Console.WriteLine("Test");
            Console.WriteLine(package.Workbook.Worksheets);
            var ws = package.Workbook.Worksheets[0];
            
            Console.WriteLine("TEst1");
            var temp = ws.Dimension;
            Console.WriteLine($"C: {temp.End.Column} R: {temp.End.Row}");
            Console.WriteLine("TEst12");
            string name = summoner.summoner;
            bool alreadyExists = false;
            Console.WriteLine("TEst2");
        
            if(ws.Dimension is null){
            ws.Cells[1, 1].Value = name;
            Console.WriteLine("Empty", name);
            await package.SaveAsync();
            return;
            }
            for (int i = 1; i <= ws.Dimension.End.Row; i++){
                Console.WriteLine(i);
                var cell = ws.Cells[i, 1].Value.ToString();
                if (cell is string)
                    if (cell.Trim() == name){
                        alreadyExists = true;
                    }
                if (alreadyExists)
                    break; 
            } 
            if(!alreadyExists){
                ws.Cells[ws.Dimension.End.Row + 1, 1].Value = name;
                Console.WriteLine("NonEmpty", name);
                await package.SaveAsync();
                return;
            }
            Console.WriteLine("Not New");
            throw new Exception("Not New");
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
        
    }
}

