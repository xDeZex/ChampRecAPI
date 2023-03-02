using System;
namespace testapi.Models;

public class Summoner{

    public string Name {get;}
    public DateTime Start {get;}

    public Summoner(string Name){
        this.Name = Name;

        Start = DateTime.UtcNow;
    }
}