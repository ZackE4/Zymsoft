var TimerTask;

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/scoreboardHub").build();

connection.start().then(function () {
    alert("Scoreboard Connected!");
}).catch(function (err) {
    alert("Scoreboard Connection Error!");
});

connection.on("RecieveToggleTimer", function (action) {
    if (action === "start")
    {
        //alert("start timer!");
        if ($('#timerRunning').val() !== "true") {
            StartTimer();
        }
    }
    else if (action === "stop")
    {
        //alert("stop timer!");
        if ($('#timerRunning').val() === "true") {
            StopTimer();
        }
    }
});

connection.on("updateScore", function (gameScore) {
    $('#homeTeamScore').html(gameScore.homeTeamScore);
    $('#awayTeamScore').html(gameScore.awayTeamScore);
    $('#homeTeamFouls').html(gameScore.homeTeamFouls.reduce((a, b) => a + b, 0));
    $('#awayTeamFouls').html(gameScore.awayTeamFouls.reduce((a, b) => a + b, 0));
});

function tickTimer() {
    var GameSeconds = parseInt($('#TimerSeconds').html());
    var GameMinutes = parseInt($('#TimerMins').html());
    if (GameSeconds > 0) {
        GameSeconds--;
        var GameSecondsString = GameSeconds.toString().length > 1 ? GameSeconds : "0" + GameSeconds;
        $('#TimerSeconds').html(GameSecondsString);
    }
    else if (GameMinutes > 0) {
        GameSeconds = 59;
        GameMinutes--;
        var GameSecondsString = GameSeconds.toString().length > 1 ? GameSeconds : "0" + GameSeconds;
        $('#TimerSeconds').html(GameSecondsString);
        $('#TimerMins').html(GameMinutes);
    }
    else {
        if (parseInt($('#period').html()) < 4) {
            $('#TimerSeconds').html("00");
            $('#TimerMins').html(12);
            var newPeriod = parseInt($('#period').html()) + 1;
            $('#period').html(newPeriod);
        }
        StopTimer();
    }
}

function StartTimer() {
    $('#timerRunning').val("true");
    this.TimerTask = setInterval(tickTimer, 1000);
}

function StopTimer() {
    $('#timerRunning').val("false");
    clearInterval(this.TimerTask);

    var data = {};
    data.period = parseInt($('#period').html());
    data.minutes = parseInt($('#TimerMins').html());
    data.seconds = parseInt($('#TimerSeconds').html());

    $.ajax({
        type: "POST",
        url: "/Screen/SaveGameTime",
        data: data,
        success: function (responseData) {
        },
        dataType: "json"
    });
    // update time back to Connector...
}

//$(document).ready(function () {

//});