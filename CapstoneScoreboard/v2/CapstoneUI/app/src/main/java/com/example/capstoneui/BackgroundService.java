package com.example.capstoneui;

import android.app.IntentService;
import android.content.Intent;
import android.util.Log;

import com.example.capstoneui.API.TeamInfoAPI;
import com.example.capstoneui.Controller.ViewControllerContainer;

import java.io.IOException;

import androidx.annotation.Nullable;
import retrofit2.Call;

public class BackgroundService extends IntentService {

    public BackgroundService() {
        super("BackgroundService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        TeamInfoAPI teamInfo = ViewControllerContainer.ViewController.retrofit
                .create(TeamInfoAPI.class);
        Call<String> call = teamInfo.checkConnection(ViewControllerContainer.ViewController.apiKey);

        try {
            ViewControllerContainer.ViewController.connectionStat = call.execute().body();
        } catch (
                IOException e) {
            e.printStackTrace();
        }
    }
}
