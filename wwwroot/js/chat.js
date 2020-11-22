"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

//receiving messages
connection.on("ReceiveMessage", function (user, message) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //var encodedMsg = user + " says: " + msg;
    var encodedMsg = user + " says: " + message;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    var groupName = document.getElementById("groupName").value;
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();

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
      
        if (message != "") {
            connection.invoke("SendMessage", message).catch(function (err) {
                return console.error(err.toString());
            });
        }
        event.preventDefault();
        document.getElementById("messageInput").value = "";
    });
}
//sending messages to the private chat:
else if (document.getElementById("mainChat").value == 0) {

    connection.on('Notify', function (message) {
        let notifyElem = document.createElement("b");
        notifyElem.appendChild(document.createTextNode(message));
        let elem = document.createElement("p");
        elem.appendChild(notifyElem);
        document.getElementById("messagesList").appendChild(elem);
    });

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var message = document.getElementById("messageInput").value;
        var user = document.getElementById("userName").value;
        if (message != "") {
        connection.invoke("SendMessageUser", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        }
        event.preventDefault();
        document.getElementById("messageInput").value = "";
    });
}
//sending messages to the chat room:
else if (document.getElementById("mainChat").value == 3)
{
   
    connection.on('Notify', function (message) {
        let notifyElem = document.createElement("b");
        notifyElem.appendChild(document.createTextNode(message));
        let elem = document.createElement("p");
        elem.appendChild(notifyElem);
        document.getElementById("messagesList").appendChild(elem);
    });
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var message = document.getElementById("messageInput").value;
        var groupName = document.getElementById("groupName").value;
        if (message != "") {
        connection.invoke("SendMessageGroup", message, groupName).catch(function (err) {
            return console.error(err.toString());
        });
        }
        event.preventDefault();
        document.getElementById("messageInput").value = "";
    });
}
////join chat
//document.getElementById("joinButton").addEventListener("click", function (event) {
//    var groupName = document.getElementById("groupName").value;
//    //var groupId = document.getElementById("groupId").value;
//    //window.location.href = '/Chat/JoinGroup/' + groupId;
//    connection.invoke("JoinGroup", groupName).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});
////leave chat
//document.getElementById("leaveButton").addEventListener("click", function (event) {
//    var groupName = document.getElementById("groupName").value;
//    var groupId = document.getElementById("groupId").value;
//    //window.location.href = '/Chat/LeaveGroup/' + groupId;
//    connection.invoke("LeaveGroup", groupName).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});


