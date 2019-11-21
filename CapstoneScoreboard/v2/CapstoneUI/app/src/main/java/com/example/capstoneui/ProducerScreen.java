package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;

import com.example.capstoneui.Controller.ViewControllerContainer;

import java.util.ArrayList;
import java.util.List;

public class ProducerScreen extends AppCompatActivity {

    private ListView lvVideos;
    private ListView lvImages;
    private Button btnVideos;
    private Button btnImages;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_producer_screen);

        btnImages = (Button) findViewById(R.id.btnImages);
        btnVideos = (Button) findViewById(R.id.btnVideos);

        lvVideos = (ListView) findViewById(R.id.dyListVideos);
        lvImages = (ListView) findViewById(R.id.dyListImages);

        List<String> imagesList = new ArrayList<String>();
        List<String> videosList = new ArrayList<String>();

        for (int i = 1; i < 4;i++)
        {
            imagesList.add("" + i);
            videosList.add("" + i);
        }


        // This is the array adapter, it takes the context of the activity as a
        // first parameter, the type of list view as a second parameter and your
        // array as a third parameter.
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(
                this,
                android.R.layout.simple_list_item_1,
                imagesList);

        ArrayAdapter<String> arrayAdapter2 = new ArrayAdapter<String>(
                this,
                android.R.layout.simple_list_item_1,
                videosList);

        lvVideos.setAdapter(arrayAdapter2);
        lvImages.setAdapter(arrayAdapter);
        final Context context = getApplicationContext();

        lvVideos.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {


            }
        });
        lvImages.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {


            }
        });
    }

    public void switchImages(View view) {
        view.setBackgroundColor(Color.DKGRAY);

    }
}
