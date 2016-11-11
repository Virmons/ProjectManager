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
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.BasicNetwork;
import com.android.volley.toolbox.DiskBasedCache;
import com.android.volley.toolbox.HurlStack;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.logging.Level;
import java.util.logging.Logger;

import static android.view.View.VISIBLE;
import static com.example.simeons.projectmanager.Constants.ADD_PROJECT_REQUEST;
import static com.example.simeons.projectmanager.Constants.EDIT_PROJECT_REQUEST;
import static com.example.simeons.projectmanager.Constants.PROJECTS_API_GETALL;

public class ProjectActivity extends Activity {

    ListView listView;
    ProgressBar progressBar;
    //    ArrayList<String> projects;
    JSONArray projects;
    JSONObject project1;
    JSONObject project2;
    String url;
    ArrayList<String> values = new ArrayList<String>();

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

//        projects = new JSONArray();
//        projects.put(project1);
//        projects.put(project2);

        url = PROJECTS_API_GETALL + "SS";

        new getProjectData().execute();

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

    public String getJSON(String url, int timeout) {
        HttpURLConnection c = null;
        try {
            URL u = new URL(url);
            c = (HttpURLConnection) u.openConnection();
            c.setRequestMethod("GET");
            c.setRequestProperty("Content-length", "0");
            c.setUseCaches(false);
            c.setAllowUserInteraction(false);
            c.setConnectTimeout(timeout);
            c.setReadTimeout(timeout);
            c.connect();
            int status = c.getResponseCode();

            Log.i("Status Code", Integer.toString(status));

            switch (status) {
                case 200:
                case 201:
                    BufferedReader br = new BufferedReader(new InputStreamReader(c.getInputStream()));
                    StringBuilder sb = new StringBuilder();
                    String line;
                    while ((line = br.readLine()) != null) {
                        sb.append(line.concat("\n"));
                    }
                    br.close();
                    return sb.toString();
                case 404:
                    System.out.println("404");

                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {

                            Toast.makeText(getApplicationContext(), "User Not Found", Toast.LENGTH_SHORT);

                        }
                    });
                    return null;
                case 501:
                    System.out.println("501");

                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {


                            Toast.makeText(getApplicationContext(), "501 Error", Toast.LENGTH_SHORT);
                        }
                    });
                    return null;
                default:
                    break;
            }

        } catch (UnknownHostException e) {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {

                    Toast.makeText(getApplicationContext(), "Check your network connection", Toast.LENGTH_SHORT);

                }
            });
        }
        catch (MalformedURLException ex) {
            Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException ex) {
            Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
        } finally {
            if (c != null) {
                try {
                    c.disconnect();
                } catch (Exception ex) {
                    Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
                }
            }
        }
        return null;
    }

    class getProjectData extends AsyncTask<Void,Void,Integer> {

        String data;

        @Override
        protected void onPreExecute() {

            progressBar.setVisibility(VISIBLE);

        }

        @Override
        protected Integer doInBackground(Void... params) {
            data = getJSON(url,30000);
            try {

                projects = new JSONArray(data);

            }
            catch(JSONException je){

                Toast.makeText(getApplicationContext(), "Error converting input string to JSONArray", Toast.LENGTH_SHORT);

            }

            for (int i = 0; i < projects.length(); i++) {
                JSONObject project = new JSONObject();
                try {
                    project = projects.getJSONObject(i);
//                String id=project.getString("ID");
                    String name = project.getString("ProjectName");
//                String dateCreated = project.getString("DateCreated");
//                String active = project.getString("Active");
                    values.add(name);
                    Log.d(name, "Output");
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }


            return null;
        }

        protected void onPostExecute(Integer result){

            progressBar.setVisibility(View.GONE);

            ArrayAdapter<String> adapter = new ArrayAdapter<String>(ProjectActivity.this,
                    android.R.layout.simple_list_item_1, android.R.id.text1, values);


            // Assign adapter to ListView
            listView.setAdapter(adapter);

            //        new getProjectData().execute("SS");

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
    }
}