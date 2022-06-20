# JobsityChallenge

This .NET application consists of two projects: a chatroom and a bot. The chatroom is a MVC web application and the bot is a web API. The first one is a simple chatroom where any logged in user can send messages and the user can also send a message to the bot. For now, the only command that the bot can handle is "/stock={STOCK_CODE}".

The bot API receives the command and executes it. The stock command makes a request to an external API () and gets a CSV containing the stock information. The stock quote is parsed from the CSV and published on a RabbitMQ queue. The chatroom project has a service responsible for consuming this queue and send the bot "response" message on the chatroom.

## Instructions
To execute this application, you are going to need a RabbitMQ and a SQL Server. You can configure both on the appsettings.json file of both projects.

You also need to set up your solution to execute both projects, check out this link for more info.
