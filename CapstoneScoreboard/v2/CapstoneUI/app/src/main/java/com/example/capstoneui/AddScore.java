package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import com.example.capstoneui.Controller.ViewController;

import java.util.ArrayList;
import java.util.List;

public class AddScore extends AppCompatActivity {

    private ListView lv;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_score);

        ViewController viewController = new ViewController("168de16b", this);
        lv = (ListView) findViewById(R.id.listViewAddSc);

        // Instanciating an array list (you don't need to do this,
        // you already have yours).
        List<String> your_array_list = new ArrayList<String>();
        //adding list of players
        your_array_list.add("foo");
        your_array_list.add("bar");
        your_array_list.add("foo");
        your_array_list.add("bar");
        your_array_list.add("foo");
        your_array_list.add("bar");
        your_array_list.add("foo");
        your_array_list.add("bar");
        your_array_list.add("foo");
        your_array_list.add("bar");
        your_array_list.add("foo");
        your_array_list.add("bar");

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
                    //onselect

                finish();
            }
        });

    }
}
