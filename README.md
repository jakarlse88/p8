# Californian Health

Solution for the 8th project of the [OpenClassrooms .NET Back-End Developer Path](https://openclassrooms.com/en/paths/156-back-end-developer-net).

## Running
##### Core services only
Docker must be running, and must be assigned at least 3GB of memory.

1. Clone/download this repo
2. Open your terminal of choice 
3. Navigate to the project's ``src/`` directory 
4. ``docker-compose up -d`` to bring the services up
5. ``docker-compose down`` to bring the services down

##### Core services + Elasticsearch/Kibana
Docker must be running, and must be assigned at least 4GB of memory.

1. Clone/download this repo
2. Open your terminal of choice 
3. Navigate to the project's ``src/`` directory 
4. ``docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d`` to bring the services up
5. ``docker-compose -f docker-compose.yml -f docker-compose.prod.yml down`` to bring the services down

Note that in either case, it takes some time for the containers to start up and become healthy. 

##### Patient Overview
In lieu of a actual membership system, the application is seeded on startup with the following patient data. Bookings made with patient data not found in the below form will be rejected by the booking service.

| First Name | Last Name | Date of Birth<sup>1</sup> |
| ---------- | --------- | ------------- |
| Alima      | Rankin    | 01/01/2000    |
| Chelsie    | Regan     | 17/07/1980    |
| Michalina  | Dejesus   | 12/03/1997    |
| Daniaal    | Hill      | 27/05/2007    |
| Adele      | Benjamin  | 29/11/1989    |
| Rhodri     | Ellis     | 09/09/1977    |
| Hakeem     | Conner    | 11/04/2011    |
| Nur        | Lim       | 19/02/1963    |
| Kenzo      | Traynor   | 30/10/1990    |
| Nyla       | Davey     | 13/06/2007    |                   

<sup>1</sup>DD/MM/YYYY
