package com.example.simeons.projectmanager;

import android.app.Activity;
import android.os.Bundle;
import android.widget.Toast;

public class ProjectInProgressActivity extends Activity {

    int ID;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_project_in_progress);
        int ID = getIntent().getIntExtra("ID",0);
        Toast.makeText(getApplicationContext(), Integer.toString(ID), Toast.LENGTH_SHORT).show();
    }
}
