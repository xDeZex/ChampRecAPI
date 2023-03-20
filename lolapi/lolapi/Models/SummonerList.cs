using System;
using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

namespace testapi.Models;


public class SummonerList{

    [Key]
    public string summoner {get; set;} = "";
    public double[] mList {get;} = new double[155];

    public SummonerList(Document doc){

        var keys = doc.Keys;
        int i = 0;
        
        foreach (var key in keys.OrderBy(x => x))
        {
            if(key == "summoner")
                summoner = doc[key];
            else if(key != "lastupdated"){
                mList[i] = (double)doc[key];
                i++;
            }
                
        }
    }

}