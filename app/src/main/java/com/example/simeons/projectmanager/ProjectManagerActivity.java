package com.example.simeons.projectmanager;

import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.example.simeons.projectmanager.Model.Message;
import com.example.simeons.projectmanager.Model.Person;
import com.example.simeons.projectmanager.Model.Project;
import com.example.simeons.projectmanager.Model.Story;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import org.json.JSONArray;
import org.json.JSONException;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.http.Body;
import retrofit2.http.POST;

import static android.view.View.VISIBLE;
import static com.example.simeons.projectmanager.Constants.PROJECTS_API;
import static java.lang.Thread.sleep;

public class ProjectManagerActivity extends AppCompatActivity {

    private Button mAddProjectButton;
    private TextView mTable1Header;
    private TextView mTable2Header;
    private ListView mTable1;
    private ListView mTable2;
    private ProgressBar mProgressBar;
    private JSONArray stories;
    private JSONArray personnel;
    private List<String> values;
    private Boolean uploaded = false;
    private String token;
    private int userID;
    private EditText mProjectName;
    private Project project;

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
        mProjectName = (EditText) findViewById(R.id.projectName);

        ArrayList<String> valuesStory = new ArrayList<>();
        ArrayList<String> valuesPersonnel = new ArrayList<>();

        userID = getIntent().getIntExtra("UserID", 0);

        stories = new JSONArray();
        personnel = new JSONArray();

        token = getIntent().getStringExtra("Token");

        String projectString = getIntent().getStringExtra("Project");
        if(projectString != null){
            project = (new Gson()).fromJson(projectString,Project.class);
            if (Integer.parseInt(project.ID) != 0) {
                mProjectName.setText(project.ProjectName);

                mAddProjectButton.setText(R.string.btn_update_project);
                for (Story story : project.Stories) {

                    valuesStory.add(story.StoryName);
                }
                for (Person person : project.Personnel) {

                    valuesPersonnel.add(person.Name);
                }
                ArrayAdapter<String> adapter1 = new ArrayAdapter<>(ProjectManagerActivity.this,
                        android.R.layout.simple_list_item_1, android.R.id.text1, valuesStory);

                ArrayAdapter<String> adapter2 = new ArrayAdapter<>(ProjectManagerActivity.this,
                        android.R.layout.simple_list_item_1, android.R.id.text1, valuesPersonnel);

                // Assign adapter to ListView
                mTable1.setAdapter(adapter1);
                mTable2.setAdapter(adapter2);
            }
        }

        // ListView Item Click Listener
        mTable1.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view,
                                    int position, long id) {

                // ListView Clicked item value
                String itemValue = (String) mTable1.getItemAtPosition(position);

                try {
                    int ID = stories.getJSONObject(position).getInt("ID") - 1;
                    Boolean active = stories.getJSONObject(position).getBoolean("Active");
                    if (active) {
                        showStoryDetails(ID);
                    }
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                // Show Alert
                Toast.makeText(getApplicationContext(),
                        "Position :" + position + "  ListItem : " + itemValue, Toast.LENGTH_LONG)
                        .show();

            }

        });

        //ListViewItemHoldListener
        mTable1.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
                try {
                    int ID = stories.getJSONObject(position).getInt("ID") - 1;
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

                // ListView Clicked item value
                String itemValue = (String) mTable2.getItemAtPosition(position);

                try {
                    int ID = personnel.getJSONObject(position).getInt("ID");
                    Boolean active = personnel.getJSONObject(position).getBoolean("Active");
                    if (active) {
                        showPersonnelDetails(ID);
                    }
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                // Show Alert
                Toast.makeText(getApplicationContext(),
                        "Position :" + position + "  ListItem : " + itemValue, Toast.LENGTH_LONG)
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
    public String addProject(View view){
        String result = null;
        if(mAddProjectButton.getText().equals("Update")){
            //TODO Fire off update project API call
            new projectManagerAsync().execute(project);
        }
        else if(mAddProjectButton.getText().equals("Add Project")) {
            //TODO Fire off add project API call

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

    class projectManagerAsync extends AsyncTask<Project, Object, Void> {

        String data;

        @Override
        protected void onPreExecute() {

            mProgressBar.setVisibility(VISIBLE);

        }

        @Override
        protected Void doInBackground(Project... params) {

            postJsonData(PROJECTS_API,token, params[0]);
            while(!uploaded){

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


    public void postJsonData(String url, final String token, Project project)
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

        ProjectManagerActivity.ProjectManagerAPIEndpointInterface apiService =
                retrofit.create(ProjectManagerActivity.ProjectManagerAPIEndpointInterface.class);

        Call<Message> call = apiService.updateProject(project);
        call.enqueue(new Callback<Message>() {
            @Override
            public void onResponse(Call<Message> call, final Response<Message> response) {

                if(response.body().Message != null){

                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {

                            mProgressBar.setVisibility(View.GONE);
                            toastToUI(getApplicationContext(),response.body().Message,Toast.LENGTH_LONG);

                        }
                    });

                    uploaded = true;
                }


            }

            @Override
            public void onFailure(Call<Message> call, Throwable t) {

                toastToUI(getApplicationContext(), "Failed", Toast.LENGTH_LONG);

            }
        });

    }

    public interface ProjectManagerAPIEndpointInterface {

        @POST("updateProject")
        Call<Message> updateProject(@Body Project project);
    }

    public void toastToUI(final Context context, final String message, final int duration){

        runOnUiThread(new Runnable() {
            @Override
            public void run() {

                Toast.makeText(context ,message ,duration).show();

            }
        });
    }


}
