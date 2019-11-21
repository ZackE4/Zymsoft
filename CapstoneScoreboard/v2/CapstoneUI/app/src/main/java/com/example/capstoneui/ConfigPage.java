package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;

import com.example.capstoneui.Controller.ViewControllerContainer;
import com.example.capstoneui.R;

public class ConfigPage extends AppCompatActivity {

    private EditText edt1;
    private EditText edt2;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_config_page);
        edt1 = (EditText) findViewById(R.id.edtIp);
        edt1.setHintTextColor(R.style.TextAltColor);
        edt2 = (EditText) findViewById(R.id.edtIp);
        edt2.setHintTextColor(R.style.TextAltColor);

    }

    public void submitIP(View view) {
        //validation
        MainActivity.ipaddress = edt1.getText().toString();
        MainActivity.apiKey = edt2.getText().toString();
//        ViewControllerContainer.ViewController.checkConnection();
//        if (ViewControllerContainer.ViewController.connectionStat.equals(""))
        Intent intent = new Intent(this, MainActivity.class);//new intent
        startActivity(intent);//gets into intent
    }
}
