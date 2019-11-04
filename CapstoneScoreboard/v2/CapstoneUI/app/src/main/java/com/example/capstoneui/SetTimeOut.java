package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.RadioButton;
import android.widget.RadioGroup;

import com.example.capstoneui.Controller.ViewControllerContainer;
import com.example.capstoneui.Models.AwayTeamRoster;
import com.example.capstoneui.Models.HomeTeamRoster;

import java.util.ArrayList;
import java.util.List;

public class SetTimeOut extends AppCompatActivity {

    private ListView lv;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_set_time_out);

    }





    public void setTimeout(View view) {
        RadioGroup radioGroup = (RadioGroup) findViewById(R.id.radioGroup);

                // get selected radio button from radioGroup
                int selectedId = radioGroup.getCheckedRadioButtonId();

                // find the radiobutton by returned id
                RadioButton rb = (RadioButton) findViewById(selectedId);
                if (rb.getTag().equals("team1"))
                {
                    ViewControllerContainer.ViewController.point=true;
                }
                else {
                    ViewControllerContainer.ViewController.point = false;
                }
        EditText et = (EditText) findViewById(R.id.editText);
        Integer timeouts = Integer.parseInt(et.getText().toString());
        ViewControllerContainer.ViewController.setTimeOuts(timeouts);
        ViewControllerContainer.ViewController.updateTimeouts();
        finish();

    }
}
