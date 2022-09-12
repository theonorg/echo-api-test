# Echo WebApp

## About The Project

This Echo WebApp is a really simple dotnet webapp + webapi that echoes the message you enter on WebUI.

When you echo a message a record is added to a MS SQL database to get a full log of all echoed message.

All application components are containerized to make it easier to use this simple app.

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white) ![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

## Getting Started

This application is container based so you only need to use docker compose to run it. So you need to ensure you have docker and docker-compose running on your machine.

To run this application you only need to run the following command on the root dir of this repo.

```bash
docker-compose up
```

This command will run 3 containers: MS SQL database, Echo API and Echo WebApp.

## Usage

To use this application you need to open a browser and navigate to:

```bash
http://localhost:8080
```

Now you may start echoing your messages!

To get access to echo history navigate to:

```bash
http://localhost:9001/log
```

Enjoy!