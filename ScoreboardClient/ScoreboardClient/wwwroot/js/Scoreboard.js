var TimerTask;

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/scoreboardHub").build();

connection.start().then(function () {
    //alert("Scoreboard Connected!");
    console.log("Scoreboard Connected");
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
    if (parseInt($('#homeTeamScore').html()) !== gameScore.homeTeamScore || parseInt($('#awayTeamScore').html()) !== gameScore.awayTeamScore) {
        $('#shotclock').html(24);
    }
    $('#homeTeamScore').html(gameScore.homeTeamScore);
    $('#awayTeamScore').html(gameScore.awayTeamScore);
    $('#homeTeamFouls').html(gameScore.homeTeamFouls.reduce((a, b) => a + b, 0));
    $('#awayTeamFouls').html(gameScore.awayTeamFouls.reduce((a, b) => a + b, 0));

    if (parseInt($('#period').html()) === 1) {
        $('#homeTeamQuarterFouls').val(gameScore.homeTeamFouls[0]);
        $('#awayTeamQuarterFouls').val(gameScore.awayTeamFouls[0]);
    } else if (parseInt($('#period').html()) === 2) {
        $('#homeTeamQuarterFouls').val(gameScore.homeTeamFouls[1]);
        $('#awayTeamQuarterFouls').val(gameScore.awayTeamFouls[1]);
    } else if (parseInt($('#period').html()) === 3) {
        $('#homeTeamQuarterFouls').val(gameScore.homeTeamFouls[2]);
        $('#awayTeamQuarterFouls').val(gameScore.awayTeamFouls[2]);
    } else if (parseInt($('#period').html()) === 4) {
        $('#homeTeamQuarterFouls').val(gameScore.homeTeamFouls[3]);
        $('#awayTeamQuarterFouls').val(gameScore.awayTeamFouls[3]);
    }

    if (parseInt($('#homeTeamQuarterFouls').val()) > 4) {
        $('#homeBonus').html('< BONUS');
    }
    if (parseInt($('#awayTeamQuarterFouls').val()) > 4) {
        $('#awayBonus').html('BONUS >');
    }
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

connection.on("saveFoul", function (playerId) {
    SaveFoul(playerId);
});

connection.on("saveScore", function (points, playerId) {
    SaveScore(parseInt(points), parseInt(playerId));
});

connection.on("ReceiveCallTimeout", function (side) {
    if (side === "Home") {
        if (parseInt($('#homeTeamTimeouts').html().trim()) > 0) {
            $('#homeTeamTimeouts').html(parseInt($('#homeTeamTimeouts').html().trim()) - 1);
            playHorn();
            StopTimer();
        }
    } else if (side === "Away") {
        if (parseInt($('#awayTeamTimeouts').html().trim()) > 0) {
            $('#awayTeamTimeouts').html(parseInt($('#awayTeamTimeouts').html().trim()) - 1);
            playHorn();
            StopTimer();
        }
    }
});

connection.on("ReceiveSetTimeout", function (side, timeouts) {
    if (side === "Home") {
        $('#homeTeamTimeouts').html(timeouts);
    } else if (side === "Away") {
        $('#awayTeamTimeouts').html(timeouts);
    }
});

connection.on("RecieveSwitchMediaPage", function () {
    showMediaPage();
    if ($('#isVideoShowing').val() === "true") {
        var vid = document.getElementById("media-video");
        vid.play();
    }
});

connection.on("RecieveSwitchScoreboardPage", function () {
    showScoreboardPage();
    var vid = document.getElementById("media-video");
    vid.pause();
});

connection.on("RecievePlayVideo", function (videoFileName) {
    showMediaPage();
    var vid = document.getElementById("media-video");
    $('#activeVideo').attr('src', "/media/" + videoFileName);
    vid.load();
    if ($('#isVideoShowing').val() === "false") {
        $('#imageAdDiv').fadeOut();
        $('#videoPlayerDiv').fadeIn();
    }
    setTimeout(function () {
        vid.play();
    }, 500);
    $('#isVideoShowing').val("true"); 
});
connection.on("RecieveShowImage", function (imgFileName) {
    showMediaPage();
    $('#imgAd').attr('src', "/media/" + imgFileName);
    if ($('#isVideoShowing').val() === "true") {
        var vid = document.getElementById("media-video");
        vid.pause();
        $('#videoPlayerDiv').fadeOut();
        $('#imageAdDiv').fadeIn();
    }
    $('#isVideoShowing').val("false"); 
});

connection.on("NewScoreboardOpened", function () {
    alert("Another scoreboard screen has been opened, this screen will now be redirected");
    window.location.href = "/Game?errorMsg=Another scoreboard screen was opened.";
});

function playHorn() {
    var audio = new Audio('/audio/buzzer.mp3');
    audio.play();
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
            $('#homeTeamQuarterFouls').val(0);
            $('#awayTeamQuarterFouls').val(0);
            $('#homeBonus').html('');
            $('#awayBonus').html('');
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

    // update time back to Connector
    $.ajax({
        type: "POST",
        url: "/Screen/SaveGameTime",
        data: data,
        success: function (responseData) {
        },
        dataType: "json"
    });
}

function SaveScore(points, playerId) {
    var data = {};
    data.period = parseInt($('#period').html());
    data.minutes = parseInt($('#TimerMins').html());
    data.seconds = parseInt($('#TimerSeconds').html());
    data.playerId = playerId;
    data.points = points;

    // record time of scoring play for stats
    $.ajax({
        type: "POST",
        url: "/Screen/SaveRecordedPoints",
        data: data,
        success: function (responseData) {
        },
        dataType: "json"
    });
}

function SaveFoul(playerId) {
    var data = {};
    data.period = parseInt($('#period').html());
    data.minutes = parseInt($('#TimerMins').html());
    data.seconds = parseInt($('#TimerSeconds').html());
    data.playerId = playerId;

    //record time of foul for stats
    $.ajax({
        type: "POST",
        url: "/Screen/SaveRecordedFoul",
        data: data,
        success: function (responseData) {
        },
        dataType: "json"
    });
}

function showMediaPage() {
    //$('#switchMediaPage').trigger('click');
    location.hash = "#mediaPage";
}

function showScoreboardPage() {
    //$('#switchScoreboardPage').trigger('click');
    location.hash = "#scoreboardPage";
}

$(document).ready(function () {
    var vids = $("video");
    $.each(vids, function () {
        this.controls = false;
    }); 

    var data = {};

    $.ajax({
        type: "POST",
        url: "/Screen/UpdateScreenOpen",
        data: data,
        success: function (responseData) {
        },
        dataType: "json"
    });
});