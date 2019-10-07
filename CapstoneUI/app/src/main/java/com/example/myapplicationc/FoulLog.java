package com.example.myapplicationc;


import java.util.Date;

public class FoulLog {
    private int fouldLogId;
    private Date gameTime;
    private int playerId;
    private int gameId;


    public int getFouldLogId() {
        return fouldLogId;
    }

    public void setFouldLogId(int fouldLogId) {
        this.fouldLogId = fouldLogId;
    }

    public Date getGameTime() {
        return gameTime;
    }

    public void setGameTime(Date gameTime) {
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
