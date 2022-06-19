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
    $("#sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

$("#sendButton").on("click", function (event) {    
    var userId = $("#userId").val();
    var message = $("#messageInput").val();
    connection.invoke("SendMessage", message, userId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});