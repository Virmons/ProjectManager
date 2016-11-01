package com.example.simeons.projectmanager;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class AddPersonnelActivity extends AppCompatActivity {

    private int ID;
    private boolean IsEdit;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_personnel);

        ID = getIntent().getIntExtra("ID",0);
        IsEdit = getIntent().getBooleanExtra("IsEdit",false);


    }
}
