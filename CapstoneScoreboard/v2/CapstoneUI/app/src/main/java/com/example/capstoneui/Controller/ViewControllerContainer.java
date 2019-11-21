package com.example.capstoneui.Controller;

import android.app.Activity;
import android.util.Log;
import android.widget.TextView;

import com.example.capstoneui.API.TeamInfoAPI;
import com.example.capstoneui.Models.BasicRequest;
import com.example.capstoneui.Models.CallTimeout;
import com.example.capstoneui.Models.Game;
import com.example.capstoneui.Models.RecordFoul;
import com.example.capstoneui.Models.RecordScore;
import com.example.capstoneui.Models.SetGameClock;
import com.example.capstoneui.Models.SetShotClock;
import com.example.capstoneui.Models.SetTimeout;
import com.example.capstoneui.R;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class ViewControllerContainer {
    public static class ViewController {

        public static String apiKey;
        public static Retrofit retrofit;
        public static OkHttpClient okHttpClient;
        public static Activity context;

        public static Game currentGame;
        private static TextView score1;
        private static TextView score2;
        private static TextView fouls1;
        private static TextView fouls2;
        private static TextView timeouts1;
        private static TextView timeouts2;

        public static boolean point;//which team false home
        public static boolean timerRunning;
        public static Integer period;
        public static Integer scoring;//amount of points
        public static Integer playerId;

        public static String connectionStat;


        public static void playHorn() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            BasicRequest requestHorn = new BasicRequest();
            requestHorn.apiToken = apiKey;
            Call<BasicRequest> call = teamInfo.playHorn(requestHorn);
            Log.e("API", " " + apiKey);
            call.enqueue(new Callback<BasicRequest>() {
                @Override
                public void onResponse(Call<BasicRequest> call, Response<BasicRequest> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }
                    BasicRequest gets = response.body();
                    Log.e("API", " " + gets.apiToken);
                }

                @Override
                public void onFailure(Call<BasicRequest> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });
        }

        public static void grabTeamScores() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);


            Call<Game> call = teamInfo.GetFullGameData(apiKey);
            Log.e("API", " " + apiKey);
            call.enqueue(new Callback<Game>() {
                @Override
                public void onResponse(Call<Game> call, Response<Game> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code());
                        return;
                    }
                    currentGame = response.body();
                    String time = currentGame.getGameScore().getGameTime();
                    String[] times = time.split(":");
                    Log.e("api ", " " + times[0] +" a" + times[1] + " b" + times[2]);
                    period = Integer.parseInt(times[1]) / 12 + 1;
                    Log.e("API", ""+period);
                    if (period > 4)
                    {
                        period=4;
                    }
                    fillBoard();

                }

                @Override
                public void onFailure(Call<Game> call, Throwable t) {
                    Log.e("APICall", t.getMessage());
                }
            });
        }

        public static void startTimer() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);


            BasicRequest startTimer = new BasicRequest();
            startTimer.apiToken = apiKey;
            Call<Integer> call = teamInfo.startTimer(startTimer);
            Log.e("API", " " + apiKey);
            call.enqueue(new Callback<Integer>() {
                @Override
                public void onResponse(Call<Integer> call, Response<Integer> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code());
                        return;
                    }

                    period = response.body();
                    Log.e("APICall", " " + period);
                }

                @Override
                public void onFailure(Call<Integer> call, Throwable t) {
                    Log.e("APICall", t.getMessage());
                }
            });
        }

        public static void stopTimer() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);


            BasicRequest startTimer = new BasicRequest();
            startTimer.apiToken = apiKey;
            Call<Integer> call = teamInfo.stopTimer(startTimer);
            Log.e("API", " " + apiKey);
            call.enqueue(new Callback<Integer>() {
                @Override
                public void onResponse(Call<Integer> call, Response<Integer> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code());
                        return;
                    }

                    period = response.body();
                    Log.e("APICall", " " + period);
                }

                @Override
                public void onFailure(Call<Integer> call, Throwable t) {
                    Log.e("APICall", t.getMessage());
                }
            });
        }

        public static void updateScore(){
            score1.setText("Home Score: " + currentGame.getGameScore().getHomeTeamScore());
            score2.setText("Away Score: " + currentGame.getGameScore().getAwayTeamScore());
        }

        public static void fillBoard() {
            score1 = (TextView) context.findViewById(R.id.txtTeam1Score);
            score1.setText("Home Score: " + currentGame.getGameScore().getHomeTeamScore());
            score2 = (TextView) context.findViewById(R.id.txtTeam2Score);
            score2.setText("Away Score: " + currentGame.getGameScore().getAwayTeamScore());
            fouls1 = (TextView) context.findViewById(R.id.txtTeam1Fouls);
            int foulssum = 0;
            foulssum = currentGame.getGameScore().getHomeTeamFouls().get(0) + currentGame.getGameScore().getHomeTeamFouls().get(1)
                    + currentGame.getGameScore().getHomeTeamFouls().get(2) + currentGame.getGameScore().getHomeTeamFouls().get(3);
            fouls1.setText("Home Fouls: " + foulssum);
            fouls2 = (TextView) context.findViewById(R.id.txtTeam2Fouls);
            foulssum = currentGame.getGameScore().getAwayTeamFouls().get(0) + currentGame.getGameScore().getAwayTeamFouls().get(1)
                    + currentGame.getGameScore().getAwayTeamFouls().get(2) + currentGame.getGameScore().getAwayTeamFouls().get(3);
            fouls2.setText("Away Fouls: " + foulssum);
            timeouts1 = (TextView) context.findViewById(R.id.txtTeam1TimeOuts);
            timeouts1.setText("" + currentGame.getGameScore().getHomeTeamTimeoutsRemaining());
            timeouts2 = (TextView) context.findViewById(R.id.txtTeam2TimeOuts);
            timeouts2.setText("" + currentGame.getGameScore().getAwayTeamTimeoutsRemaining());

            TextView team1 = (TextView) context.findViewById(R.id.txtTeam1);
            team1.setText(currentGame.getHomeTeam().getTeamName());

            TextView team2 = (TextView) context.findViewById(R.id.txtTeam2);
            team2.setText(currentGame.getAwayTeam().getTeamName());
        }

        public static void recordScore() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            RecordScore recordScore = new RecordScore();
            recordScore.apiToken = apiKey;
            recordScore.setPoints(scoring);
            if(point)
            {
                recordScore.setSide("Home");
                currentGame.getGameScore().setHomeTeamScore(currentGame.getGameScore().getHomeTeamScore() + scoring);
            }
            else
            {
                recordScore.setSide("Away");
                currentGame.getGameScore().setAwayTeamScore(currentGame.getGameScore().getAwayTeamScore() + scoring);
            }
            recordScore.setPlayerId(playerId);
            Call<String> call = teamInfo.recordScore(recordScore);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }
                    String info = response.body();
                    Log.e("API", info);

                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });

        }

        public static void updateFoul()
        {
            int foulssum = 0;
            foulssum = currentGame.getGameScore().getHomeTeamFouls().get(0) + currentGame.getGameScore().getHomeTeamFouls().get(1)
                    + currentGame.getGameScore().getHomeTeamFouls().get(2) + currentGame.getGameScore().getHomeTeamFouls().get(3);
            fouls1.setText("Home Fouls: " + foulssum);
            foulssum = currentGame.getGameScore().getAwayTeamFouls().get(0) + currentGame.getGameScore().getAwayTeamFouls().get(1)
                    + currentGame.getGameScore().getAwayTeamFouls().get(2) + currentGame.getGameScore().getAwayTeamFouls().get(3);
            fouls2.setText("Away Fouls: " + foulssum);
        }

        public static void recordFoul() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            RecordFoul recordFoul = new RecordFoul();
            recordFoul.apiToken = apiKey;
            if(point)
            {
                recordFoul.setSide("Home");
                List<Integer> homeFouls = currentGame.getGameScore().getHomeTeamFouls();
                homeFouls.set(period-1, homeFouls.get(period-1) +1);
                Log.e("API", ""+homeFouls.get(period-1));
                currentGame.getGameScore().setHomeTeamFouls(homeFouls);
            }
            else
            {
                recordFoul.setSide("Away");
                List<Integer> awayFouls = currentGame.getGameScore().getAwayTeamFouls();
                awayFouls.set(period-1, awayFouls.get(period-1) +1);
                Log.e("API", ""+awayFouls.get(period-1));
                currentGame.getGameScore().setAwayTeamFouls(awayFouls);
            }
            recordFoul.setPlayerId(playerId);
            recordFoul.setPeriod(period);
            Call<String> call = teamInfo.recordFoul(recordFoul);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }

                    String info = response.body();
                    Log.e("API", info);

                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });

        }


        public static void setShotClock(Integer seconds) {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            SetShotClock shockClock = new SetShotClock();
            shockClock.apiToken = apiKey;
            shockClock.setValue(seconds);
            Call<String> call = teamInfo.setShotClock(shockClock);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }

                    String info = response.body();
                    Log.e("API", info);

                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });


        }

        public static void resetShotClock() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            BasicRequest shockClock = new BasicRequest();
            shockClock.apiToken = apiKey;
            Call<String> call = teamInfo.resetShotClock(shockClock);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }

                    String info = response.body();
                    Log.e("API", info);

                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });
        }

        public static void setTimer(Integer minutes, Integer seconds) {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            SetGameClock clock = new SetGameClock();

            clock.apiToken = apiKey;
            clock.setMinutes(minutes);
            clock.setSeconds(seconds);
            Call<String> call = teamInfo.setGameClock(clock);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }
                    String info = response.body();
                    Log.e("API", info);

                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });
        }

        public static void setTimeOuts(Integer timeouts) {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);
            SetTimeout setTimeout = new SetTimeout();
            setTimeout.setTimeouts(timeouts);//int

            if(point)//fix this later adding point
            {
                setTimeout.setSide("Home");
                currentGame.getGameScore().setHomeTeamTimeoutsRemaining(timeouts);
            }
            else
            {
                setTimeout.setSide("Away");
                currentGame.getGameScore().setAwayTeamTimeoutsRemaining(timeouts);
            }
            setTimeout.apiToken = apiKey;
            Call<String> call = teamInfo.setTimeout(setTimeout);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }

                    String info = response.body();
                    Log.e("API", info);
                    updateTimeouts();
                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });
        }

        public static void addTimeOut() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            CallTimeout callTimeout = new CallTimeout();


            if (point)
            {
                currentGame.getGameScore().setHomeTeamTimeoutsRemaining(currentGame.getGameScore().getHomeTeamTimeoutsRemaining()-1);
                timeouts1.setText("" + currentGame.getGameScore().getHomeTeamTimeoutsRemaining());
                callTimeout.setSide("Home");
            }
            else
            {
                currentGame.getGameScore().setAwayTeamTimeoutsRemaining(currentGame.getGameScore().getAwayTeamTimeoutsRemaining()-1);
                timeouts2.setText("" + currentGame.getGameScore().getAwayTeamTimeoutsRemaining());
                callTimeout.setSide("Away");
            }

            callTimeout.apiToken = apiKey;


            Call<String> call = teamInfo.callTimeout(callTimeout);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }

                    String info = response.body();
                    Log.e("API", info);
                    updateTimeouts();
                }



                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });

        }

        public static void updateTimeouts() {
            timeouts1.setText("" + currentGame.getGameScore().getHomeTeamTimeoutsRemaining());
            timeouts2.setText("" + currentGame.getGameScore().getAwayTeamTimeoutsRemaining());
        }

        public static void checkConnection() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            BasicRequest checkConn = new BasicRequest();
            checkConn.apiToken = apiKey;
            Call<String> call = teamInfo.checkConnection(checkConn);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }
                    connectionStat = response.body();
                }
                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });
        }

        public static void getAvailMedia() {
            //create our api with retrofit
            TeamInfoAPI teamInfo = retrofit
                    .create(TeamInfoAPI.class);

            BasicRequest requestHorn = new BasicRequest();
            requestHorn.apiToken = apiKey;
            Call<BasicRequest> call = teamInfo.playHorn(requestHorn);
            Log.e("API", " " + apiKey);
            call.enqueue(new Callback<BasicRequest>() {
                @Override
                public void onResponse(Call<BasicRequest> call, Response<BasicRequest> response) {
                    if (!response.isSuccessful()) {
                        Log.e("APICall", "Code: " + response.code() + " Message: " + response.errorBody());
                        return;
                    }
                    BasicRequest gets = response.body();
                    Log.e("API", " " + gets.apiToken);
                }

                @Override
                public void onFailure(Call<BasicRequest> call, Throwable t) {
                    Log.e("APICall2", t.getMessage());
                }
            });
        }




    }
}
