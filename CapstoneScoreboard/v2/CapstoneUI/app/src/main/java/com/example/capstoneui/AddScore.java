package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import com.example.capstoneui.Controller.ViewControllerContainer;
import com.example.capstoneui.Models.AwayTeamRoster;
import com.example.capstoneui.Models.HomeTeamRoster;

import java.util.ArrayList;
import java.util.List;

public class AddScore extends AppCompatActivity {

    private ListView lv;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_score);
        lv = (ListView) findViewById(R.id.listViewAddSc);
        List<String> your_array_list = new ArrayList<String>();
        if (ViewControllerContainer.ViewController.point)
        {
            List<HomeTeamRoster> roster = ViewControllerContainer.ViewController.currentGame.getHomeTeamRoster();
            for (int i =0; i < roster.size(); i++)
            {
                your_array_list.add(i, roster.get(i).getFirstName() + ", " + roster.get(i).getLastName());
            }
        }
        else
        {
            List<AwayTeamRoster> roster = ViewControllerContainer.ViewController.currentGame.getAwayTeamRoster();
            for (int i =0; i < roster.size(); i++)
            {
                your_array_list.add(i, roster.get(i).getFirstName() + ", " + roster.get(i).getLastName());
            }
        }




        // This is the array adapter, it takes the context of the activity as a
        // first parameter, the type of list view as a second parameter and your
        // array as a third parameter.
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(
                this,
                android.R.layout.simple_list_item_1,
                your_array_list );

        lv.setAdapter(arrayAdapter);

        lv.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {
                if (ViewControllerContainer.ViewController.point)
                {
                    List<HomeTeamRoster> roster = ViewControllerContainer.ViewController.currentGame.getHomeTeamRoster();
                    ViewControllerContainer.ViewController.playerId=roster.get(position).getPlayerId();
                }
                else
                {
                    List<AwayTeamRoster> roster = ViewControllerContainer.ViewController.currentGame.getAwayTeamRoster();
                    ViewControllerContainer.ViewController.playerId=roster.get(position).getPlayerId();
                }

                if(ViewControllerContainer.ViewController.scoring==10)
                {
                    ViewControllerContainer.ViewController.recordFoul();
                    ViewControllerContainer.ViewController.updateFoul();
                    ViewControllerContainer.ViewController.scoring=0;
                }
                else{
                    ViewControllerContainer.ViewController.recordScore();
                    ViewControllerContainer.ViewController.updateScore();
                }
                finish();
            }
        });

    }
}
