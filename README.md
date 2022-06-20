# JobsityChallenge - Chatroom with stock bot

## Description
This .NET application consists of two projects: a chatroom and a bot. The chatroom is a MVC web application and the bot is a web API. The first one is a simple chatroom where a user can send messages to other users, besides commands to a bot. For now, the only command that the bot can handle is "/stock={STOCK_CODE}".

The bot API receives the command and executes it. The stock quote command makes a request to an [external API](https://stooq.pl/) and gets a CSV containing the stock information. The stock quote is parsed from the CSV and published on a RabbitMQ queue. The chatroom project has a background service responsible for consuming this queue and send the bot response message on the chatroom.

As a user it is possible to register, login, logout, talk to all users on the chatroom and send commands to the bot. The gif below demonstrates the application running:

![chat](https://user-images.githubusercontent.com/24402145/174518489-4eddbdb8-c340-45ab-b1a2-44949fbc2dcc.gif)

## How to run the project
To execute this application, you are going to need a RabbitMQ and a SQL Server. You can configure both on the *appsettings.json* file of both projects.
Besides that, you can also config the queue name and the stock API endpoint.

***The StockQueueName on both appsettings.json files should be the same for the application to work properly.***

The projects are configured to run on ports 7224 (chatroom) and 7154 (bot API) but you can change this on the *launchsettings.json* files, if you do so, don't forget to change the *BotApiEndpoint* on the chat project *appsettings.json* to point to the new port.

You also need to set up your solution to execute both projects, check out this [link](https://docs.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2022) for more details on how to do it.

After you have done the set up, your solution properties should look like the image below and that's it, you are all set to execute the project.

![image](https://user-images.githubusercontent.com/24402145/174517060-38fc3a1b-f400-4284-b1a6-ee651a50f07c.png)

## Built With

- [.NET](https://dotnet.microsoft.com/en-us/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server)
- [Bootstrap](https://getbootstrap.com/)
- [JQuery](https://jquery.com/)
