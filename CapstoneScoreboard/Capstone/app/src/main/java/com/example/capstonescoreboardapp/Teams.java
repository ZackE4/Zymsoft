package com.example.capstonescoreboardapp;

public class Teams {

    private int teamId;
    private String teamName;
    private String coachName;
    private String logo;
    private int leagueId;




    public int getTeamId() {
        return teamId;
    }

    public void setTeamId(int teamId) {
        this.teamId = teamId;
    }

    public String getTeamname() {
        return teamName;
    }

    public void setTeamname(String teamname) {
        this.teamName = teamname;
    }

    public String getCoachName() {
        return coachName;
    }

    public void setCoachName(String coachName) {
        this.coachName = coachName;
    }

    public String getLogo() {
        return logo;
    }

    public void setLogo(String logo) {
        this.logo = logo;
    }

    public int getLeagueId() {
        return leagueId;
    }

    public void setLeagueId(int leagueId) {
        this.leagueId = leagueId;
    }
}
