package com.example.capstoneui;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.text.InputFilter;
import android.text.InputType;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.example.capstoneui.Controller.UnsafeOkHttpClient;
import com.example.capstoneui.Controller.ViewControllerContainer;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

public class MainActivity extends AppCompatActivity {



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Gson gson = new GsonBuilder()
                .setLenient()
                .create();

        ViewControllerContainer.ViewController.retrofit = new Retrofit.Builder()
                .baseUrl("http://" + ViewControllerContainer.ViewController.ipAddress + "/api/MediaControl/")
                .client(ViewControllerContainer.ViewController.okHttpClient)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .build();
        ViewControllerContainer.ViewController.getAvailMedia();

        ViewControllerContainer.ViewController.retrofit = new Retrofit.Builder()
                .baseUrl("http://" + ViewControllerContainer.ViewController.ipAddress + "/api/Scoreboard/")
                .client(ViewControllerContainer.ViewController.okHttpClient)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .build();

        ViewControllerContainer.ViewController.point = false;//false for home
        ViewControllerContainer.ViewController.context=this;//set current context
        ViewControllerContainer.ViewController.timerRunning = false;//set timer is running
        ViewControllerContainer.ViewController.grabTeamScores();//update to current game scores

    }

    public void setScore(View view) {
        if (view.getTag().equals("team1"))
        {
            ViewControllerContainer.ViewController.point = true;
        }
        else
        {
            ViewControllerContainer.ViewController.point = false;
        }
        Intent intent = new Intent(this, ChooseScore.class);//new intent
        startActivity(intent);//gets into intent
    }

    public void setTimeOut(View view) {
        Intent intent = new Intent(this, SetTimeOut.class);//new intent
        startActivity(intent);//gets into intent

    }

    public void addTimeout(View view) {
        if (view.getTag().equals("team1"))
        {
            ViewControllerContainer.ViewController.point = true;
        }
        else
        {
            ViewControllerContainer.ViewController.point = false;
        }

        ViewControllerContainer.ViewController.addTimeOut();

    }

    public void setFoul(View view) {
        ViewControllerContainer.ViewController.scoring=10;
        if (view.getTag().equals("team1"))
        {
            ViewControllerContainer.ViewController.point = true;
        }
        else
        {
            ViewControllerContainer.ViewController.point = false;
        }
        Intent intent = new Intent(this, AddScore.class);//new intent
        startActivity(intent);//gets into intent

    }

    public void soundHorn(View view) {
        ViewControllerContainer.ViewController.playHorn();
    }

    public void toggleTime(View view) {
        if((ViewControllerContainer.ViewController.timerRunning))
        {
            ViewControllerContainer.ViewController.stopTimer();
        }
        else
        {
            ViewControllerContainer.ViewController.startTimer();
            ViewControllerContainer.ViewController.startTimer();
        }
    }

    public void resetShotClock(View view) {
        ViewControllerContainer.ViewController.resetShotClock();
    }

    private static Integer minutes;

    public void setTimer(View view) {

        AlertDialog.Builder alert = new AlertDialog.Builder(this);

        alert.setTitle("Set Game Time Minutes");

        // Set an EditText view to get user input
        final EditText input = new EditText(this);
        input.setFilters(new InputFilter[]{ new InputFilterMinMax("0", "59")});
        input.setInputType(InputType.TYPE_CLASS_NUMBER);
        alert.setView(input);

        alert.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                minutes = Integer.parseInt(input.getText().toString());
                AlertDialog.Builder alert2 = new AlertDialog.Builder(MainActivity.this);

                alert2.setTitle("Set Game Time Seconds");

                // Set an EditText view to get user input
                final EditText input2 = new EditText(MainActivity.this);
                input2.setFilters(new InputFilter[]{ new InputFilterMinMax("0", "59")});
                input2.setInputType(InputType.TYPE_CLASS_NUMBER);
                alert2.setView(input2);

                alert2.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int whichButton) {
                        Integer seconds = Integer.parseInt(input2.getText().toString());
                        ViewControllerContainer.ViewController.setTimer(minutes, seconds);
                    }
                });

                alert2.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int whichButton) {
                        // Canceled.
                    }
                });
                alert2.show();
            }
        });

        alert.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                // Canceled.
            }
        });
        alert.show();


    }

    public void setShockClock(View view) {
        AlertDialog.Builder alert = new AlertDialog.Builder(this);

        alert.setTitle("Set Shot Clock");

        // Set an EditText view to get user input
        final EditText input = new EditText(this);
        input.setFilters(new InputFilter[]{ new InputFilterMinMax("0", "24")});
        input.setInputType(InputType.TYPE_CLASS_NUMBER);
        alert.setView(input);

        alert.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                Integer seconds = Integer.parseInt(input.getText().toString());
                ViewControllerContainer.ViewController.setShotClock(seconds);
            }
        });

        alert.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                // Canceled.
            }
        });

        alert.show();
    }

    public void switchMedia(View view) {
        Intent intent = new Intent(this, ProducerScreen.class);//new intent
        startActivity(intent);//gets into intent
    }
}
