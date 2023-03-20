

from openpyxl import Workbook
from openpyxl import load_workbook



wb = load_workbook(filename='..\Clusters.xlsx')
ws = wb['Clusters']

clusters = []

for row in ws.rows:
    clusters.append([cell.value for cell in row])

clusters = clusters[1:]

clist = ['Aatrox', 'Ahri', 'Akali', 'Alistar', 'Amumu', 'Anivia', 'Annie', 'Aphelios', 'Ashe', 'AurelionSol', 'Azir', 'Bard', 'Blitzcrank', 'Brand', 'Braum', 'Caitlyn', 'Camille', 'Cassiopeia', 'Chogath', 'Corki', 'Darius', 'Diana', 'Draven', 'DrMundo', 'Ekko', 'Elise', 'Evelynn', 'Ezreal', 'Fiddlesticks', 'Fiora', 'Fizz', 'Galio', 'Gangplank', 'Garen', 'Gnar', 'Gragas', 'Graves', 'Gwen', 'Hecarim', 'Heimerdinger', 'Illaoi', 'Irelia', 'Ivern', 'Janna', 'JarvanIV', 'Jax', 'Jayce', 'Jhin', 'Jinx', 'Kaisa', 'Kalista', 'Karma', 'Karthus', 'Kassadin', 'Katarina', 'Kayle', 'Kayn', 'Kennen', 'Khazix', 'Kindred', 'Kled', 'KogMaw', 'Leblanc', 'LeeSin', 'Leona', 'Lillia', 'Lissandra', 'Lucian', 'Lulu', 'Lux', 'Malphite', 'Malzahar', 'Maokai', 'MasterYi', 'MissFortune', 'MonkeyKing', 'Mordekaiser', 'Morgana', 'Nami', 'Nasus', 'Nautilus', 'Neeko', 'Nidalee', 'Nocturne', 'Nunu', 'Olaf', 'Orianna', 'Ornn', 'Pantheon', 'Poppy', 'Pyke', 'Qiyana', 'Quinn', 'Rakan', 'Rammus', 'RekSai', 'Rell', 'Renekton', 'Rengar', 'Riven', 'Rumble', 'Ryze', 'Samira', 'Sejuani', 'Senna', 'Seraphine', 'Sett', 'Shaco', 'Shen', 'Shyvana', 'Singed', 'Sion', 'Sivir', 'Skarner', 'Sona', 'Soraka', 'Swain', 'Sylas', 'Syndra', 'TahmKench', 'Taliyah', 'Talon', 'Taric', 'Teemo', 'Thresh', 'Tristana', 'Trundle', 'Tryndamere', 'TwistedFate', 'Twitch', 'Udyr', 'Urgot', 'Varus', 'Vayne', 'Veigar', 'Velkoz', 'Vi', 'Viego', 'Viktor', 'Vladimir', 'Volibear', 'Warwick', 'Xayah', 'Xerath', 'XinZhao', 'Yasuo', 'Yone', 'Yorick', 'Yuumi', 'Zac', 'Zed', 'Ziggs', 'Zilean', 'Zoe', 'Zyra']

cString = ""
for c in clist:
    cString += f"{c},"
cString = cString[:-1]

print(len(clist))
print(len(clusters[0]))

print(len(clusters))


import json

i = 0
for file in range(42):
    print("file", file)
    jsonString = """{ "clusters": ["""
    while not (i >= (file + 1) * 25 or i >= len(clusters)):
        cluster = clusters[i]
        print(i)
        jsonString += """ 
        {"PutRequest": { 
            "Item": {
                """
        for j, c in enumerate(cluster):
            if j == 0:
                jsonString += """ "clusters": {"S": """
                jsonString += f"\"{cluster[j]}\""
                jsonString += """}"""
            else:
                jsonString += f"\"{clist[j-1].lower()}\": "
                jsonString += """ {"N": """
                jsonString += f"\"{cluster[j]}\""
                jsonString += """}"""
            if j != 155:
                jsonString += """,
                """
        jsonString += """
        }}},"""
        i += 1


    jsonString = jsonString[:-1]
    jsonString += """]}"""


    with open(f"{file}.json", "w") as outfile:
        outfile.write(json.dumps(json.loads(jsonString), indent=4))