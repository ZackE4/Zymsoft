package com.example.capstoneui.Models;

public class Scoring {
    private int homeTeamScore;
    private int awayTeamScore;
    private int gameId;


    public Scoring(int homeTeamScore, int awayTeamScore, int gameId) {
        this.homeTeamScore = homeTeamScore;
        this.awayTeamScore = awayTeamScore;
        this.gameId = gameId;
    }

    public int getHomeTeamScore() {
        return homeTeamScore;
    }

    public void setHomeTeamScore(int homeTeamScore) {
        this.homeTeamScore = homeTeamScore;
    }

    public int getAwayTeamScore() {
        return awayTeamScore;
    }

    public void setAwayTeamScore(int awayTeamScore) {
        this.awayTeamScore = awayTeamScore;
    }

    public int getGameId() {
        return gameId;
    }

    public void setGameId(int gameId) {
        this.gameId = gameId;
    }
}

