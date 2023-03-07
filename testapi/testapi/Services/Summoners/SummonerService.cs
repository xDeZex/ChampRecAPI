using System.Data.Common;
namespace testapi.Services.Summoners;
using testapi.Models;
using System.Data.Odbc;
using OfficeOpenXml;
using Microsoft.Data.SqlClient;
using testapi.Data;
using testapi.Exceptions;

public class SummonerService : ISummonerService{

    public async Task GetSummoner(Summoner summoner){
        List<Cluster> clusters = GetClusters();
    }

    static async Task<List<Cluster>> GetClusters(){

        List<Cluster> returnList = new();
        string queryString = $"""SELECT * FROM "Summoners"."Cluster" """;
        Console.WriteLine(queryString);
        OdbcCommand command = new OdbcCommand(queryString);
        using (OdbcConnection conn = new OdbcConnection("DSN=PostgreSQL35W")){
            conn.Open();
            command.Connection = conn;
            DbDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read()){
                Console.WriteLine(reader.GetType());
                Console.WriteLine(reader);
                break;
                //returnList.Add(new Cluster())
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

            string queryString = $"""INSERT INTO "Summoners"."Summoner" (summoner) VALUES ('{summoner.summoner}');""" ;
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
                
                HMException newE = new HMException("ERROR: That summoner is already in the database.");
                newE.HTTPCode = 409;
                throw newE;

            }
            throw e;

        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
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

