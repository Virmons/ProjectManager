package com.example.simeons.projectmanager;

import android.app.Activity;
import android.os.Bundle;
import android.widget.Toast;

import com.example.simeons.projectmanager.Model.Project;
import com.google.gson.Gson;

public class ProjectInProgressActivity extends Activity {

    int ID;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_project_in_progress);
        int ID = getIntent().getIntExtra("ID",0);
        String projectString  = getIntent().getStringExtra("Project");
        Project project = (new Gson()).fromJson(projectString,Project.class);

        Toast.makeText(getApplicationContext(),"yoyoyoyoyo", Toast.LENGTH_LONG);

    }
}
