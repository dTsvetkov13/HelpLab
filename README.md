## HelpLab ##

### Purpose ###

HelpLab is a platform for questions and answers related with the school material. It is implemented with Microservice Architecture pattern

### Technologies ###

Angular
ASP.NET Core
AMQP (RabbitMQ)

### Building and Running the project ###

**Windows**

To build the solution, you need to install Visual Studio. Also, you will need Docker, which you can download from here - https://www.docker.com/get-started.

1. Open the solution in VS.
2. Set your server name into appsettings.json in Microservices.Answers, Microservices.Posts and Microservices.Users
3. Run PowerShell in project's root folder and type: dotnet ef database update
4. Run Docker
5. Visit this link https://www.rabbitmq.com/download.html. You can either download and install RabbitMQ or run this line in console:
``` shell
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```
4. Right click on the solution -> Common Properties -> Startup  Project -> Multiple startup projects -> Select the projects you want to run. For the last step I recommend to select WebClient, APIGateway, Microservices.Users, Microservices.Posts, Microservices.Answers.
