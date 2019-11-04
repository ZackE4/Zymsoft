package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.example.capstoneui.Controller.ViewControllerContainer;
import com.example.capstoneui.Models.AwayTeamRoster;
import com.example.capstoneui.Models.HomeTeamRoster;

import java.util.ArrayList;
import java.util.List;

public class ChooseScore extends AppCompatActivity {

    private ListView lv;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_choose_score);

        lv = (ListView) findViewById(R.id.listViewAddSc);
        List<String> your_array_list = new ArrayList<String>();

        for (int i = 1; i < 4;i++)
        {
            your_array_list.add("" + i);
        }


        // This is the array adapter, it takes the context of the activity as a
        // first parameter, the type of list view as a second parameter and your
        // array as a third parameter.
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(
                this,
                android.R.layout.simple_list_item_1,
                your_array_list );

        lv.setAdapter(arrayAdapter);
        final Context context = getApplicationContext();

        lv.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {
                ViewControllerContainer.ViewController.scoring=position+1;
                Intent intent = new Intent(context, AddScore.class);//new intent
                startActivity(intent);//gets into intent
                finish();
            }
        });
    }
}
