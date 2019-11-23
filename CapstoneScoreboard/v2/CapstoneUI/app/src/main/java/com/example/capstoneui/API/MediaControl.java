package com.example.capstoneui.API;


import com.example.capstoneui.Models.AvailableMedia;
import com.example.capstoneui.Models.BasicRequest;
import com.example.capstoneui.Models.PlayMedia;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface MediaControl {

    @GET("AvailableMedia")
    Call<AvailableMedia> getAvailableMedia(@Query("apiToken") String APIToken
    );

    @POST("PlayVideo")
    Call<String> playVideo(@Body PlayMedia setMedia);


    @POST("ShowImage")
    Call<String> showImage(@Body PlayMedia setMedia);

    @POST("ShowMediaPage")
    Call<String> showMedia(@Body BasicRequest setMedia);

    @POST("ShowScoreboardPage")
    Call<String> showScoreboard(@Body BasicRequest setMedia);


}
