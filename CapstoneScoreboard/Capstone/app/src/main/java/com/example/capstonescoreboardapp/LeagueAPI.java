package com.example.capstonescoreboardapp;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface LeagueAPI {

    @GET("api/Login")
    Call<League> Login(@Query("leagueKey") String LeagueCode,
                      @Query("hashedPassword") String HashedPw
    );

}
