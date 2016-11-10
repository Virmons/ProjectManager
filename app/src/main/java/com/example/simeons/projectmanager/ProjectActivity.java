package com.example.simeons.projectmanager;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.Toast;

import com.android.volley.Cache;
import com.android.volley.Network;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.BasicNetwork;
import com.android.volley.toolbox.DiskBasedCache;
import com.android.volley.toolbox.HurlStack;
import com.android.volley.toolbox.JsonArrayRequest;

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
import static com.example.simeons.projectmanager.Constants.ADD_PROJECT_REQUEST;
import static com.example.simeons.projectmanager.Constants.EDIT_PROJECT_REQUEST;
import static com.example.simeons.projectmanager.Constants.PROJECTS_API_GETALL;

public class ProjectActivity extends Activity {

    ListView listView;
    ProgressBar progressBar;
    ArrayList<String> values;
    //    ArrayList<String> projects;
    JSONArray projects;
    JSONObject project1;
    JSONObject project2;
    String url;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_project);

        // Get ListView object from xml
        listView = (ListView) findViewById(R.id.list);
        progressBar = (ProgressBar) findViewById(R.id.ProjectProgressBar);

        RequestQueue mRequestQueue;

// Instantiate the cache
        Cache cache = new DiskBasedCache(getCacheDir(), 1024 * 1024); // 1MB cap

// Set up the network to use HttpURLConnection as the HTTP client.
        Network network = new BasicNetwork(new HurlStack());

// Instantiate the RequestQueue with the cache and network.
        mRequestQueue = new RequestQueue(cache, network);

// Start the queue
        mRequestQueue.start();

        //TODO remove these hardcoded values, pull through getProjects()
        // Defined Array values to show in ListView
//        JSONObject project1 = new JSONObject();
//        try{
//            project1.put("ID",1);
//            project1.put("Name","ANPR");
//            project1.put("DateCreated","2016-10-31 13:28:00");
//            project1.put("Active",true);
//        }
//        catch(JSONException e){
//            e.printStackTrace();
//        }
//
//        JSONObject project2 = new JSONObject();
//        try{
//            project2.put("ID","2");
//            project2.put("Name","CSC");
//            project2.put("DateCreated","2016-10-31 13:28:00");
//            project2.put("Active",false);
//        }
//        catch(JSONException e){
//            e.printStackTrace();
//        }

        projects = new JSONArray();
//        projects.put(project1);
//        projects.put(project2);

        url = new String(PROJECTS_API_GETALL + "SS");

        progressBar.setVisibility(View.VISIBLE);

        JsonArrayRequest projectDataRequest = new JsonArrayRequest
                (Request.Method.GET, url, null, new Response.Listener<JSONArray>() {

                    @Override
                    public void onResponse(JSONArray response) {

                        progressBar.setVisibility(View.GONE);

                        ArrayList<String> values = new ArrayList<String>();
                        for (int i = 0; i < projects.length(); i++) {
                            JSONObject project = new JSONObject();
                            try {
                                project = projects.getJSONObject(i);
//                String id=project.getString("ID");
                                String name = project.getString("Name");
//                String dateCreated = project.getString("DateCreated");
//                String active = project.getString("Active");
                                values.add(name);
                                Log.d(name, "Output");
                            } catch (JSONException e) {
                                e.printStackTrace();
                            }
                        }

                        ArrayAdapter<String> adapter = new ArrayAdapter<String>(ProjectActivity.this,
                                android.R.layout.simple_list_item_1, android.R.id.text1, values);


                        // Assign adapter to ListView
                        listView.setAdapter(adapter);

                    }
                }, new Response.ErrorListener() {

                    @Override
                    public void onErrorResponse(VolleyError error) {
                        // TODO Auto-generated method stub
                        Toast.makeText(getApplicationContext(), "Error", Toast.LENGTH_SHORT);

                    }
                });
