package com.example.simeons.projectmanager;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;

public class ProjectManagerActivity extends AppCompatActivity {

    private Button mAddProjectButton;
    private TextView mTable1Header;
    private TextView mTable2Header;
    private ListView mTable1;
    private ListView mTable2;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        mAddProjectButton = (Button) findViewById(R.id.button_add_project);
        mTable1Header = (TextView) findViewById(R.id.table1Header);
        mTable2Header = (TextView) findViewById(R.id.table2Header);
        mTable1 = (ListView) findViewById(R.id.table1List);
        mTable2 = (ListView) findViewById(R.id.table2List);

        int ID = getIntent().getIntExtra("ID",0);
        if(ID != 0)
        {
            mAddProjectButton.setText(R.string.btn_update_project);
            //TODO Set this as a runnable and get and populate the listviews
            getProjectData(ID);
        }
        setContentView(R.layout.activity_project_manager);
    }

    //For the addproject button, navigates to add Project Activity
    public void addProject(View view){
        Intent intent = new Intent(this,ProjectActivity.class);
        startActivity(intent);
    }

    //For the addTable1 button, navigates to screen for adding a new item to table1 (Story)
    public void addTable1(View view){
        Intent intent = new Intent(this,AddStoryActivity.class);
        startActivity(intent);
    }

    //For the addTable1 button, navigates to screen for adding a new item to table2 (Personnel)
    public void addTable2(View view){
        Intent intent = new Intent(this,AddPersonnelActivity.class);
        startActivity(intent);
    }

    //Gets the data to populate the listviews
    public void getProjectData(int ID) {
        //TODO Add in httpget for project peronnel and stories
    }
}
