using System;
using System.ComponentModel.DataAnnotations;

namespace testapi.Models;


public class SummonerList{

    [Key]
    public string name {get; set;}
    public double[] mList {get;} = new double[155];

    public SummonerList(object[] meta){
        name = Convert.ToString(meta[0]);
        for (int i = 1; i < meta.Count(); i++)
        {
            mList[i-1] = (Convert.ToDouble(meta[i]));
        }
    }

}