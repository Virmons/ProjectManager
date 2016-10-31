package com.example.simeons.projectmanager;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.util.ArrayList;

import static android.content.ContentValues.TAG;

public class ProjectActivity extends Activity {

    ListView listView ;
    ProgressBar progressBar;
    ArrayList<String> values;
//    ArrayList<String> projects;
    JSONArray projects;
    JSONObject project1;
    JSONObject project2;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_project);

        // Get ListView object from xml
        listView = (ListView) findViewById(R.id.list);
        progressBar = (ProgressBar) findViewById(R.id.ProjectProgressBar);

        //TODO remove these hardcoded values, pull through getProjects()
        // Defined Array values to show in ListView
        JSONObject project1 = new JSONObject();
        try{
            project1.put("ID",1);
            project1.put("Name","ANPR");
            project1.put("DateCreated","2016-10-31 13:28:00");
            project1.put("Active",true);
        }
        catch(JSONException e){
            e.printStackTrace();
        }

        JSONObject project2 = new JSONObject();
        try{
            project2.put("ID","2");
            project2.put("Name","CSC");
            project2.put("DateCreated","2016-10-31 13:28:00");
            project2.put("Active",false);
        }
        catch(JSONException e){
            e.printStackTrace();
        }

        projects = new JSONArray();
        projects.put(project1);
        projects.put(project2);

        ArrayList<String> values = new ArrayList<String>();
        for(int i=0; i < projects.length() ; i++) {
            JSONObject project = new JSONObject();
            try{
                project = projects.getJSONObject(i);
//                String id=project.getString("ID");
                String name=project.getString("Name");
//                String dateCreated = project.getString("DateCreated");
//                String active = project.getString("Active");
                values.add(name);
                Log.d(name,"Output");
            }
            catch(JSONException e)
            {
                e.printStackTrace();
            }
        }

        // Define a new Adapter
        // First parameter - Context
        // Second parameter - Layout for the row
        // Third parameter - ID of the TextView to which the data is written
        // Forth - the Array of data

        ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_list_item_1, android.R.id.text1, values);


        // Assign adapter to ListView
        listView.setAdapter(adapter);

        //ListViewItemHoldListener
        listView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener(){
            @Override
            public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
                try{
                    int ID = projects.getJSONObject(position).getInt("ID");
                    editProject(ID);
                }
                catch(JSONException e)
                {
                    e.printStackTrace();
                }
                return true;
            }
        });

        // ListView Item Click Listener
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {

                // ListView Clicked item index
                int itemPosition = position;

                // ListView Clicked item value
                String itemValue = (String) listView.getItemAtPosition(position);

                try {
                    int ID = projects.getJSONObject(position).getInt("ID");
                    Boolean active = projects.getJSONObject(position).getBoolean("Active");
                    if(active == true)
                    {
                        showProjectDetails(ID);
                    }
                }
                catch(JSONException e)
                {
                    e.printStackTrace();
                }
                // Show Alert
                Toast.makeText(getApplicationContext(),
                        "Position :" + itemPosition + "  ListItem : " + itemValue, Toast.LENGTH_LONG)
                        .show();

            }

        });



    }

    public void addNewProject(View view) {

        Intent intent = new Intent(ProjectActivity.this, ProjectManagerActivity.class);
        startActivity(intent);

    }

    public void showProjectDetails(int ID) {

        Intent intent = new Intent(ProjectActivity.this, ProjectInProgressActivity.class);
        intent.putExtra("ID",ID);
        startActivity(intent);

    }

    public void editProject(int ID)
    {
        Intent intent = new Intent(ProjectActivity.this, ProjectManagerActivity.class);
        intent.putExtra("ID", ID);
        startActivity(intent);
    }

    public String getProjects() {
        String response = null;
        try {
            URL url = new URL("http://http://192.168.0.77:56020/api/Projects/getAllProjects/SS");
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setRequestMethod("GET");
            // read the response
            InputStream in = new BufferedInputStream(conn.getInputStream());
            response = convertStreamToString(in);
            Toast.makeText(getApplicationContext(),response,Toast.LENGTH_LONG).show();
        } catch (MalformedURLException e) {
            Log.e(TAG, "MalformedURLException: " + e.getMessage());
        } catch (ProtocolException e) {
            Log.e(TAG, "ProtocolException: " + e.getMessage());
        } catch (IOException e) {
            Log.e(TAG, "IOException: " + e.getMessage());
        } catch (Exception e) {
            Log.e(TAG, "Exception: " + e.getMessage());
        }
        return response;
    }

    private String convertStreamToString(InputStream is) {
        BufferedReader reader = new BufferedReader(new InputStreamReader(is));
        StringBuilder sb = new StringBuilder();

        String line;
        try {
            while ((line = reader.readLine()) != null) {
                sb.append(line).append('\n');
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                is.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        return sb.toString();
    }
}


