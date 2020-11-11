"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

//receiving messages
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//sending messages to the main chat:
if (document.getElementById("mainChat").value == 1) {

    //notification function for incoming and outgoing users
    connection.on('Notify', function (message) {

        let notifyElem = document.createElement("b");
        notifyElem.appendChild(document.createTextNode(message));
        let elem = document.createElement("p");
        elem.appendChild(notifyElem);
        document.getElementById("messagesList").appendChild(elem);
    });

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var message = document.getElementById("messageInput").value;
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        document.getElementById("messageInput").value = "";
    });
}
//sending messages to the private chat:
else if (document.getElementById("mainChat").value == 0) {

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var message = document.getElementById("messageInput").value;
        var user = document.getElementById("userName").value;
        connection.invoke("SendMessageUser", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        document.getElementById("messageInput").value = "";
    });
}
//sending messages to the chat room:
else if (document.getElementById("mainChat").value == 3)
{
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var message = document.getElementById("messageInput").value;
        var user = document.getElementById("userName").value;
        var groupName = document.getElementById("groupName").value;
        connection.invoke("SendMessageGroup", user, message, groupName).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        document.getElementById("messageInput").value = "";
    });
}

//join chat
document.getElementById("joinButton").addEventListener("click", function (event) {
    var groupName = document.getElementById("groupName").value;
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
//leave chat
document.getElementById("leaveButton").addEventListener("click", function (event) {
    var groupName = document.getElementById("groupName").value;
    connection.invoke("LeaveGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
