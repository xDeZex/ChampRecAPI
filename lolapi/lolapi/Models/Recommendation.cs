using System;
using System.ComponentModel.DataAnnotations;
using MathNet.Numerics.LinearAlgebra;

namespace lolapi.Models;


public class Recommendation{

    public string[] recommendation {get;} = new string[3];


    public Recommendation(double[] summonerList, List<Cluster> clusters){

        var V = Vector<double>.Build;
        Vector<double> summonerV = V.DenseOfArray(summonerList);
        List<Vector<double>> clustersV = new();
        foreach (Cluster cluster in clusters)
        {
            clustersV.Add(V.DenseOfArray(cluster.mList));
        }
        double minDist = double.MaxValue;
        Vector<double> minDistV = clustersV[0];
        foreach (Vector<double> cV in clustersV)
        {
            double dist = (summonerV - cV).L2Norm();
            if (dist < minDist){
                minDist = dist;
                minDistV = cV;
            }

        }
        string[] championNames = {"Aatrox", "Ahri", "Akali", "Alistar", "Amumu", "Anivia", "Annie", "Aphelios", "Ashe", "AurelionSol", "Azir", "Bard", "Blitzcrank", "Brand", "Braum", "Caitlyn", "Camille", "Cassiopeia", "Chogath", "Corki", "Darius", "Diana", "Draven", "DrMundo", "Ekko", "Elise", "Evelynn", "Ezreal", "Fiddlesticks", "Fiora", "Fizz", "Galio", "Gangplank", "Garen", "Gnar", "Gragas", "Graves", "Gwen", "Hecarim", "Heimerdinger", "Illaoi", "Irelia", "Ivern", "Janna", "JarvanIV", "Jax", "Jayce", "Jhin", "Jinx", "Kaisa", "Kalista", "Karma", "Karthus", "Kassadin", "Katarina", "Kayle", "Kayn", "Kennen", "Khazix", "Kindred", "Kled", "KogMaw", "Leblanc", "LeeSin", "Leona", "Lillia", "Lissandra", "Lucian", "Lulu", "Lux", "Malphite", "Malzahar", "Maokai", "MasterYi", "MissFortune", "MonkeyKing", "Mordekaiser", "Morgana", "Nami", "Nasus", "Nautilus", "Neeko", "Nidalee", "Nocturne", "Nunu", "Olaf", "Orianna", "Ornn", "Pantheon", "Poppy", "Pyke", "Qiyana", "Quinn", "Rakan", "Rammus", "RekSai", "Rell", "Renekton", "Rengar", "Riven", "Rumble", "Ryze", "Samira", "Sejuani", "Senna", "Seraphine", "Sett", "Shaco", "Shen", "Shyvana", "Singed", "Sion", "Sivir", "Skarner", "Sona", "Soraka", "Swain", "Sylas", "Syndra", "TahmKench", "Taliyah", "Talon", "Taric", "Teemo", "Thresh", "Tristana", "Trundle", "Tryndamere", "TwistedFate", "Twitch", "Udyr", "Urgot", "Varus", "Vayne", "Veigar", "Velkoz", "Vi", "Viego", "Viktor", "Vladimir", "Volibear", "Warwick", "Xayah", "Xerath", "XinZhao", "Yasuo", "Yone", "Yorick", "Yuumi", "Zac", "Zed", "Ziggs", "Zilean", "Zoe", "Zyra"};

        List<Tuple<String, double, double>> toBeSorted = new();
        for (int i = 0; i < 155; i++)
        {
            toBeSorted.Add(new Tuple<string, double, double>(championNames[i], summonerV[i]- minDistV[i], summonerV[i]));
        }
        double sumM = summonerV.Sum();
        var sortedM = toBeSorted.OrderBy(t => t.Item3).ToList();
        sortedM.Reverse();

        var top10 = sortedM.GetRange(0, 10);
        var toBeFiltered = sortedM.GetRange(0, 10);
        double sum10 = 0;
        foreach (var champ in top10)
        {
            sum10 += champ.Item3;
        }
        if(sum10 > sumM / 4){
            int i;
            for (i = 9; i > 1; i--)
            {   
                sum10 -= top10[i].Item3;
                if(sum10 < sumM / 4)
                    break;
            }
            top10.RemoveRange(i, 10 - i);
        }

        var sortedD = toBeSorted.OrderBy(t => t.Item2).ToList();
        //inte rek top 10 eller 25% 
        int j = 0;
        for (int i = 0; i < 155; i++)
        {
            if(!top10.Contains(sortedD[i])){
                recommendation[j] = sortedD[i].Item1.ToLower();
                j++;
                if (recommendation[2] is not null)
                    break;
            }
        }
    }
    

}