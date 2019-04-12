package com.example.myapplicationc;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.RadioGroup;

public class AddFouls extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_fouls);

        final RadioGroup team1 = findViewById(R.id.rdTeam1);
        final RadioGroup team2 = findViewById(R.id.rdTeam2);


        team1.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                try{
                    if (team2.getCheckedRadioButtonId() == -1)
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

        team2.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                try{
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
    }

    public void saveClose(View view) {
        finish();
    }
}
