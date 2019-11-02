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
            var shotClock = parseInt($('#shotclock').html());
            if (shotClock === 0) {
                $('#shotclock').html(24);
            }
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

connection.on("RecieveResetShotClock", function () {
    $('#shotclock').html(24);
});

connection.on("RecieveSetGameClock", function (mins, seconds) {
    if ($('#timerRunning').val() === "false") {
        var GameSecondsString = seconds.toString().length > 1 ? seconds.toString() : "0" + seconds;
        $('#TimerSeconds').html(GameSecondsString);
        $('#TimerMins').html(mins);
        StopTimer();
    }
});

connection.on("RecieveSetShotClock", function (value) {
    if ($('#timerRunning').val() !== "true") {
        $('#shotclock').html(value);
    }
});

connection.on("ReceivePlayHorn", function () {
    playHorn();
});

function playHorn() {
    var audio = new Audio('/audio/buzzer.mp3');
    var audioPromise = audio.play();
    if (audioPromise !== null) {
        audioPromise.catch(() => { audio.play(); })
    }
}

function tickShotClock() {
    var shotClock = parseInt($('#shotclock').html());
    if (shotClock > 0) {
        shotClock--;
        $('#shotclock').html(shotClock);
    }
    if (shotClock === 0) {
        playHorn();
        StopTimer();
    }
}

function tickTimer() {
    var GameSeconds = parseInt($('#TimerSeconds').html());
    var GameMinutes = parseInt($('#TimerMins').html());
    if (GameSeconds > 0) {
        GameSeconds--;
        var GameSecondsString = GameSeconds.toString().length > 1 ? GameSeconds : "0" + GameSeconds;
        $('#TimerSeconds').html(GameSecondsString);
        tickShotClock();
        if (GameMinutes === 0 && GameSeconds === 0) {
            playHorn();
        }
    }
    else if (GameMinutes > 0) {
        GameSeconds = 59;
        GameMinutes--;
        var GameSecondsString = GameSeconds.toString().length > 1 ? GameSeconds : "0" + GameSeconds;
        $('#TimerSeconds').html(GameSecondsString);
        $('#TimerMins').html(GameMinutes);
        tickShotClock();
        if (GameMinutes === 0 && GameSeconds === 0) {
            playHorn();
        }
    }
    else {
        if (parseInt($('#period').html()) < 4) {
            $('#TimerSeconds').html("00");
            $('#TimerMins').html(12);
            var newPeriod = parseInt($('#period').html()) + 1;
            $('#period').html(newPeriod);
            $('#shotclock').html(24);
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