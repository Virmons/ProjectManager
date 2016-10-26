package com.example.simeons.projectmanager;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;

public class ProjectManagerActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_project_manager);
    }

    public void addProject(View view){
        Intent intent = new Intent(this,ProjectActivity.class);
        startActivity(intent);
    }

    public void addTable1(View view){
        Intent intent = new Intent(this,AddStoryActivity.class);
        startActivity(intent);
    }

    public void addTable2(View view){
        Intent intent = new Intent(this,AddPersonnelActivity.class);
        startActivity(intent);
    }
}
