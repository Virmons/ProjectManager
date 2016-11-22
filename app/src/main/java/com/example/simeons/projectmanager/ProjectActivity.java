package com.example.simeons.projectmanager;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.Toast;

import com.example.simeons.projectmanager.Model.Project;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.IOException;
import java.util.ArrayList;

import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.http.GET;
import retrofit2.http.Path;

import static android.view.View.VISIBLE;
import static com.example.simeons.projectmanager.Constants.ADD_PROJECT_REQUEST;
import static com.example.simeons.projectmanager.Constants.EDIT_PROJECT_REQUEST;
import static com.example.simeons.projectmanager.Constants.PROJECTS_API;
import static java.lang.Thread.sleep;

public class ProjectActivity extends Activity {

    ListView listView;
    ProgressBar progressBar;
    boolean downloaded = false;
    ArrayList<Project> projects = new ArrayList<>();
    ArrayList<String> values = new ArrayList<>();
    private String token;
    int userID;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_project);

        // Get ListView object from xml
        listView = (ListView) findViewById(R.id.list);
        progressBar = (ProgressBar) findViewById(R.id.ProjectProgressBar);
        userID = getIntent().getIntExtra("UserID", 0);
        token = getIntent().getStringExtra("Token");

        // ListView Item Click Listener
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {

                // ListView Clicked item value
                String itemValue = (String) listView.getItemAtPosition(position);

                        int ID = Integer.parseInt(projects.get(position).ID) -1;
                        Boolean active = Boolean.parseBoolean(projects.get(position).Active);
                        if (active) {
                            showProjectDetails(ID);
                        }

                    // Show Alert
                toastToUI(getApplicationContext(),
                            "Position :" + position + "  ListItem : " + itemValue, Toast.LENGTH_LONG);

            }

        });

        //ListViewItemHoldListener
        listView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {

                        int ID = Integer.parseInt(projects.get(position).ID) - 1;
                        editProject(ID);

                return true;
            }
        });

        if(!(token.equals(""))){
            String[] inParams = new String[2];
            inParams[0] = token;
            inParams[1] = String.valueOf(userID);
            new getProjectData().execute(inParams);

        }


    }

    public void addNewProject(View view) {

        Intent intent = new Intent(ProjectActivity.this, ProjectManagerActivity.class);
        intent.putExtra("UserID",userID);
        intent.putExtra("Token", token);
        startActivityForResult(intent, ADD_PROJECT_REQUEST);

    }

    public void showProjectDetails(int ID) {

        Intent intent = new Intent(ProjectActivity.this, ProjectInProgressActivity.class);
        intent.putExtra("ID", ID);
        intent.putExtra("Project",(new Gson()).toJson(projects.get(ID)));
        intent.putExtra("Token", token);
        intent.putExtra("UserID",userID);
        startActivity(intent);

    }

    public void editProject(int ID) {
        Intent intent = new Intent(ProjectActivity.this, ProjectManagerActivity.class);
        intent.putExtra("ID", ID);
        intent.putExtra("Project",(new Gson()).toJson(projects.get(ID)));
        intent.putExtra("Token", token);
        intent.putExtra("UserID",userID);
        startActivityForResult(intent, EDIT_PROJECT_REQUEST);
    }

    class getProjectData extends AsyncTask<Object, Object, Void> {

        @Override
        protected void onPreExecute() {

            progressBar.setVisibility(VISIBLE);

        }

        @Override
        protected Void doInBackground(Object... params) {

            getJSONData(PROJECTS_API,params[0].toString(),params[1].toString());
            while(!downloaded){

                try {
                    sleep(1);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }

            }
            return null;
        }

        protected void onPostExecute(Void result){

        }
    }

    public void getJSONData(String url, final String token, String userID)
    {
        OkHttpClient.Builder httpClient = new OkHttpClient.Builder();
        httpClient.addInterceptor(new Interceptor() {
            @Override
            public okhttp3.Response intercept(Chain chain) throws IOException {
                Request original = chain.request();

                // Request customization: add request headers
                Request.Builder requestBuilder = original.newBuilder()
                        .addHeader("Authorization", token);

                Request request = requestBuilder.build();
                return chain.proceed(request);
            }
        });

        OkHttpClient client = httpClient.build();

        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ssZ")
                .create();

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(url)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .client(client)
                .build();

        ProjectActivity.ProjectAPIEndpointInterface apiService =
                retrofit.create(ProjectActivity.ProjectAPIEndpointInterface.class);

        Call<Project[]> call = apiService.getProjects(Integer.parseInt(userID));
        call.enqueue(new Callback<Project[]>() {
            @Override
            public void onResponse(Call<Project[]> call, Response<Project[]> response) {

                for (Project project : response.body()) {
                    values.add(project.ProjectName);
                    projects.add(project);
                }

                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {

                            progressBar.setVisibility(View.GONE);

                            ArrayAdapter<String> adapter = new ArrayAdapter<>(ProjectActivity.this,
                                    android.R.layout.simple_list_item_1, android.R.id.text1,values);


                            // Assign adapter to ListView
                            listView.setAdapter(adapter);
                        }
                    });

                downloaded = true;

            }

            @Override
            public void onFailure(Call<Project[]> call, Throwable t) {

                toastToUI(getApplicationContext(), "Failed", Toast.LENGTH_LONG);

            }
        });

    }

    public interface ProjectAPIEndpointInterface {
        // Request method and URL specified in the annotation
        // Callback for the parsed response is the last parameter

//        @GET("users/{username}")
//        Call<User> getUser(@Path("username") String username);
//
//        @GET("group/{id}/users")
//        Call<List<User>> groupList(@Path("id") int groupId, @Query("sort") String sort);

        @GET("getAllProjects/{userID}")
        Call<Project[]> getProjects(@Path ("userID") int userID);

//        @POST("authoriseToken")
//        Call<Object> authoriseToken(@Body String token);
    }

    public void toastToUI(final Context context, final String message, final int duration){

        runOnUiThread(new Runnable() {
            @Override
            public void run() {

                Toast.makeText(context ,message ,duration).show();

            }
        });
    }
    //      //Old getJSON
