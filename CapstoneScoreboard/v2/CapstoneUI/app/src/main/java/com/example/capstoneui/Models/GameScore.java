package com.example.capstoneui.Models;

import java.util.List;

public class GameScore {

    private Integer homeTeamScore;
    private Integer awayTeamScore;
    private List<Integer> homeTeamFouls = null;
    private List<Integer> awayTeamFouls = null;
    private Integer homeTeamTimeoutsRemaining;
    private Integer awayTeamTimeoutsRemaining;
    private String gameTime;

    public Integer getHomeTeamScore() {
        return homeTeamScore;
    }

    public void setHomeTeamScore(Integer homeTeamScore) {
        this.homeTeamScore = homeTeamScore;
    }

    public Integer getAwayTeamScore() {
        return awayTeamScore;
    }

    public void setAwayTeamScore(Integer awayTeamScore) {
        this.awayTeamScore = awayTeamScore;
    }

    public List<Integer> getHomeTeamFouls() {
        return homeTeamFouls;
    }

    public void setHomeTeamFouls(List<Integer> homeTeamFouls) {
        this.homeTeamFouls = homeTeamFouls;
    }

    public List<Integer> getAwayTeamFouls() {
        return awayTeamFouls;
    }

    public void setAwayTeamFouls(List<Integer> awayTeamFouls) {
        this.awayTeamFouls = awayTeamFouls;
    }

    public Integer getHomeTeamTimeoutsRemaining() {
        return homeTeamTimeoutsRemaining;
    }

    public void setHomeTeamTimeoutsRemaining(Integer homeTeamTimeoutsRemaining) {
        this.homeTeamTimeoutsRemaining = homeTeamTimeoutsRemaining;
    }

    public Integer getAwayTeamTimeoutsRemaining() {
        return awayTeamTimeoutsRemaining;
    }

    public void setAwayTeamTimeoutsRemaining(Integer awayTeamTimeoutsRemaining) {
        this.awayTeamTimeoutsRemaining = awayTeamTimeoutsRemaining;
    }

    public String getGameTime() {
        return gameTime;
    }

    public void setGameTime(String gameTime) {
        this.gameTime = gameTime;
    }

}