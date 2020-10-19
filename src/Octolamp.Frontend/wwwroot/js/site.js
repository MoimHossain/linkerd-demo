

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationhub").build();



connection.on("ReceiveMessage", function (user, message) {

    console.log(user, message);

});

connection.start().then(function () {
    console.log("SignalR connected..successfully");
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});