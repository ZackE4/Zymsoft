package com.example.myapplicationc;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

public class AddFouls extends AppCompatActivity {


    private String playerId = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_fouls);

        final RadioGroup team1 = findViewById(R.id.rdTeam1);
        final RadioGroup team2 = findViewById(R.id.rdTeam2);
        final TextView txt = findViewById(R.id.textView15);


        team1.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                try{
                    RadioButton rb = (RadioButton) findViewById(checkedId);
                    playerId=rb.getTag().toString();
                    if (team2.getCheckedRadioButtonId() == -1)
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
                        team2.clearCheck();
                    }
                }
                catch (Exception ex)
                {

                }

            }
        });
    }

    public void saveClose(View view) {
        if(playerId == " ")
        {
            Toast.makeText(getApplicationContext(), "No Player selected ", Toast.LENGTH_SHORT).show();
            return;

        }

        Intent intent = new Intent();
        intent.putExtra("PlayerId", playerId);
        setResult(RESULT_OK, intent);
        finish();
    }
}
