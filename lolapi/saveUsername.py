from riotwatcher import LolWatcher, ApiError
import pyodbc
import json
import sys, os


def main(name):

    api_key = ''
    watcher = LolWatcher(api_key)
    my_region = 'euw1'


    conn_str = (
        r"DSN=PostgreSQL35W"
    )
    conn = pyodbc.connect(conn_str)
    cursor = conn.cursor()

    clist = ['Aatrox', 'Ahri', 'Akali', 'Alistar', 'Amumu', 'Anivia', 'Annie', 'Aphelios', 'Ashe', 'AurelionSol', 'Azir', 'Bard', 'Blitzcrank', 'Brand', 'Braum', 'Caitlyn', 'Camille', 'Cassiopeia', 'Chogath', 'Corki', 'Darius', 'Diana', 'Draven', 'DrMundo', 'Ekko', 'Elise', 'Evelynn', 'Ezreal', 'Fiddlesticks', 'Fiora', 'Fizz', 'Galio', 'Gangplank', 'Garen', 'Gnar', 'Gragas', 'Graves', 'Gwen', 'Hecarim', 'Heimerdinger', 'Illaoi', 'Irelia', 'Ivern', 'Janna', 'JarvanIV', 'Jax', 'Jayce', 'Jhin', 'Jinx', 'Kaisa', 'Kalista', 'Karma', 'Karthus', 'Kassadin', 'Katarina', 'Kayle', 'Kayn', 'Kennen', 'Khazix', 'Kindred', 'Kled', 'KogMaw', 'Leblanc', 'LeeSin', 'Leona', 'Lillia', 'Lissandra', 'Lucian', 'Lulu', 'Lux', 'Malphite', 'Malzahar', 'Maokai', 'MasterYi', 'MissFortune', 'MonkeyKing', 'Mordekaiser', 'Morgana', 'Nami', 'Nasus', 'Nautilus', 'Neeko', 'Nidalee', 'Nocturne', 'Nunu', 'Olaf', 'Orianna', 'Ornn', 'Pantheon', 'Poppy', 'Pyke', 'Qiyana', 'Quinn', 'Rakan', 'Rammus', 'RekSai', 'Rell', 'Renekton', 'Rengar', 'Riven', 'Rumble', 'Ryze', 'Samira', 'Sejuani', 'Senna', 'Seraphine', 'Sett', 'Shaco', 'Shen', 'Shyvana', 'Singed', 'Sion', 'Sivir', 'Skarner', 'Sona', 'Soraka', 'Swain', 'Sylas', 'Syndra', 'TahmKench', 'Taliyah', 'Talon', 'Taric', 'Teemo', 'Thresh', 'Tristana', 'Trundle', 'Tryndamere', 'TwistedFate', 'Twitch', 'Udyr', 'Urgot', 'Varus', 'Vayne', 'Veigar', 'Velkoz', 'Vi', 'Viego', 'Viktor', 'Vladimir', 'Volibear', 'Warwick', 'Xayah', 'Xerath', 'XinZhao', 'Yasuo', 'Yone', 'Yorick', 'Yuumi', 'Zac', 'Zed', 'Ziggs', 'Zilean', 'Zoe', 'Zyra']
    print(name)    
    latest = watcher.data_dragon.versions_for_region(my_region)['n']['champion']
    static_champ_list = watcher.data_dragon.champions(latest, False, 'en_US')

    #columns = [column[0] for column in cursor.description][1:]

    champ_dict = {}
    for key in static_champ_list['data']:
        row = static_champ_list['data'][key]
        champ_dict[row['key']] = row['id']

    me = watcher.summoner.by_name(my_region, name)
    #print(me)
    champMastery = watcher.champion_mastery.by_summoner(my_region, me['id'])
    #print(champMastery)
    listofc = []
    for champion in champMastery:
        champID = champion['championId']
        champM = champion['championPoints']
        champName = champ_dict[str(champID)]
        if champName in clist:
            listofc.append((champName, champM))
            #print(champ_dict[str(champID)], champM)

    #print(listofc[:10])

    sqlInsert = f"""INSERT INTO public."Summoner" (summoner,""" 
    for (x, _) in listofc:
        sqlInsert += f"""{x},"""
    sqlInsert = sqlInsert[:-1]
    sqlInsert += f""")
    VALUES (
        '{name}', """
    for (_, y) in listofc:
        sqlInsert += f"""{y}, """
    sqlInsert = sqlInsert[:-2] + ");"
    print(sqlInsert)
    cursor.execute(sqlInsert)
    conn.commit()
if __name__ == '__main__':
    import sys
    main(sys.argv[1])
