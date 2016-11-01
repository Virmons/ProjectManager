package com.example.simeons.projectmanager;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;

import static com.example.simeons.projectmanager.Constants.PROJECTS_API_ADD;

public class ProjectManagerActivity extends AppCompatActivity {

    private Button mAddProjectButton;
    private TextView mTable1Header;
    private TextView mTable2Header;
    private ListView mTable1;
    private ListView mTable2;
    private ProgressBar mProgressBar;
    private JSONArray stories;
    private JSONArray personnel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_project_manager);

        mAddProjectButton = (Button) findViewById(R.id.buttonAddProjectPM);
        mTable1Header = (TextView) findViewById(R.id.storiesHeader);
        mTable2Header = (TextView) findViewById(R.id.personnelHeader);
        mTable1 = (ListView) findViewById(R.id.storiesList);
        mTable2 = (ListView) findViewById(R.id.personnelList);
        mProgressBar = (ProgressBar) findViewById(R.id.ProjectManagerProgressBar);

        int ID = getIntent().getIntExtra("ID", 0);

        stories = new JSONArray();
        personnel = new JSONArray();

        if (ID != 0) {
            mAddProjectButton.setText(R.string.btn_update_project);
            //TODO Set this as a runnable and get and populate the listviews
            new getProjectData().execute(ID);
        }

        // ListView Item Click Listener
        mTable1.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {

                // ListView Clicked item index
                int itemPosition = position;

                // ListView Clicked item value
                String itemValue = (String) mTable1.getItemAtPosition(position);

                try {
                    int ID = stories.getJSONObject(position).getInt("ID");
                    Boolean active = stories.getJSONObject(position).getBoolean("Active");
                    if (active == true) {
                        showStoryDetails(ID);
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
        mTable1.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
                try {
                    int ID = stories.getJSONObject(position).getInt("ID");
                    editStoryDetails(ID);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                return true;
            }
        });

        // ListView Item Click Listener
        mTable2.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {

                // ListView Clicked item index
                int itemPosition = position;

                // ListView Clicked item value
                String itemValue = (String) mTable2.getItemAtPosition(position);

                try {
                    int ID = personnel.getJSONObject(position).getInt("ID");
                    Boolean active = personnel.getJSONObject(position).getBoolean("Active");
                    if (active == true) {
                        showPersonnelDetails(ID);
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
        mTable2.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
                try {
                    int ID = personnel.getJSONObject(position).getInt("ID");
                    editPersonnelDetails(ID);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                return true;
            }
        });

    }

    //For the addproject button, navigates to add Project Activity
    public String addProject(View view, JSONArray project){
        String result = null;
        if(mAddProjectButton.getText() == "Update"){
            //TODO Fire off update project API call

        }
        else if(mAddProjectButton.getText() == "Add Project"){
            //TODO Fire off add project API call
            HttpURLConnection urlConnection;
            String url = PROJECTS_API_ADD;
            String data = project.toString();

            try {
                //Connect
                urlConnection = (HttpURLConnection) ((new URL(url).openConnection()));
                urlConnection.setDoOutput(true);
                urlConnection.setRequestProperty("Content-Type", "application/json");
                urlConnection.setRequestProperty("Accept", "application/json");
                urlConnection.setRequestMethod("POST");
                urlConnection.connect();

                //Write
                OutputStream outputStream = urlConnection.getOutputStream();
                BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(outputStream, "UTF-8"));
                writer.write(data);
                writer.close();
                outputStream.close();

                //Read
                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream(), "UTF-8"));

                String line = null;
                StringBuilder sb = new StringBuilder();

                while ((line = bufferedReader.readLine()) != null) {
                    sb.append(line);
                }

                bufferedReader.close();
                result = sb.toString();

            }
            catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            } catch (IOException e) {
                e.printStackTrace();
            }

        }
        return result;
    }

    //For the addTable1 button, navigates to screen for adding a new item to table1 (Story)
    public void addStory(View view){
        Intent intent = new Intent(this,AddStoryActivity.class);
        startActivity(intent);
    }

    //For the addTable1 button, navigates to screen for adding a new item to table2 (Personnel)
    public void addPersonnel(View view){
        Intent intent = new Intent(this,AddPersonnelActivity.class);
        startActivity(intent);
    }

    public void showStoryDetails(int ID) {
        Intent intent = new Intent(this, AddStoryActivity.class);
        intent.putExtra("ID",ID);
        intent.putExtra("IsEdit",false);
        startActivity(intent);
    }

    public void editStoryDetails(int ID){
        Intent intent = new Intent(this, AddStoryActivity.class);
        intent.putExtra("ID",ID);
        intent.putExtra("IsEdit", true);
        startActivity(intent);
    }

    public void showPersonnelDetails(int ID){
        Intent intent = new Intent(this, AddPersonnelActivity.class);
        intent.putExtra("ID",ID);
        intent.putExtra("IsEdit",false);
        startActivity(intent);
    }

    public void editPersonnelDetails(int ID){
        Intent intent = new Intent(this, AddPersonnelActivity.class);
        intent.putExtra("ID",ID);
        intent.putExtra("IsEdit", true);
        startActivity(intent);
    }

    //Gets the data to populate the listviews
    public void getProjectData(int ID) {
        //TODO Add in httpget for project peronnel and stories
    }

    class getProjectData extends AsyncTask<Integer, Void, JSONArray> {


        private Exception exception;

        protected void onPreExecute() {
            mProgressBar.setVisibility(View.VISIBLE);
        }

        protected JSONArray doInBackground(Integer... initials) {
            JSONArray response = null;
//            try {
//                URL url = new URL(PROJECTS_API_GETALL + initials);
//                HttpURLConnection conn = (HttpURLConnection) url.openConnection();
//                conn.setRequestMethod("GET");
//                // read the response
//                InputStream in = new BufferedInputStream(conn.getInputStream());
//                response = new JSONArray(convertStreamToString(in));
//            } catch (MalformedURLException e) {
//                Log.e(TAG, "MalformedURLException: " + e.getMessage());
//            } catch (ProtocolException e) {
//                Log.e(TAG, "ProtocolException: " + e.getMessage());
//            } catch (IOException e) {
//                Log.e(TAG, "IOException: " + e.getMessage());
//            } catch (Exception e) {
//                Log.e(TAG, "Exception: " + e.getMessage());
//            }


            //TODO remove these hardcoded values, pull through getProjects()
            // Defined Array values to show in ListView
            JSONObject project1 = new JSONObject();
            try {
                project1.put("ID", 1);
                project1.put("Name", "ANPR");
                project1.put("DateCreated", "2016-10-31 13:28:00");
                project1.put("Active", true);
            } catch (JSONException e) {
                e.printStackTrace();
            }

            JSONObject project2 = new JSONObject();
            try {
                project2.put("ID", "2");
                project2.put("Name", "CSC");
                project2.put("DateCreated", "2016-10-31 13:28:00");
                project2.put("Active", false);
            } catch (JSONException e) {
                e.printStackTrace();
            }

            stories = new JSONArray();
            stories.put(project1);
            stories.put(project2);

            personnel = new JSONArray();
            personnel.put(project1);
            personnel.put(project2);

            return response;
        }

        protected void onPostExecute(JSONArray response) {
            mProgressBar.setVisibility(View.GONE);

            ArrayList<String> valuesTable1 = new ArrayList<String>();
            for (int i = 0; i < stories.length(); i++) {
                JSONObject project = new JSONObject();
                try {
                    project = stories.getJSONObject(i);
//                String id=project.getString("ID");
                    String name = project.getString("Name");
//                String dateCreated = project.getString("DateCreated");
//                String active = project.getString("Active");
                    valuesTable1.add(name);
                    Log.d(name, "Output");
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }

            ArrayList<String> valuesTable2 = new ArrayList<String>();
            for (int i = 0; i < stories.length(); i++) {
                JSONObject project = new JSONObject();
                try {
                    project = stories.getJSONObject(i);
//                String id=project.getString("ID");
                    String name = project.getString("Name");
//                String dateCreated = project.getString("DateCreated");
//                String active = project.getString("Active");
                    valuesTable2.add(name);
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

            ArrayAdapter<String> storiesAdapter = new ArrayAdapter<String>(ProjectManagerActivity.this,
                    android.R.layout.simple_list_item_1, android.R.id.text1, valuesTable1);

            ArrayAdapter<String> personnelAdapter = new ArrayAdapter<String>(ProjectManagerActivity.this,
                    android.R.layout.simple_list_item_1, android.R.id.text1, valuesTable2);

            // Assign adapter to ListView
            mTable1.setAdapter(storiesAdapter);
            mTable2.setAdapter(personnelAdapter);
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
