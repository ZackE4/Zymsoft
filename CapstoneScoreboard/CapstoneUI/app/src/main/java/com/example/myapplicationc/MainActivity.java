package com.example.myapplicationc;

import android.content.Intent;
import android.media.MediaPlayer;
import android.os.CountDownTimer;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;


import java.sql.Time;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class MainActivity extends AppCompatActivity {

    private static final long START_TIME_IN_MILLIS = 3600000;
     MediaPlayer horn;

    private TextView mtextView;
    private Button mstart;
    private Button mreset;
    private Button mstop;
    private CountDownTimer mcountTimer;
    private  boolean mTimerRunning;
    private long mtimeLeftInMills = START_TIME_IN_MILLIS;
    int minteger = 0;
    int binterger = 0;
    private int foulsAway = 0;
    private int foulsHome = 0;
    private int scoreHome = 0;
    private int scoreAway = 0;
    private String APIToken = "5f0542d0-2fac-4089-9332-19c4faa74dea";



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Retrofit allows the connection to post man/webapi
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl("http://142.55.32.86:50291/")
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        //create our api with retrofit
        TeamInfoAPI teamInfo = retrofit
                .create(TeamInfoAPI.class);


        Call<Scoring> call = teamInfo.GetScoreOfGame(APIToken, 3);

        call.enqueue(new Callback<Scoring>() {
            @Override
            public void onResponse(Call<Scoring> call, Response<Scoring> response) {
                if (!response.isSuccessful())
                {
                    Log.e("APICall", "Code: "+ response.code());
                    return;
                }

                Scoring gets = response.body();

                TextView homeScore = (TextView) findViewById(R.id.txtScoreHome);
                homeScore.setText(" " + gets.getHomeTeamScore());
                scoreHome = gets.getHomeTeamScore();
                TextView awayScore = (TextView) findViewById(R.id.txtScoreAway);
                awayScore.setText(" " + gets.getAwayTeamScore());
                scoreAway = gets.getAwayTeamScore();
//                    String content="";
//                    content+="Id:  "+gets.getGameId() + " score1  " + gets.getAwayTeamScore() + " score2 " + gets.getHomeTeamScore();
//                    content+= "Id: " + gets.getTeamId();
//                    content+= " Name:  " + gets.getTeamname();
//                    content+= " Coach:  " + gets.getCoachName();
//                    content+= " Logo:  " + gets.getLogo();
//                    content+= " League:  " + gets.getLeagueId();
//                    Log.e("APICall", content);
            }

            @Override
            public void onFailure(Call<Scoring> call, Throwable t) {
                Log.e("APICall", t.getMessage());
            }
        });


        //Retrofit allows the connection to post man/webapi
        Retrofit re = new Retrofit.Builder()
                .baseUrl("http://142.55.32.86:50291/")
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        //create our api with retrofit
        TeamInfoAPI teamI = retrofit
                .create(TeamInfoAPI.class);


        Call<Fouls> calls = teamInfo.GetFoulsOfGame(APIToken, 3);

        calls.enqueue(new Callback<Fouls>() {
            @Override
            public void onResponse(Call<Fouls> call, Response<Fouls> response) {
                if (!response.isSuccessful())
                {
                    Log.e("APICall", "Code: "+ response.code());
                    return;
                }

                Fouls gets = response.body();

                TextView homeFouls = (TextView) findViewById(R.id.txtFoulsHome);
                homeFouls.setText(" " + gets.getHomeTeamFouls());
                foulsHome = gets.getHomeTeamFouls();
                TextView awayFouls = (TextView) findViewById(R.id.txtFoulsAway);
                awayFouls.setText(" " + gets.getAwayTeamFouls());
                foulsAway = gets.getAwayTeamFouls();
//                    String content="";
//                    content+="Id:  "+gets.getGameId() + " score1  " + gets.getAwayTeamScore() + " score2 " + gets.getHomeTeamScore();
//                    content+= "Id: " + gets.getTeamId();
//                    content+= " Name:  " + gets.getTeamname();
//                    content+= " Coach:  " + gets.getCoachName();
//                    content+= " Logo:  " + gets.getLogo();
//                    content+= " League:  " + gets.getLeagueId();
//                    Log.e("APICall", content);
            }

            @Override
            public void onFailure(Call<Fouls> calls, Throwable t) {
                Log.e("APICall", t.getMessage());
            }
        });


                mtextView = findViewById(R.id.txtTimer);
                mstart =findViewById(R.id.btnStart);
                mstop = findViewById(R.id.btnstop);

                mreset = findViewById(R.id.btnreset);
                horn = MediaPlayer.create(MainActivity.this , R.raw.horn);

                mstart.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if(mTimerRunning){
                            pauseTimer();
                        }
                        else{
                            startTimer();
                        }

                     }
                });


        mreset.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                resetTimer();

            }
        });

        updateCountDownText();

    }





    private void startTimer(){
        mcountTimer = new CountDownTimer(mtimeLeftInMills,1000) {
            @Override
            public void onTick(long millisUntilFinished) {
            mtimeLeftInMills= millisUntilFinished;
            updateCountDownText();
            }


            @Override
            public void onFinish() {
                mTimerRunning = false;
                mstart.setText("Start");
                mstart.setVisibility(View.INVISIBLE);
                mstop.setVisibility(View.INVISIBLE);
                mreset.setVisibility(View.VISIBLE);
                 }
            }.start();

        mTimerRunning = true;
        mstart.setText("Stop");
        mreset.setVisibility(View.INVISIBLE);
        mstop.setVisibility(View.INVISIBLE);


    }
    private void resetTimer(){
        mtimeLeftInMills = START_TIME_IN_MILLIS;
        updateCountDownText();
        mreset.setVisibility(View.INVISIBLE);
        mstart.setVisibility(View.VISIBLE);
        mstop.setVisibility(View.VISIBLE);
        mreset.setVisibility(View.VISIBLE);

    }
    private void pauseTimer(){
        mcountTimer.cancel();
        mTimerRunning = false;
        mstart.setText("Start");
        mreset.setVisibility(View.VISIBLE);


    }
    private void updateCountDownText(){
        int minutes = (int) (mtimeLeftInMills/1000)/60;
        int seconds = (int) (mtimeLeftInMills/1000)%60;


        String timeLeftFormated  = String.format(Locale.getDefault(),"%02d:%02d", minutes,seconds);
   mtextView.setText(timeLeftFormated);

    }

    public void addScore(View view) {
        Intent intent = new Intent(this, AddScore.class);
        startActivityForResult(intent, 1);
    }

    public void addFouls(View view) {
        Intent intent = new Intent(this, AddFouls.class);
        startActivityForResult(intent, 1);
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 1) {
            if(resultCode == RESULT_OK) {
                int score = data.getIntExtra("Score", 0);
                String team= data.getStringExtra("PlayerId");
                int playerId = Integer.parseInt(team);
                if (score == 0)
                {
                    addFouling(playerId);
                }
                else
                {
                    addScoring(playerId, score);
                }
            }
        }
    }

    private void addScoring(int playerId, int score) {
        String currentTime = new SimpleDateFormat("HH:mm:ss", Locale.getDefault()).format(new Date());
        final ScoringLog scoringLog = new ScoringLog(APIToken,"ABC123", currentTime, score, playerId, 3);
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl("http://142.55.32.86:50291/")
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        //create our api with retrofit
        TeamInfoAPI teamInfo = retrofit
                .create(TeamInfoAPI.class);

        Call<ScoringLog> call = teamInfo.RecordScore(scoringLog);

        call.enqueue(new Callback<ScoringLog>() {
            @Override
            public void onResponse(Call<ScoringLog> call, Response<ScoringLog> response) {
                if(!response.isSuccessful())
                {
                    Log.e("ScoreError", response.code() + " not working");
                    return;
                }
                ScoringLog scoreResponse = response.body();

                Log.e("ScoreError", scoreResponse.getGameId() + " " + scoreResponse.getScoringLogId());

                if (scoringLog.getPlayerId() == 4)
                {
                    scoreHome+=scoreResponse.getPoints();
                    TextView txtScoreHome = (TextView) findViewById(R.id.txtScoreHome);
                    txtScoreHome.setText(" "  + scoreHome);

                }
                else
                {
                    scoreAway+=scoreResponse.getPoints();
                    TextView txtScoreAway = (TextView) findViewById(R.id.txtScoreAway);
                    txtScoreAway.setText(" "  + scoreAway);
                }
            }

            @Override
            public void onFailure(Call<ScoringLog> call, Throwable t) {

            }
        });


    }

    private void addFouling(int playerId) {
        String currentTime = new SimpleDateFormat("HH:mm:ss", Locale.getDefault()).format(new Date());
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl("http://142.55.32.86:50291/")
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        //create our api with retrofit
        TeamInfoAPI teamInfo = retrofit
                .create(TeamInfoAPI.class);


        FoulLog foulLog = new FoulLog(APIToken, "ABC123", currentTime, playerId, 3);
        Call<FoulLog> call = teamInfo.RecordFoul(foulLog);
        Log.e("ScoreError", currentTime + " not working");
        call.enqueue(new Callback<FoulLog>() {
            @Override
            public void onResponse(Call<FoulLog> call, Response<FoulLog> response) {
                if(!response.isSuccessful())
                {
                    Log.e("ScoreError", response.code() + " not working");
                    return;
                }
                FoulLog foulLog = response.body();

                Log.e("ScoreError", foulLog.getGameId() + " " + foulLog.getGameTime());
                if (foulLog.getPlayerId() == 4)
                {
                    foulsHome+=1;
                    TextView txtFoul = (TextView) findViewById(R.id.txtFoulsHome);
                    txtFoul.setText(" " + foulsHome);

                }
                else
                {
                    foulsAway+=1;
                    TextView txtFou2 = (TextView) findViewById(R.id.txtFoulsAway);
                    txtFou2.setText(" " + foulsAway);
                }



            }

            @Override
            public void onFailure(Call<FoulLog> call, Throwable t) {

            }
        });




    }

    public void onminus(View view) {
        minteger = minteger-1;
        display(minteger);



    }
    public void onClickAdd(View view){
        minteger = minteger +1;
        display(minteger);

    }
    public void onminusB(View view) {
        binterger = binterger-1;
        displayB(binterger);



    }
    public void onClickAddB(View view){
        binterger = binterger +1;
        displayB(binterger);

    }

    public void onClickPlayHorn(View view){
        horn.start();

    }
    private void display(int number) {
        TextView displayInteger = (TextView) findViewById(
                R.id.textView15);
        displayInteger.setText("" + number);
    }
    private void displayB(int number) {

        TextView displayInter = (TextView)  findViewById(R.id.textView16);
        displayInter.setText("" + number);
    }


}
