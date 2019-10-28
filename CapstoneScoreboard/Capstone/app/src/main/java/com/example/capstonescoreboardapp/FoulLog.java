package com.example.capstonescoreboardapp;


public class FoulLog {
    private String apiToken;
    private String leagueKey;
    private int fouldLogId;
    private String gameTime;
    private int playerId;
    private int gameId;

    public FoulLog(String apiToken, String leagueKey, String gameTime, int playerId, int gameId) {
        this.apiToken = apiToken;
        this.leagueKey = leagueKey;
        this.gameTime = gameTime;
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

    public int getFouldLogId() {
        return fouldLogId;
    }

    public void setFouldLogId(int fouldLogId) {
        this.fouldLogId = fouldLogId;
    }

    public String getGameTime() {
        return gameTime;
    }

    public void setGameTime(String gameTime) {
        this.gameTime = gameTime;
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
