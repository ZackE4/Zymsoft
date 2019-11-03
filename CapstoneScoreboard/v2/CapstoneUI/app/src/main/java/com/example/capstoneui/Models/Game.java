package com.example.capstoneui.Models;


import java.util.List;

public class Game {

    private HomeTeam homeTeam;
    private AwayTeam awayTeam;
    private GameScore gameScore;
    private List<Object> homeTeamRoster = null;
    private List<Object> awayTeamRoster = null;

    public HomeTeam getHomeTeam() {
        return homeTeam;
    }

    public void setHomeTeam(HomeTeam homeTeam) {
        this.homeTeam = homeTeam;
    }

    public AwayTeam getAwayTeam() {
        return awayTeam;
    }

    public void setAwayTeam(AwayTeam awayTeam) {
        this.awayTeam = awayTeam;
    }

    public GameScore getGameScore() {
        return gameScore;
    }

    public void setGameScore(GameScore gameScore) {
        this.gameScore = gameScore;
    }

    public List<Object> getHomeTeamRoster() {
        return homeTeamRoster;
    }

    public void setHomeTeamRoster(List<Object> homeTeamRoster) {
        this.homeTeamRoster = homeTeamRoster;
    }

    public List<Object> getAwayTeamRoster() {
        return awayTeamRoster;
    }

    public void setAwayTeamRoster(List<Object> awayTeamRoster) {
        this.awayTeamRoster = awayTeamRoster;
    }

}