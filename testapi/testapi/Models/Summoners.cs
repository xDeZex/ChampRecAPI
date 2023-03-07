using System;
using System.ComponentModel.DataAnnotations;

namespace testapi.Models;


public class Summoner{

    [Key]
    public string summoner {get; set;} = "";
    public DateTime Start {get; set;}

}