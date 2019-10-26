package com.example.myapplicationc;


import java.util.Date;

public class ScoringLog {
    private String apiToken;
    private String leagueKey;
    private int scoringLogId;
    private String gameTime;
    private int points;
    private int playerId;
    private int gameId;

    public ScoringLog(String apiToken, String leagueKey, String gameTime, int points, int playerId, int gameId) {
        this.apiToken = apiToken;
        this.leagueKey = leagueKey;
        this.gameTime = gameTime;
        this.points = points;
        this.playerId = playerId;
        this.gameId = gameId;
    }

    public String getApiToken() {
        return apiToken;
    }

    public void setApiToken(String apiToken) {
        this.apiToken = apiToken;
    }

    public String getLeagueKey() {
        return leagueKey;
    }

    public void setLeagueKey(String leagueKey) {
        this.leagueKey = leagueKey;
    }

    public int getScoringLogId() {
        return scoringLogId;
    }

    public void setScoringLogId(int scoringLogId) {
        this.scoringLogId = scoringLogId;
    }

    public String getGameTime() {
        return gameTime;
    }

    public void setGameTime(String gameTime) {
        this.gameTime = gameTime;
    }

    public int getPoints() {
        return points;
    }

    public void setPoints(int points) {
        this.points = points;
    }

    public int getPlayerId() {
        return playerId;
    }

    public void setPlayerId(int playerId) {
        this.playerId = playerId;
    }

    public int getGameId() {
        return gameId;
    }

    public void setGameId(int gameId) {
        this.gameId = gameId;
    }
}
