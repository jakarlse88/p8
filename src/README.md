# California Health

## Running
##### Core services only
Docker must be running, and must be assigned at least 3GB of memory.

1. Clone/download this repo
2. Open your terminal of choice 
3. Navigate to the project's ``src/`` directory 
4. ``docker-compose up [-d]`` to bring the services up
5. ``docker-compose down`` to bring the services down

##### Core services + Elasticsearch/Kibana
Docker must be running, and must be assigned at least 4GB of memory.

1. Clone/download this repo
2. Open your terminal of choice 
3. Navigate to the project's ``src/`` directory 
4. ``docker-compose -f docker-compose.yml -f docker-compose.prod.yml up [-d]`` to bring the services up
5. ``docker-compose -f docker-compose.yml -f docker-compose.prod.yml down`` to bring the services down