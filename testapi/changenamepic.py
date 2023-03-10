import os

l = os.listdir("../../angular/champrec/src/assets/champs")

for file in l:
    os.rename("../../angular/champrec/src/assets/champs/" + file, file.lower())