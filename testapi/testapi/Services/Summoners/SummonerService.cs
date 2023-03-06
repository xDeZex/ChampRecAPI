namespace testapi.Services.Summoners;
using testapi.Models;
using System.Data.OleDb;
using OfficeOpenXml;

public class SummonerService : ISummonerService{
    public async Task CreateSummoner(Summoner summoner){
        //C:\Users\ollib\Documents
        var file  = new FileInfo(@"..\Summoners.xlsx");

        try
        {
            
            await SaveExcelFile(summoner, file);
        }
        catch (System.Exception e)
        {
            Console.WriteLine("testFel");
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
            string name = summoner.Name;
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

