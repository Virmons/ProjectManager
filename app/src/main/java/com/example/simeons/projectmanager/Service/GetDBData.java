package com.example.simeons.projectmanager.Service;

import android.app.IntentService;
import android.content.ContentValues;
import android.content.Intent;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

import org.json.JSONObject;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

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

import static com.example.simeons.projectmanager.Constants.PROJECTS_API;

/**
 * Created by SimeonS on 28/11/2016.
 */

public class GetDBData extends IntentService {

    String userID;
    String token;

    public GetDBData(){

        super("GetDBData");

    }

    @Override
    protected void onHandleIntent(Intent intent) {

        try{
            SQLiteDatabase projectDatabase = openOrCreateDatabase("ProjectManagerDB",MODE_PRIVATE,null);

            userID = intent.getStringExtra("UserID");
            token = intent.getStringExtra("Token");

            new GetProjectData().GetProjectData(userID, token, projectDatabase);
        }
        catch(Exception e){

            e.printStackTrace();
            Thread.currentThread().interrupt();

        }

    }
}

class GetProjectData{

    boolean downloaded = false;

    public void GetProjectData(String userID, final String token, final SQLiteDatabase projectDatabase)
    {
        String url = PROJECTS_API;
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

        GetProjectData.ProjectAPIEndpointInterface apiService =
                retrofit.create(GetProjectData.ProjectAPIEndpointInterface.class);

        Call<JsonArray> call = apiService.getProjects(Integer.parseInt(userID));
        call.enqueue(new Callback<JsonArray>() {
            @Override
            public void onResponse(Call<JsonArray> call, Response<JsonArray> response) {

                for (JsonElement element : response.body()) {

                    JsonObject elementObject = element.getAsJsonObject();

                    JsonArray sql = elementObject.getAsJsonArray("SQL");

                    JsonArray values = elementObject.getAsJsonArray("Array");

                    projectDatabase.execSQL("DROP TABLE IF EXISTS " + sql.get(0).getAsString());
                    projectDatabase.execSQL(sql.get(sql.size() - 1).getAsString());

                    ContentValues insertValues = new ContentValues();

                    for(JsonElement valueObject : values){

                        Map<String, Object> attributes = new HashMap<String, Object>();
                        Set<Map.Entry<String, JsonElement>> entrySet = valueObject.getAsJsonObject().entrySet();
                        for(Map.Entry<String,JsonElement> entry : entrySet){
                            insertValues.put(entry.getKey(), entry.getValue().getAsString());
                        }
                        projectDatabase.insert(sql.get(0).getAsString(),null,insertValues);

                    }

                    String a = "";
                }
                Cursor resultSet = projectDatabase.rawQuery("Select * from Project",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from ProjectPerson",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from Person",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from Story",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from Actor",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from Theme",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from Task",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from SprintTask",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from Sprint",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }

                resultSet = projectDatabase.rawQuery("Select * from WorkLog",null);
                resultSet.moveToFirst();
                switch(resultSet.getCount()) {
                    case 1:
                        String name = resultSet.getString(1);
                        break;
                    case 0:
                        String hereweAre = "Here!";
                        break;
                    default:
                        break;


                }
            }

            @Override
            public void onFailure(Call<JsonArray> call, Throwable t) {

//                toastToUI(getApplicationContext(), "Failed", Toast.LENGTH_LONG);

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
        Call<JsonArray> getProjects(@Path("userID") int userID);

//        @POST("authoriseToken")
//        Call<Object> authoriseToken(@Body String token);
    }
}