"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

$("#sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {    
    var currentDateTimeString = new Date().toLocaleString();
    var currentDateTimeStringFormatted = currentDateTimeString.replace(",", "");
    var div = $("<div></div>");
    
    div.html(`<b>${user}:</b><p>${message}</p><span class="time-right">${currentDateTimeStringFormatted}</span>`);
    div.addClass("msg-container");
    $("#messagesList").append(div);
});

connection.start().then(function () {
    var chatId = parseInt($("#chatId").val());

    $("#sendButton").disabled = false;
    connection.invoke("JoinGroup", chatId)
        .catch(err => {
            console.log(err);
        });
}).catch(function (err) {
    return console.error(err.toString());
});

$("#sendButton").on("click", function (event) {    
    debugger;
    var userId = $("#userId").val();
    var chatId = parseInt($("#chatId").val());
    var message = $("#messageInput").val();
    connection.invoke("SendMessage", message, chatId, userId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});