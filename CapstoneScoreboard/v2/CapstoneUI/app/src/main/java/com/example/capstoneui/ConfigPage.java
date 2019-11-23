package com.example.capstoneui;

import androidx.appcompat.app.AppCompatActivity;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import com.example.capstoneui.Controller.UnsafeOkHttpClient;
import com.example.capstoneui.Controller.ViewControllerContainer;
import com.example.capstoneui.R;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

public class ConfigPage extends AppCompatActivity {

    private EditText edt1;
    private EditText edt2;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_config_page);
        edt1 = (EditText) findViewById(R.id.edtIp);
        edt1.setHintTextColor(R.style.TextAltColor);
        edt2 = (EditText) findViewById(R.id.edtPass);
        edt2.setHintTextColor(R.style.TextAltColor);

    }

    public void submitIP(View view) {
        //validation
        String ipaddress = edt1.getText().toString();
        String apiPass = edt2.getText().toString();
        ipaddress="192.168.0.44";
        apiPass="168de16b";
        ViewControllerContainer.ViewController.apiKey=""+apiPass;
        Gson gson = new GsonBuilder()
                .setLenient()
                .create();


        ViewControllerContainer.ViewController.okHttpClient = UnsafeOkHttpClient.getUnsafeOkHttpClient();
        //Retrofit allows the connection to post man/webapi
        ViewControllerContainer.ViewController.retrofit = new Retrofit.Builder()
                .baseUrl("http://" +  ipaddress + "/api/Scoreboard/")
                .client(ViewControllerContainer.ViewController.okHttpClient)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .build();

        ViewControllerContainer.ViewController.checkConnection();
        ViewControllerContainer.ViewController.checkConnection();
        try {
            if (ViewControllerContainer.ViewController.connectionStat.equals("Success"))
            {
                ViewControllerContainer.ViewController.ipAddress=ipaddress;
                Intent intent = new Intent(this, MainActivity.class);//new intent
                startActivity(intent);//gets into intent
            }
            else
            {
                Toast.makeText(this, "Invalid Information Please try again", Toast.LENGTH_LONG);
            }
        }
        catch (Exception ex)
        {
            Toast.makeText(this, "Invalid Information Please try again", Toast.LENGTH_LONG);
        }



    }
}
