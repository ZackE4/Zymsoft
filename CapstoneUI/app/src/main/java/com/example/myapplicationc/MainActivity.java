package com.example.myapplicationc;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void addScore(View view) {
        Intent intent = new Intent(this, AddScore.class);
        startActivity(intent);
    }

    public void addFouls(View view) {
        Intent intent = new Intent(this, AddFouls.class);
        startActivity(intent);
    }
}
