package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.ListView;
import android.widget.Switch;
import android.widget.TextView;

import com.example.capstoneui.Controller.ViewControllerContainer;
import com.example.capstoneui.Models.AvailableMedia;
import com.example.capstoneui.Models.AvailableVideo;
import com.example.capstoneui.Models.AwayTeamRoster;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.ArrayList;
import java.util.List;

public class ProducerScreen extends AppCompatActivity {

    private ListView lvVideos;
    private ListView lvImages;
    private Button btnVideos;
    private Button btnImages;
    private Switch mediaChoice;
    private boolean ismediaShowing;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_producer_screen);
        ismediaShowing=false;
        btnImages = (Button) findViewById(R.id.btnImages);
        btnVideos = (Button) findViewById(R.id.btnVideos);

        btnVideos.setBackgroundColor(Color.DKGRAY);
        btnImages.setBackgroundColor(Color.GRAY);

        lvVideos = (ListView) findViewById(R.id.dyListVideos);
        lvImages = (ListView) findViewById(R.id.dyListImages);


        mediaChoice = (Switch) findViewById(R.id.mediaSwitch);
        lvImages.setVisibility(View.INVISIBLE);
        mediaChoice.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked)
                {
                    ViewControllerContainer.ViewController.showMedia();

                }
                else
                {
                    ViewControllerContainer.ViewController.showScoreboard();
                }
            }
        });


    loadData();
    }


    public void loadData()
    {


        final List<String> imagesList = new ArrayList<String>();
        final List<String> videosList = new ArrayList<String>();


        for (int i = 0; i < ViewControllerContainer.ViewController.currentMedia.getAvailableImages().size(); i++)
        {
            imagesList.add(""+ ViewControllerContainer.ViewController.currentMedia.getAvailableImages().get(i));

        }

        final List<AvailableVideo> videos = ViewControllerContainer.ViewController.currentMedia.getAvailableVideos();
        for (int i = 0; i < ViewControllerContainer.ViewController.currentMedia.getAvailableVideos().size(); i++)
        {
            videosList.add(""+ videos.get(i).getFilename() + " Duration: " + videos.get(i).getDuration());
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
                ViewControllerContainer.ViewController.playVideoMedia(videos.get(position).getFilename());
                mediaChoice.setOnCheckedChangeListener (null);
                mediaChoice.setChecked(true);
                mediaChoice.setOnCheckedChangeListener (new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                        if (isChecked)
                        {
                            ViewControllerContainer.ViewController.showMedia();

                        }
                        else
                        {
                            ViewControllerContainer.ViewController.showScoreboard();
                        }
                    }
                });

            }
        });
        lvImages.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {
                ViewControllerContainer.ViewController.playAvailImage(imagesList.get(position));
                mediaChoice.setOnCheckedChangeListener (null);
                mediaChoice.setChecked(true);
                mediaChoice.setOnCheckedChangeListener (new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                        if (isChecked)
                        {
                            ViewControllerContainer.ViewController.showMedia();

                        }
                        else
                        {
                            ViewControllerContainer.ViewController.showScoreboard();
                        }
                    }
                });

            }
        });
    }
    public void switchImages(View view) {
        view.setBackgroundColor(Color.DKGRAY);
        btnVideos.setBackgroundColor(Color.GRAY);
        lvVideos.setVisibility(View.INVISIBLE);
        lvImages.setVisibility(View.VISIBLE);
    }

    public void switchVideos(View view) {
        view.setBackgroundColor(Color.DKGRAY);
        btnImages.setBackgroundColor(Color.GRAY);
        lvVideos.setVisibility(View.VISIBLE);
        lvImages.setVisibility(View.INVISIBLE);
    }

    public void returnScoreBoard(View view) {
        finish();
    }
}
