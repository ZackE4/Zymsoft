package com.example.capstoneui.Models;



public class League {

    private Login login;
    private int leagueId;
    private String leagueName;
    private String logo;
    private String leagueKey;

    public League(Login login, int leagueId, String leagueName, String logo, String leagueKey) {
        this.login = login;
        this.leagueId = leagueId;
        this.leagueName = leagueName;
        this.logo = logo;
        this.leagueKey = leagueKey;
    }

    public Login getLogin() {
        return login;
    }

    public void setLogin(Login login) {
        this.login = login;
    }

    public int getLeagueId() {
        return leagueId;
    }

    public void setLeagueId(int leagueId) {
        this.leagueId = leagueId;
    }

    public String getLeagueName() {
        return leagueName;
    }

    public void setLeagueName(String leagueName) {
        this.leagueName = leagueName;
    }

    public String getLogo() {
        return logo;
    }

    public void setLogo(String logo) {
        this.logo = logo;
    }

    public String getLeagueKey() {
        return leagueKey;
    }

    public void setLeagueKey(String leagueKey) {
        this.leagueKey = leagueKey;
    }
}
