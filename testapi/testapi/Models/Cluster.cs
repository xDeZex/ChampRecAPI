using System;
using System.ComponentModel.DataAnnotations;

namespace testapi.Models;


public class Cluster{

    [Key]
    public int id {get; set;}
    public List<int> mList {get; init;} = new List<int>();

}