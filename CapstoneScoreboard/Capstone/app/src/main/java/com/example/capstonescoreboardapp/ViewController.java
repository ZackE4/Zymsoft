package com.example.capstonescoreboardapp;

import android.util.Log;
import android.view.View;
import android.widget.TextView;

import androidx.databinding.DataBindingUtil;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ViewController {

    private String apiToken;
    private Retrofit retrofit;
    private Scoring gameScore;

    public ViewController(String apiToken) {
        this.apiToken = apiToken;
        //Retrofit allows the connection to post man/webapi
        retrofit = new Retrofit.Builder()
                .baseUrl("http://142.55.32.86:50291/")
                .addConverterFactory(GsonConverterFactory.create())
                .build();

    }

    public String createAPIToken() {
        //create our api with retrofit
        LeagueAPI login = retrofit
                .create(LeagueAPI.class);

        Call<League> call = login.Login("ABC123", "2AC9CB7DC02B3C0083EB70898E549B63");

        call.enqueue(new Callback<League>() {
            @Override
            public void onResponse(Call<League> call, Response<League> response) {
                if (!response.isSuccessful())
                {
                    Log.e("APICall", "Code: "+ response.code());
                    return;
                }
                League gets = response.body();
                apiToken = gets.getLogin().getLoginKey();
            }

            @Override
            public void onFailure(Call<League> call, Throwable t) {
                Log.e("APICall", t.getMessage());
            }
        });
        Log.e("API", "ab" + apiToken);
        return apiToken;
    }


    public void grabTeamScores() {
        //create our api with retrofit
        TeamInfoAPI teamInfo = retrofit
                .create(TeamInfoAPI.class);

        Call<Scoring> call = teamInfo.GetScoreOfGame(apiToken, 2);
        Log.e("API", " " + apiToken);
        call.enqueue(new Callback<Scoring>() {
            @Override
            public void onResponse(Call<Scoring> call, Response<Scoring> response) {
                if (!response.isSuccessful())
                {
                    Log.e("APICall", "Code: "+ response.code());
                    return;
                }


                Scoring gets = response.body();
                gameScore = new Scoring(gets.getHomeTeamScore(), gets.getAwayTeamScore(), gets.getGameId());
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
            public void onFailure(Call<Scoring> call, Throwable t) {
                Log.e("APICall", t.getMessage());
            }
        });

    }
}
