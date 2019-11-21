package com.example.capstoneui.API;

import com.example.capstoneui.Models.BasicRequest;
import com.example.capstoneui.Models.CallTimeout;
import com.example.capstoneui.Models.Game;
import com.example.capstoneui.Models.RecordFoul;
import com.example.capstoneui.Models.RecordScore;
import com.example.capstoneui.Models.SetGameClock;
import com.example.capstoneui.Models.SetShotClock;
import com.example.capstoneui.Models.SetTimeout;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface TeamInfoAPI {


    @GET("GetGame")
    Call<Game> GetFullGameData(@Query("apiToken") String APIToken
    );

    @GET("CheckConnection")
    Call<String> checkConnection(@Query("apiToken") BasicRequest APIToken
    );

    @POST("PlayHorn")
    Call<BasicRequest> playHorn(@Body BasicRequest requestHorn);

    @POST("StartTimer")
    Call<Integer> startTimer(@Body BasicRequest startTimerRequest);

    @POST("StopTimer")
    Call<Integer> stopTimer(@Body BasicRequest stopTimerRequest);

    @POST("SetGameClock")
    Call<String> setGameClock(@Body SetGameClock setGameClock);

    @POST("ResetShotClock")
    Call<String> resetShotClock(@Body BasicRequest resetShotClock);

    @POST("SetShotClock")
    Call<String> setShotClock(@Body SetShotClock setShotClock);

    @POST("RecordScore")
    Call<String> recordScore(@Body RecordScore recordScore);

    @POST("RecordFoul")
    Call<String> recordFoul(@Body RecordFoul recordFoul);

    @POST("CallTimeout")
    Call<String> callTimeout(@Body CallTimeout callTimeout);

    @POST("SetTimeouts")
    Call<String> setTimeout(@Body SetTimeout callTimeout);

}
