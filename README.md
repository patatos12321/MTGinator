# MTGinator

## How to contribute
Please use the trello board to monitor bugs, enhancements and new features: https://trello.com/b/b4E6ND6D/mtginator-web-app

## How to build Docker image

`docker build -t mtginator -m 2GB ./`

## How to run Docker image

`docker run -d -v <Path to DB>:/db -p 80:80 -p 443:443 mtginator`

or

`docker-compose up -d`