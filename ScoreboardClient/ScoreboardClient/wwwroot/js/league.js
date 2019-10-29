$(document).ready(function () {
    $('#ChangeLeagueDiv').hide();
    $('#ChangeLeagueBtn').click(function () {
        $('#ChangeLeagueDiv').toggle();
    });
});

$(function () {
    $('.datepicker').datepicker();
});

function showChangeLeague() {
    $('#ChangeLeagueDiv').show();
}