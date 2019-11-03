package com.example.capstoneui.API;

import com.example.capstoneui.Models.FoulLog;
import com.example.capstoneui.Models.Fouls;
import com.example.capstoneui.Models.Game;
import com.example.capstoneui.Models.Scoring;
import com.example.capstoneui.Models.ScoringLog;
import com.example.capstoneui.Models.Teams;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface TeamInfoAPI {


//    @GET("GetTeam")
//    Call<Teams> GetTeamById(@Query("apiToken") String APIToken,
//                            @Query("id") int TeamId
//    );

    @GET("GetGame")
    Call<Game> GetFullGameData(@Query("apiToken") String APIToken
    );

//    @POST("StartTimer")
//    Call<>


    @GET("Scoring/Fouls")
    Call<Fouls> GetFoulsOfGame(
            @Query("apiToken") String APIToken,
            @Query("gameId") int gameId
    );

    @POST("Scoring/RecordScore")
    Call<ScoringLog> RecordScore(@Body ScoringLog scoringLog);

    @POST("Scoring/RecordFoul")
    Call<FoulLog> RecordFoul(@Body FoulLog foulLog);





}
