package com.example.capstoneui.Controller;

import android.content.Context;
import android.util.Log;

import com.example.capstoneui.API.LeagueAPI;
import com.example.capstoneui.API.TeamInfoAPI;
import com.example.capstoneui.Models.Game;
import com.example.capstoneui.Models.League;
import com.example.capstoneui.Models.Scoring;
import com.example.capstoneui.Models.Teams;


import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ViewController{

    private String apiKey;
    private Retrofit retrofit;
    private OkHttpClient okHttpClient;
//    private Scoring gameScore;

    public ViewController(String apiKey, Context context) {
        this.apiKey = apiKey;
        okHttpClient = UnsafeOkHttpClient.getUnsafeOkHttpClient();
        //Retrofit allows the connection to post man/webapi
        retrofit = new Retrofit.Builder()
                .baseUrl("https://10.0.2.2:44352/api/Scoreboard/")
                .client(okHttpClient)
                .addConverterFactory(GsonConverterFactory.create())
                .build();
    }


    public void grabTeamScores() {
        //create our api with retrofit
        TeamInfoAPI teamInfo = retrofit
                .create(TeamInfoAPI.class);

        Call<Game> call = teamInfo.GetFullGameData(apiKey);
        Log.e("API", " " + apiKey);
        call.enqueue(new Callback<Game>() {
            @Override
            public void onResponse(Call<Game> call, Response<Game> response) {
                if (!response.isSuccessful())
                {
                    Log.e("APICall", "Code: "+ response.code());
                    return;
                }


                Game gets = response.body();
                Log.e("API", " " + gets.getGameScore().getAwayTeamScore());
//                //data binding instead
//                MainActivity binding =
//                        DataBindingUtil.setContentView(this, R.layout.activity_main);
//                binding.txt.setText("Hello World"); // you should use resources!
//
//                TextView team1Score = (TextView) MainActivity.class.g(R.id.txtTeam1Score);
//                TextView team2Score = (TextView) findViewById(R.id.txtTeam2Score);
//                team1Score.setText("Score : 1");
//                team2Score.setText("Score : " +gameScores.getAwayTeamScore());
            }

            @Override
            public void onFailure(Call<Game> call, Throwable t) {
                Log.e("APICall", t.getMessage());
            }
        });



    }

    public void grabTeamId()
    {
        //create our api with retrofit
        TeamInfoAPI teamInfo = retrofit
                .create(TeamInfoAPI.class);

        Log.e("API", " " + retrofit);
        Call<Teams> call = teamInfo.GetTeamById(apiKey, 1);
        Log.e("API", " " + apiKey);
        call.enqueue(new Callback<Teams>() {
            @Override
            public void onResponse(Call<Teams> call, Response<Teams> response) {
                if (!response.isSuccessful())
                {
                    Log.e("APICall", "Code: "+ response.code());
                    return;
                }


                Teams gets = response.body();
                Log.e("API", " " + gets.getTeamname());
//                //data binding instead
//                MainActivity binding =
//                        DataBindingUtil.setContentView(this, R.layout.activity_main);
//                binding.txt.setText("Hello World"); // you should use resources!
//
//                TextView team1Score = (TextView) MainActivity.class.g(R.id.txtTeam1Score);
//                TextView team2Score = (TextView) findViewById(R.id.txtTeam2Score);
//                team1Score.setText("Score : 1");
//                team2Score.setText("Score : " +gameScores.getAwayTeamScore());
            }

            @Override
            public void onFailure(Call<Teams> call, Throwable t) {
                Log.e("APICall", t.getMessage());
            }
        });



    }

//    public void checkConnection() {
        //create our api with retrofit
//        TeamInfoAPI teamInfo = retrofit
//                .create(TeamInfoAPI.class);
//
//        Call<Scoring> call = teamInfo.GetScoreOfGame(apiToken, 2);
//        Log.e("API", " " + apiToken);
//        call.enqueue(new Callback<Scoring>() {
//            @Override
//            public void onResponse(Call<Scoring> call, Response<Scoring> response) {
//                if (!response.isSuccessful())
//                {
//                    Log.e("APICall", "Code: "+ response.code());
//                    return;
//                }
//
//
//                Scoring gets = response.body();
//                gameScore = new Scoring(gets.getHomeTeamScore(), gets.getAwayTeamScore(), gets.getGameId());
////                //data binding instead
////                MainActivity binding =
////                        DataBindingUtil.setContentView(this, R.layout.activity_main);
////                binding.txt.setText("Hello World"); // you should use resources!
////
////                TextView team1Score = (TextView) MainActivity.class.g(R.id.txtTeam1Score);
////                TextView team2Score = (TextView) findViewById(R.id.txtTeam2Score);
////                team1Score.setText("Score : 1");
////                team2Score.setText("Score : " +gameScores.getAwayTeamScore());
//            }
//
//            @Override
//            public void onFailure(Call<Scoring> call, Throwable t) {
//                Log.e("APICall", t.getMessage());
//            }
//        });
//    }
}
