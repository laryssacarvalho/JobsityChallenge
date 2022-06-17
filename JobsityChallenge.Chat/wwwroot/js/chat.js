"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

$("#sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = $("<li></li>").text(user + ": " + message);
    li.addClass("list-group-item");
    $("#messagesList").append(li);
});

connection.start().then(function () {
    $("#sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

$("#sendButton").on("click", function (event) {
    var user = $("#userName").val();
    var message = $("#messageInput").val();
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});