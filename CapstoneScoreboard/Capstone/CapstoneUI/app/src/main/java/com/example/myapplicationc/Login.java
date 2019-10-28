package com.example.myapplicationc;

import java.sql.Time;

public class Login {
    private int LoginId;
    private Time LoginTimeStamp;
    private Time Expiry;
    private String LoginKey;
    private int LeagueLeagueId;


    public int getLoginId() {
        return LoginId;
    }

    public void setLoginId(int loginId) {
        LoginId = loginId;
    }

    public Time getLoginTimeStamp() {
        return LoginTimeStamp;
    }

    public void setLoginTimeStamp(Time loginTimeStamp) {
        LoginTimeStamp = loginTimeStamp;
    }

    public Time getExpiry() {
        return Expiry;
    }

    public void setExpiry(Time expiry) {
        Expiry = expiry;
    }

    public String getLoginKey() {
        return LoginKey;
    }

    public void setLoginKey(String loginKey) {
        LoginKey = loginKey;
    }

    public int getLeagueLeagueId() {
        return LeagueLeagueId;
    }

    public void setLeagueLeagueId(int leagueLeagueId) {
        LeagueLeagueId = leagueLeagueId;
    }
}
