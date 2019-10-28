package com.example.capstonescoreboardapp;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;


public class MainActivity extends AppCompatActivity {


    private String APIToken = " ";//temp
    private ViewController controller;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        controller = new ViewController(APIToken);
        APIToken = controller.createAPIToken();
        controller.grabTeamScores();

    }


    public void setTime(View view) {


    }

    public void setScore(View view) {


    }

    public void setTimeOut(View view) {


    }

    public void setFoul(View view) {


    }

    public void soundHorn(View view) {

    }
}
