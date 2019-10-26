package com.example.myapplicationc;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface LoginAPI {

    @GET("api")
    Call<List<Teams>> Login(@Query("apiToken") String APIToken,
                                  @Query("id") int TeamId
    );


}
