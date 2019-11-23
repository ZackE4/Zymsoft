package com.example.capstoneui;


import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import androidx.annotation.NonNull;

public class CustomAdapter extends ArrayAdapter<String> {


        String A[],B[];
        LayoutInflater mInfalter;
        public CustomAdapter(Context context, String[] A, String B[])
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
                convertView.setTag(holder);
            }else{
                holder = (ViewHolder)convertView.getTag();
            }

            holder.tv1.setText(A[position]);
            holder.tv2.setText(B[position]);
            return convertView;
        }
        static class ViewHolder
        {
            TextView tv1,tv2;
        }

}
