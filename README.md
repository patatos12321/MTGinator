# MTGinator

## How to build Docker image

`docker build -t MTGinator -m 2GB ./`

## How to run Docker image

`docker run -d -v <Path to DB>:/db -p 80:80 -p 443:443 MTGinator`

or

`docker-compose up -d`