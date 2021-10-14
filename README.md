# Ericsson assignment project
![](/images/logo.PNG)
### This repository is include two applications called Server and Client

- Server Application is a message publisher to Message-Bus wich has written by Python
- Client Application is a message consumer  and written by .net 5
- Comminucation pattern between two application is Pub/Sub so we can have multiple client instance
- RabbitMQ has messagebus role in the system
- These two application are available on DockerHub and also have the ability to simply build image on local By the Dockerfile and Docker-Compose


## - How to lunch

Base on previous explanition we have multiple option to run these applications

<details>
  <summary>
    docker-compose.yml [Expand for see it]
  </summary>
  <pre>
  version: '3'
services:
  rabbitmq:
    image: "rabbitmq:3-management"
    hostname: "rabbitmq"
    environment:
      RABBITMQ_ERLANG_COOKIE: "SWQOKODSQALRPCLNMEQG"
      RABBITMQ_DEFAULT_USER: "admin"
      RABBITMQ_DEFAULT_PASS: "@Ericsson2021"
      RABBITMQ_DEFAULT_VHOST: "/"
    ports:
      - "15672:15672"
      - "5672:5672"
    labels:
      NAME: "rabbitmq"
    restart: always
  server:
    image: "ashkanyarmoradi/ericsson-server"
    command: tail -F anything
    environment:
      override_rabbitmq: true
      rabbitmq_username: "admin"
      rabbitmq_password: "@Ericsson2021"
      rabbitmq_host: "rabbitmq"
      rabbitmq_port: 5672
      rabbitmq_exchange: "main"
    labels:
      NAME: "server"
    restart: always
  client:
    image: "ashkanyarmoradi/ericsson-client"
    environment:
      RabbitMq:Username: "admin"
      RabbitMq:Password: "@Ericsson2021"
      RabbitMq:Host: "rabbitmq"
      RabbitMq:Port: 5672
      RabbitMq:Exchange: "main"
    restart: always
</pre>
</details>

<br>

### - Pull Images [Server, client and RabbitMQ]
In the root directory just execute command in below (all the setting for connecting between applications store in environment setting of docker-compose file)

```docker compose up```

<details>
  <summary>
    docker-compose up [Expand for see it]
  </summary>

  ![](/images/docker-compose-up.PNG)
</details>

<br>

### - Pull Images [Server, client] without pulling RabbitMQ Image

I create a RabbitMQ server and applications connect to that , so you being comfortable and doesn't need to pull the rabbitmag, as you can see in the docker-compose file we don't need to add any environment setting, please execute the below command in the root directory

```docker-compose -f docker-compose.min.yml up```

### - Build Images [Server, client] And Pull RabbitMQ Image

With this option after running the command in below the docker builds two image of the server and client application and then pull the image of RabbitMQ (As you can see all application's setting is mapped in the environment of docker-compose file)

```docker-compose -f docker-compose.build.yml up```

### - Build Images [Server, client] And without pulling RabbitMQ Image

With this option when you execute the command in below the application starts to build

```docker-compose -f docker-compose.build.min.yml up```

## - How to use application

The client application try to connect to RabbitMQ after the connection is successfull, the application came up with log, then As you know the server application works in  interactive mood so please go to container host with cmd/bash and execute the bellow command

```python main.py "Hello"```

after that you see the log of client application that say Hello as well

<details>
  <summary>
    Response [Expand for see it]
  </summary>

  ![](/images/log.PNG)
</details>

<br>
Thank you for your time
Ashkan Yarmoradi