




$(function () {

    var connection = new signalR.HubConnectionBuilder().withUrl("/notificationhub").build();
    connection.on("ReceiveMessage", function (countryData) {
        datasource.add(new atlas.data.Feature(
            new atlas.data.Point([countryData.longitude, countryData.latitude]),
            {
                "mag": countryData.Cases
            }));
        console.log(countryData.Country + " " + countryData.lattitude + "  " + countryData.longitude);
    });
    connection.start().then(function () {
        console.log("SignalR connected..successfully");
    }).catch(function (err) {
        return console.error(err.toString());
    });

});