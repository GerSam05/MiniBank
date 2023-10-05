<p align="center">
  <img width="626" height="134" src="https://github.com/GerSam05/GalponIndustrial/assets/146037370/6623ae64-08b3-46f2-b04b-74edfbade9e8"><br>
  <img src="https://komarev.com/ghpvc/?username=gersam05&label=Profile%20views&color=0e75b6&style=flat" alt="gersam05" />
</p>


# 🏦API Restfull MiniBank

## Introducción
- **MiniBank** es una **API Restfull** bancaria que se puede utilizar para realizar una variedad de tareas como: consultar información sobre las cuentas bancarias de los clientes,
el balance general, tipo de cuenta, fecha de registro, entre otros.
- También se pueden administrar los propietarios de las cuentas bancarias: agregar usuarios, eliminar y actualizar.
- Todo esto con operaciones sencillas y remotas de tipo **CRUD** en una base de datos constituida por cinco **tablas relacionadas** (imgs:1 y 2).
- La Api posee dos controladores para las dos tablas principales, uno para la tabla **Client** y otro para la tabla **Account**.
-  La metodología utilizada fué **DataBase First** con el **ORM** de .**NET EntityFrameworkCore**.
-  El modelo está basado en los patrones de diseño **Dependency Injection** y **Repository**; para optimizar la funcionalidad de la Api se incorporó la arquitectura **Servicio Layer** para cada controlador,
repetando así el requerimiento de al menos una capa entre el controlador y el contexto.
- La Api posee una clase "**APIResponse**" con métodos, encargada de retornar una respuesta estandarizada para todas las peticiones realizadas desde los endpoints.

<br>

## Tecnologías utilizadas

<p align="left"> <a href="https://www.w3schools.com/cs/" target="_blank" rel="noreferrer"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/> </a> <a href="https://dotnet.microsoft.com/" target="_blank" rel="noreferrer"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dot-net/dot-net-original-wordmark.svg" alt="dotnet" width="40" height="40"/> </a> <a href="https://git-scm.com/" target="_blank" rel="noreferrer"> <img src="https://www.vectorlogo.zone/logos/git-scm/git-scm-icon.svg" alt="git" width="40" height="40"/> </a> <a href="https://www.microsoft.com/en-us/sql-server" target="_blank" rel="noreferrer"> <img src="https://www.svgrepo.com/show/303229/microsoft-sql-server-logo.svg" alt="mssql" width="40" height="40"/> </a> <a href="https://postman.com" target="_blank" rel="noreferrer"> <img src="https://www.vectorlogo.zone/logos/getpostman/getpostman-icon.svg" alt="postman" width="40" height="40"/> </a> </p>
<br>

## Video de Referencia
### La siguiente lista de reproducción fué la fuente principal para el desarrollo de esta API:
`<link>` : <https://www.youtube.com/watch?v=nsWyR-V4fAw&list=PL0-hIHBwsOM5tjSkf1KRvS93aT6JbL8If>
<br><br>

## Imágenes

Tablas de la base de datos:

![Relatioship](https://github.com/GerSam05/MiniBank/assets/146037370/b3485bbf-f3f8-4515-b57a-728c315ab813)
> Imagen 1: relación entre las tablas
<br>

![selec tablas](https://github.com/GerSam05/MiniBank/assets/146037370/d0686d01-39de-4114-be82-f9841aab5162)
> Imagen 2: query "select * from" en las tablas principales (Client and Account).
<br>

---

Espero que el repositorio les sea de utilidad👍🏻💡!!!...
 
> 📁 Todos mis proyectos estan disponibles en [![GitHub repository](https://img.shields.io/badge/repository-github-orange)](https://github.com/GerSam05?tab=repositories)
