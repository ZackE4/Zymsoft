package com.example.capstoneui.Models;

class Login {
    private int loginId;
    private String loginTimestamp;
    private String expiry;
    private String loginKey;
    private int leagueId;

    public Login(int loginId, String loginTimestamp, String expiry, String loginKey, int leagueId) {
        this.loginId = loginId;
        this.loginTimestamp = loginTimestamp;
        this.expiry = expiry;
        this.loginKey = loginKey;
        this.leagueId = leagueId;
    }

    public int getLoginId() {
        return loginId;
    }

    public void setLoginId(int loginId) {
        this.loginId = loginId;
    }

    public String getLoginTimestamp() {
        return loginTimestamp;
    }

    public void setLoginTimestamp(String loginTimestamp) {
        this.loginTimestamp = loginTimestamp;
    }

    public String getExpiry() {
        return expiry;
    }

    public void setExpiry(String expiry) {
        this.expiry = expiry;
    }

    public String getLoginKey() {
        return loginKey;
    }

    public void setLoginKey(String loginKey) {
        this.loginKey = loginKey;
    }

    public int getLeagueId() {
        return leagueId;
    }

    public void setLeagueId(int leagueId) {
        this.leagueId = leagueId;
    }
}
