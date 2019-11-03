package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.example.capstoneui.Controller.ViewController;

import java.util.Locale;

public class MainActivity extends AppCompatActivity {


    private String apiKey = "168de16b";
    private CountDownTimer mcountTimer;
    private  boolean mTimerRunning;
    private static final long START_TIME_IN_MILLIS = 3600000;
    private long mtimeLeftInMills = START_TIME_IN_MILLIS;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mTimerRunning = false;//sets timer to be stopped
        TextView timer = (TextView) findViewById(R.id.txtTimer);//grabs timer
        updateCountDownText(timer);//sets time on start of app
        timer.setOnLongClickListener(new View.OnLongClickListener(){

            @Override
            public boolean onLongClick(View v) {
                mtimeLeftInMills = START_TIME_IN_MILLIS;
                mcountTimer.cancel();
                return false;
            }
        });


        ViewController viewController = new ViewController(apiKey, this);
        viewController.grabTeamScores();


    }

    public void timer(){
        mcountTimer = new CountDownTimer(mtimeLeftInMills,1000) {
            @Override
            public void onTick(long millisUntilFinished) {
                TextView timer = (TextView) findViewById(R.id.txtTimer);
                mtimeLeftInMills=millisUntilFinished;
                updateCountDownText(timer);
            }

            @Override
            public void onFinish() {
                mTimerRunning = false;
            }
        }.start();

    }

    private void updateCountDownText(TextView timer) {
        int minutes = (int) (mtimeLeftInMills/1000)/60;
        int seconds = (int) (mtimeLeftInMills/1000)%60;
        String timeLeftFormated  = String.format(Locale.getDefault(),"%02d:%02d", minutes,seconds);
        timer.setText(" " + timeLeftFormated);
    }


    public void setTime(View view) {//switch on click
        if (mTimerRunning)
        {
            mTimerRunning = false;
            mcountTimer.cancel();
        }
        else
        {
            mTimerRunning = true;
            timer();
        }


    }

    public void setScore(View view) {
        Intent intent = new Intent(this, AddScore.class);//new intent

        startActivity(intent);//gets into intent
    }

    public void setTimeOut(View view) {


    }

    public void setFoul(View view) {


    }

    public void soundHorn(View view) {

    }

}
