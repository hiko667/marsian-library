
# Instrukcja stworzenia bazy danych

Żebyśmy wszyscy mieli to samo



## Instalacja

Upewnijcie się, że macie dockera. Następnie, będąc w terminalu w folderze w którym jest projekt instalujecie pakiety dotneta, jak ich nie macie:

```bash
  dotnet add package Oracle.ManagedDataAccess.core --version 9.0.2
  dotnet add package Oracle.EntityFrameworkCore --version 9.0.2
  dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.2
```
Jak już się zainstalują, upewnijcie się, że macie plik docker-compose.yml. Dodałem go do repo. Jak macie dockera wykonujecie polecenie:

```bash
  docker-compose up -d
  dotnet ef database update
```
Wychodząc oczywiście z założenia, że macie dockera i w kodzie są już jakieś migracje. Zgaduję, że za każdym razem, jak będą nowe migracje trzeba robić te databse update
Zanim uruchomicie bazę upewnijcie się, że działa. Jak? 

```bash
  docker logs -f oracle-db
```

Jak widzicie to:
```bash
  #########################
  DATABASE IS READY TO USE! 
  #########################

```
To znaczy, że git (git push badum tss)