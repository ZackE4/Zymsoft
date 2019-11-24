package com.example.capstoneui;


import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.example.capstoneui.Models.AvailableVideo;

import java.util.List;

import androidx.annotation.NonNull;

public class CustomAdapter extends ArrayAdapter<String> {


        String A[];
        List<AvailableVideo> B;
        LayoutInflater mInfalter;
        public CustomAdapter(Context context, String[] A, List<AvailableVideo> B)
        {
            super(context,R.layout.medialist,A);
            this.A = A;
            this.B = B;
            mInfalter = LayoutInflater.from(context);
        }

    public CustomAdapter(@NonNull Context context, int resource) {
        super(context, resource);
    }

    public View getView(int position, View convertView, ViewGroup parent) {
            ViewHolder holder;
            if(convertView==null)
            {
                convertView = mInfalter.inflate(R.layout.medialist,parent,false);
                holder = new ViewHolder();
                holder.tv1 = (TextView)convertView.findViewById(R.id.txtMediaName);
                holder.tv2 = (TextView)convertView.findViewById(R.id.txtMediaDuration);
                holder.tv1.setGravity(View.FOCUS_LEFT);
                holder.tv2.setGravity(View.FOCUS_RIGHT);
                holder.tv2.setPadding(20,10,0,10);
                holder.tv1.setPadding(0,10,20,10);
                convertView.setTag(holder);
            }else{
                holder = (ViewHolder)convertView.getTag();
            }

            holder.tv1.setText(B.get(position).getFilename());
            holder.tv2.setText(B.get(position).getDuration());
            return convertView;
        }
        static class ViewHolder
        {
            TextView tv1,tv2;
        }

}
