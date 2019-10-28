package com.example.capstonescoreboardapp;

public class Fouls {

    private int homeTeamFouls;
    private int awayTeamFouls;
    private int gameId;

    public int getHomeTeamFouls() {
        return homeTeamFouls;
    }

    public void setHomeTeamFouls(int homeTeamFouls) {
        this.homeTeamFouls = homeTeamFouls;
    }

    public int getAwayTeamFouls() {
        return awayTeamFouls;
    }

    public void setAwayTeamFouls(int awayTeamFouls) {
        this.awayTeamFouls = awayTeamFouls;
    }

    public int getGameId() {
        return gameId;
    }

    public void setGameId(int gameId) {
        this.gameId = gameId;
    }
}
