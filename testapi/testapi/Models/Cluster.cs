using System;
using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DocumentModel;

namespace testapi.Models;


public class Cluster{

    [Key]
    public int id {get; set;}
    public double[] mList {get;} = new double[155];

    public Cluster(Document doc){
        var keys = doc.Keys;
        int i = 0;
        foreach (var key in keys.OrderBy(x => x))
        {
            //Console.Write(key + " : "  + (double)doc[key] + ", ");
            if(key == "clusters")
                id = (int)doc[key];
            else{
                mList[i] = (double)doc[key];
                i++;
            }

                
        }
        //Console.WriteLine();
    }

}