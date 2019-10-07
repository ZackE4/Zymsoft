package com.example.myapplicationc;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

public class AddScore extends AppCompatActivity {


    RadioButton rd;
    TextView txt;
    String text = " ";
    private int score = 0;
    private String playerId = " ";


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_score);

        final RadioGroup team1 = findViewById(R.id.rdTeam1);
        final RadioGroup team2 = findViewById(R.id.rdTeam2);
        final RadioGroup addscore = findViewById(R.id.rdScoring);
        txt = (TextView) findViewById(R.id.txtScoreAway);



        team1.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                try{
                    RadioButton rb = (RadioButton) findViewById(checkedId);
                    playerId=rb.getTag().toString();

                    if (team2.getCheckedRadioButtonId() == -1)
                    {
                        //none are selected
                    }
                    else
                    {
                        team2.clearCheck();
                    }

                }
                catch (Exception ex)
                {

                }

            }
        });

        team2.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                try{
                    RadioButton rb = (RadioButton) findViewById(checkedId);
                    playerId=rb.getTag().toString();
                    if (team1.getCheckedRadioButtonId() == -1)
                    {

                    }
                    else
                    {
                        team1.clearCheck();
                    }
                }
                catch (Exception ex)
                {

                }

            }
        });


        addscore.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                try{
                    switch (checkedId){
                        case R.id.radioButton:
                                score=1;
                                break;
                        case R.id.radioButton2:
                                score = 2;
                                 break;
                        case R.id.radioButton3:
                                score=3;
                            break;
                        case R.id.radioButton4:
                                score=4;
                            break;
                        default:
                            score=0;
                            break;
                    }
                }
                catch (Exception ex)
                {

                }

            }
        });
    }

//
//    private void displayForTeamB(int score) {
//        TextView scoreView = (TextView) findViewById(R.id.txtScoreAway);
//        scoreView.setText(String.valueOf(score));
//    }




    public void saveClose(View view) {
        if(score == 0 || playerId == " ")
        {
            Toast.makeText(getApplicationContext(), "No score or Player selected ", Toast.LENGTH_SHORT).show();
            return;

        }

        Intent intent = new Intent();
        intent.putExtra("Score", score);
        intent.putExtra("PlayerId", playerId);
        setResult(RESULT_OK, intent);
        finish();
    }


}
