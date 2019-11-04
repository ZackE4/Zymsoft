package com.example.capstonescoreboardapp;

import android.util.Log;
import android.view.View;
import android.widget.TextView;

import com.example.capstonescoreboardapp.databinding.ActivityMainBinding;

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
    private ActivityMainBinding binding;

    public ViewController(String apiToken, ActivityMainBinding binding) {
        this.apiToken = apiToken;
        //Retrofit allows the connection to post man/webapi
        retrofit = new Retrofit.Builder()
                .baseUrl("http://142.55.32.86:50291/")
                .addConverterFactory(GsonConverterFactory.create())
                .build();
        this.binding = binding;
        Log.e("API", "start" + apiToken);
    }

    public String createAPIToken() {
        //create our api with retrofit
        LeagueAPI login = retrofit
                .create(LeagueAPI.class);

        Call<League> call = login.Login("ABC123", "2AC9CB7DC02B3C0083EB70898E549B63");

        try{
            Response<League> response = call.execute();
            League apiResponse = response.body();
            apiToken = apiResponse.getLogin().getLoginKey();
        }
        catch (Exception ex)
        {
            ex.printStackTrace();
        }


//        call.enqueue(new Callback<League>() {
//            @Override
//            public void onResponse(Call<League> call, Response<League> response) {
//                if (!response.isSuccessful())
//                {
//                    Log.e("APICall", "Code1: "+ response.code());
//                    return;
//                }
//                League gets = response.body();
//                apiToken = gets.getLogin().getLoginKey();
//                Log.e("API", "abc " + apiToken);
//            }
//
//            @Override
//            public void onFailure(Call<League> call, Throwable t) {
//                Log.e("APICall", t.getMessage());
//            }
//        });
////        call.execute();
        Log.e("API", "ab q" + apiToken);
        return apiToken;
    }


    public void grabTeamScores() {
        //create our api with retrofit
        TeamInfoAPI teamInfo = retrofit
                .create(TeamInfoAPI.class);

        Call<Scoring> call = teamInfo.GetScoreOfGame(apiToken, 2);
        Log.e("API", " ??" + apiToken);
        call.enqueue(new Callback<Scoring>() {
            @Override
            public void onResponse(Call<Scoring> call, Response<Scoring> response) {
                if (!response.isSuccessful())
                {
                    Log.e("APICall", "Code2: "+ response.code());
                    return;
                }


                Scoring gets = response.body();
                gameScore = new Scoring(gets.getHomeTeamScore(), gets.getAwayTeamScore(), gets.getGameId());
                Log.e("API", " q" + gets.getAwayTeamScore());
                binding.txtTeam1Score.setText("Score : 1");
                Log.e("API", "z " + binding.txtTeam1Score.getText());
                binding.txtTeam2Score.setText("Score : " + gameScore.getAwayTeamScore());
            }

            @Override
            public void onFailure(Call<Scoring> call, Throwable t) {
                Log.e("APICall", t.getMessage());
            }
        });

    }
}
