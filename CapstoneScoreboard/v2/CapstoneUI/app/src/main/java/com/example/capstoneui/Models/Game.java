package com.example.capstoneui.Models;


import java.util.List;

public class Game {

    private HomeTeam homeTeam;
    private AwayTeam awayTeam;
    private GameScore gameScore;
    private List<HomeTeamRoster> homeTeamRoster = null;
    private List<AwayTeamRoster> awayTeamRoster = null;

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

    public List<HomeTeamRoster> getHomeTeamRoster() {
        return homeTeamRoster;
    }

    public void setHomeTeamRoster(List<HomeTeamRoster> homeTeamRoster) {
        this.homeTeamRoster = homeTeamRoster;
    }

    public List<AwayTeamRoster> getAwayTeamRoster() {
        return awayTeamRoster;
    }

    public void setAwayTeamRoster(List<AwayTeamRoster> awayTeamRoster) {
        this.awayTeamRoster = awayTeamRoster;
    }

}