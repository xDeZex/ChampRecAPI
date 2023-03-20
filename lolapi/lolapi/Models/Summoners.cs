using System;
using System.ComponentModel.DataAnnotations;

namespace lolapi.Models;


public class Summoner{

    [Key]
    public string summoner {get; set;} = "";
    public DateTime Start {get; set;}

}