//    public String getJSON(String url, int timeout) {
//        HttpURLConnection c = null;
//        try {
//            URL u = new URL(url);
//            c = (HttpURLConnection) u.openConnection();
//            c.setRequestMethod("GET");
//            c.setRequestProperty("Content-length", "0");
//            c.setUseCaches(false);
//            c.setAllowUserInteraction(false);
//            c.setConnectTimeout(timeout);
//            c.setReadTimeout(timeout);
//            c.connect();
//            int status = c.getResponseCode();
//
//            Log.i("Status Code", Integer.toString(status));
//
//            switch (status) {
//                case 200:
//                case 201:
//                    BufferedReader br = new BufferedReader(new InputStreamReader(c.getInputStream()));
//                    StringBuilder sb = new StringBuilder();
//                    String line;
//                    while ((line = br.readLine()) != null) {
//                        sb.append(line.concat("\n"));
//                    }
//                    br.close();
//                    return sb.toString();
//                case 404:
//                    System.out.println("404");
//
//                    runOnUiThread(new Runnable() {
//                        @Override
//                        public void run() {
//
//                            Toast.makeText(getApplicationContext(), "User Not Found", Toast.LENGTH_SHORT);
//
//                        }
//                    });
//                    return null;
//                case 501:
//                    System.out.println("501");
//
//                    runOnUiThread(new Runnable() {
//                        @Override
//                        public void run() {
//
//
//                            Toast.makeText(getApplicationContext(), "501 Error", Toast.LENGTH_SHORT);
//                        }
//                    });
//                    return null;
//                default:
//                    break;
//            }
//
//        } catch (UnknownHostException e) {
//            runOnUiThread(new Runnable() {
//                @Override
//                public void run() {
//
//                    Toast.makeText(getApplicationContext(), "Check your network connection", Toast.LENGTH_SHORT);
//
//                }
//            });
//        }
//        catch (MalformedURLException ex) {
//            Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
//        } catch (ProtocolException e) {
//            e.printStackTrace();
//        } catch (IOException ex) {
//            Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
//        } finally {
//            if (c != null) {
//                try {
//                    c.disconnect();
//                } catch (Exception ex) {
//                    Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
//                }
//            }
//        }
//        return null;
//    }


//    Old AsyncTask
//    class getProjectData extends AsyncTask<Void,Void,Integer> {
//
//        String data;
//
//        @Override
//        protected void onPreExecute() {
//
//            progressBar.setVisibility(VISIBLE);
//
//        }
//
//        @Override
//        protected Integer doInBackground(Void... params) {
//            data = getJSON(url,30000);
//            try {
//
//                projects = new JSONArray(data);
//
//            }
//            catch(JSONException je){
//
//                Toast.makeText(getApplicationContext(), "Error converting input string to JSONArray", Toast.LENGTH_SHORT);
//
//            }
//
//            for (int i = 0; i < projects.length(); i++) {
//                JSONObject project = new JSONObject();
//                try {
//                    project = projects.getJSONObject(i);
////                String id=project.getString("ID");
//                    String name = project.getString("ProjectName");
////                String dateCreated = project.getString("DateCreated");
////                String active = project.getString("Active");
//                    values.add(name);
//                    Log.d(name, "Output");
//                } catch (JSONException e) {
//                    e.printStackTrace();
//                }
//            }
//
//
//            return null;
//        }
//
//        protected void onPostExecute(Integer result){
//
//            progressBar.setVisibility(View.GONE);
//
//            ArrayAdapter<String> adapter = new ArrayAdapter<String>(ProjectActivity.this,
//                    android.R.layout.simple_list_item_1, android.R.id.text1, values);
//
//
//            // Assign adapter to ListView
//            listView.setAdapter(adapter);
//
//            //        new getProjectData().execute("SS");
//
//            // ListView Item Click Listener
//            listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
//                @Override
//                public void onItemClick(AdapterView<?> parent, View view,
//                                        int position, long id) {
//
//                    // ListView Clicked item index
//                    int itemPosition = position;
//
//                    // ListView Clicked item value
//                    String itemValue = (String) listView.getItemAtPosition(position);
//
//                    try {
//                        int ID = projects.getJSONObject(position).getInt("ID");
//                        Boolean active = projects.getJSONObject(position).getBoolean("Active");
//                        if (active == true) {
//                            showProjectDetails(ID);
//                        }
//                    } catch (JSONException e) {
//                        e.printStackTrace();
//                    }
//                    // Show Alert
//                    Toast.makeText(getApplicationContext(),
//                            "Position :" + itemPosition + "  ListItem : " + itemValue, Toast.LENGTH_LONG)
//                            .show();
//
//                }
//
//            });
//
//            //ListViewItemHoldListener
//            listView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
//                @Override
//                public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
//                    try {
//                        int ID = projects.getJSONObject(position).getInt("ID");
//                        editProject(ID);
//                    } catch (JSONException e) {
//                        e.printStackTrace();
//                    }
//                    return true;
//                }
//            });
//
//        }
//    }
}