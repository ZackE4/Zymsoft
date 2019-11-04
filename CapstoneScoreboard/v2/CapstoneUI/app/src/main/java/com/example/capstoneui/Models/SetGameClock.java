package com.example.capstoneui.Models;

public class SetGameClock extends BasicRequest {
    private Integer minutes;
    private Integer seconds;

    public Integer getMinutes() {
        return minutes;
    }

    public void setMinutes(Integer minutes) {
        this.minutes = minutes;
    }

    public Integer getSeconds() {
        return seconds;
    }

    public void setSeconds(Integer seconds) {
        this.seconds = seconds;
    }
}
