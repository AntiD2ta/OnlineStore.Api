# OnlineStore API

### Pre-requisitos 📋

El proyecto fue desarrollado usando .NET SDK versión `5.0.2` usando ASP .NET Core v`5` y como base de datos Mysql server versión `5.7.21`.


### Instalación 🔧

Para ejecutar la api basta con ejecutar el siguiente comando en el cli desde la raíz del proyecto:

```
dotnet run
```

Para acceder a los endpoints usando swagger ejecute la api de la siguiente manera:

```
dotnet run environment=development
```

Acceda a la Api mediante https://localhost:5001/api y a la documentacion del la api dada por Swagger mediante https://localhost:5001/swagger/

Se incluye un script sql con el schema y los datos iniciales de la base de datos llamado `OnlineStore.sql` en la raíz del proyecto.

No obstante, si no se quiere usar el script, está integrado un database seeder con 3 usuarios, uno por cada rol.

## Autor ✒️


* **Miguel Tenorio**  - [AntiD2ta](https://github.com/AntiD2ta)