//        new getProjectData().execute("SS");

        mRequestQueue.add(projectDataRequest);

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
                    if (active == true) {
                        showProjectDetails(ID);
                    }
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                // Show Alert
                Toast.makeText(getApplicationContext(),
                        "Position :" + itemPosition + "  ListItem : " + itemValue, Toast.LENGTH_LONG)
                        .show();

            }

        });

        //ListViewItemHoldListener
        listView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
                try {
                    int ID = projects.getJSONObject(position).getInt("ID");
                    editProject(ID);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                return true;
            }
        });


    }

    public void addNewProject(View view) {

        Intent intent = new Intent(ProjectActivity.this, ProjectManagerActivity.class);
        startActivityForResult(intent, ADD_PROJECT_REQUEST);

    }

    public void showProjectDetails(int ID) {

        Intent intent = new Intent(ProjectActivity.this, ProjectInProgressActivity.class);
        intent.putExtra("ID", ID);
        startActivity(intent);

    }

    public void editProject(int ID) {
        Intent intent = new Intent(ProjectActivity.this, ProjectManagerActivity.class);
        intent.putExtra("ID", ID);
        startActivityForResult(intent, EDIT_PROJECT_REQUEST);
    }

    class getProjectData extends AsyncTask<String, Void, JSONArray>{


        private Exception exception;

        protected void onPreExecute()
        {
            Log.i("Async","on Pre Execute");
            progressBar.setVisibility(View.VISIBLE);
        }

        protected JSONArray doInBackground(String... initials)
        {
            Log.i("Async","do In Background");

            JSONArray response = null;

            //API CALL
            try {
                URL url = new URL(PROJECTS_API_GETALL + initials);
                HttpURLConnection conn = (HttpURLConnection) url.openConnection();
                conn.setRequestMethod("GET");
                // read the response
                InputStream in = new BufferedInputStream(conn.getInputStream());
                response = new JSONArray(convertStreamToString(in));
            } catch (MalformedURLException e) {
                Log.e(TAG, "MalformedURLException: " + e.getMessage());
            } catch (ProtocolException e) {
                Log.e(TAG, "ProtocolException: " + e.getMessage());
            } catch (IOException e) {
                Log.e(TAG, "IOException: " + e.getMessage());
            } catch (Exception e) {
                Log.e(TAG, "Exception: " + e.getMessage());
            }
            //API CALL END

            //TODO remove these hardcoded values, pull through getProjects()
            // Defined Array values to show in ListView
            //HARDCODED
//        JSONObject project1 = new JSONObject();
//        try{
//            project1.put("ID",1);
//            project1.put("Name","ANPR");
//            project1.put("DateCreated","2016-10-31 13:28:00");
//            project1.put("Active",true);
//        }
//        catch(JSONException e){
//            e.printStackTrace();
//        }
//
//        JSONObject project2 = new JSONObject();
//        try{
//            project2.put("ID","2");
//            project2.put("Name","CSC");
//            project2.put("DateCreated","2016-10-31 13:28:00");
//            project2.put("Active",false);
//        }
//        catch(JSONException e){
//            e.printStackTrace();
//        }
//
//                projects = new JSONArray();
//                projects.put(project1);
//                projects.put(project2);
//                response = projects;
            //HARDCODED END

            return response;
        }

        protected void onPostExecute(JSONArray response)
        {
            Log.i("Async","Post Execute");
            progressBar.setVisibility(View.GONE);

            ArrayList<String> values = new ArrayList<String>();
            for (int i = 0; i < projects.length(); i++) {
                JSONObject project = new JSONObject();
                try {
                    project = projects.getJSONObject(i);
//                String id=project.getString("ID");
                    String name = project.getString("Name");
//                String dateCreated = project.getString("DateCreated");
//                String active = project.getString("Active");
                    values.add(name);
                    Log.d(name, "Output");
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }

            // Define a new Adapter
            // First parameter - Context
            // Second parameter - Layout for the row
            // Third parameter - ID of the TextView to which the data is written
            // Forth - the Array of data

            ArrayAdapter<String> adapter = new ArrayAdapter<String>(ProjectActivity.this,
                    android.R.layout.simple_list_item_1, android.R.id.text1, values);


            // Assign adapter to ListView
            listView.setAdapter(adapter);
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

    }







