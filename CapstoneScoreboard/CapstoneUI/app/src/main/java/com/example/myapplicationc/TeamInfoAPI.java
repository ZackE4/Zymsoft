package com.example.myapplicationc;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface TeamInfoAPI {


    @GET("api/Teams")
    Call<Teams> GetTeamById(@Query("apiToken") String APIToken,
                                  @Query("id") int TeamId
    );


    @GET("api/Scoring/Score")
    Call<Scoring> GetScoreOfGame(@Query("apiToken") String APIToken,
                            @Query("gameId") int gameId
    );

    @GET("api/Scoring/Fouls")
    Call<Fouls> GetFoulsOfGame(
            @Query("apiToken") String APIToken,
            @Query("gameId") int gameId
    );

    @POST("api/Scoring/RecordScore")
    Call<ScoringLog> RecordScore(@Body ScoringLog scoringLog);

    @POST("api/Scoring/RecordFoul")
    Call<FoulLog> RecordFoul(@Body FoulLog foulLog);

}
