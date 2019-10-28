//package com.example.myapplicationc;
//
//
//import android.util.Log;
//
//import com.google.gson.Gson;
//import com.google.gson.GsonBuilder;
//
//import java.util.List;
//
//import retrofit2.Call;
//import retrofit2.Callback;
//import retrofit2.Response;
//import retrofit2.Retrofit;
//import retrofit2.converter.gson.GsonConverterFactory;
//
//public class Controller implements Callback<List<Teams>> {
//
//    static final String BASE_URL = "http://142.55.32.86:50291/";
//
////    public void start() {
////        Gson gson = new GsonBuilder()
////                .setLenient()
////                .create();
////
////        Retrofit retrofit = new Retrofit.Builder()
////                .baseUrl(BASE_URL)
////                .addConverterFactory(GsonConverterFactory.create(gson))
////                .build();
////
////        TeamInfoAPI api = retrofit.create(TeamInfoAPI.class);
////
////        Call<List<Teams>> call = api.GetTeamById();
////        call.enqueue(this);
////
////    }
////
//    @Override
//    public void onResponse(Call<List<Teams>> call, Response<List<Teams>> response) {
//        if(response.isSuccessful()) {
//            List<Teams> teams = response.body();
//
//            for (Teams get: teams)
//            {
//                String content="";
////                content+= "Id " + get.getTeamId();
//
//                Log.e("Team info ", content);
//            }
//        } else {
//            System.out.println(response.errorBody());
//        }
//    }
////
////    @Override
////    public void onFailure(Call<List<Teams>> call, Throwable t) {
////        t.printStackTrace();
////    }
//
//
//}
