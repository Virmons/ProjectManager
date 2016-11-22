package com.example.simeons.projectmanager;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.example.simeons.projectmanager.Model.Message;
import com.example.simeons.projectmanager.Model.UserDetails;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.http.Body;
import retrofit2.http.POST;

import static com.example.simeons.projectmanager.Constants.LOGIN_API_LOGIN;

public class LoginActivity extends Activity  {
    Button mButtonLogin, mButtonCancel;
    EditText mEditTextUsername, mEditTextPassword;

    LoginTask loginTask;
    LoginTask authenticateTask;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        mButtonLogin = (Button) findViewById(R.id.button);
        mEditTextUsername = (EditText) findViewById(R.id.editText);
        mEditTextPassword = (EditText) findViewById(R.id.editText2);

        mButtonCancel = (Button) findViewById(R.id.button2);

        mButtonLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                mButtonLogin.setEnabled(false);

                UserDetails userDetails = new UserDetails(mEditTextUsername.getText().toString(), mEditTextPassword.getText().toString());

                if(Validate(userDetails)){
                    loginTask = new LoginTask();

                    loginTask.execute(userDetails);

                }
            }
        });

        mButtonCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        SharedPreferences sharedPref = getPreferences(Context.MODE_PRIVATE);
        String token = sharedPref.getString("ProjectManagerToken", "");
        if(!token.equals("")){

            mButtonLogin.setEnabled(false);
            authenticateTask = new LoginTask();
            authenticateTask.execute(token);
        }



    }

    public boolean Validate(UserDetails userDetails){

        boolean valid = true;

        if(userDetails.User.length() == 0 || userDetails.User.length() > 50 ){

            valid = false;

        }
        else if(userDetails.Password.length() == 0 || userDetails.Password.length() > 50){

            valid = false;

        }

        return valid;
    }

    class LoginTask extends AsyncTask<Object, Void, String> {

        String url;

        @Override
        protected String doInBackground(Object... params) {


            url = LOGIN_API_LOGIN;

            postToAPI(url, params[0]);
            return null;
        }

        protected void onPreExecute() {

            Toast.makeText(getApplicationContext(), "Redirecting...", Toast.LENGTH_SHORT).show();

        }

        protected void onPostExecute(String result) {

        }

    }

    public void toProjectOverview(String userID, String token) {

        Intent intent = new Intent(LoginActivity.this, ProjectActivity.class);
        intent.putExtra("UserID",Integer.parseInt(userID));
        intent.putExtra("Token", token);
        startActivity(intent);
        finish();

    }

    public void postToAPI(String url, final Object dataToPost)
    {
        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ssZ")
                .create();

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(url)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .build();

        LoginAPIEndpointInterface apiService =
                retrofit.create(LoginAPIEndpointInterface.class);

        if(dataToPost instanceof UserDetails) {
            Call<Message> call = apiService.authoriseUser((UserDetails) dataToPost);
            call.enqueue(new Callback<Message>() {
                @Override
                public void onResponse(Call<Message> call, Response<Message> response) {
//                    String body = response.body().toString();
//
//                    JSONObject message;
                    String type = response.body().Type;
                    String messageContent= response.body().Message;
                    String userID = response.body().UserID;
/*                    try {
                        message = new JSONObject(body);
                        type = message.getString("Type");
                        messageContent = message.getString("Message");
                        userID = message.getString("UserID");
                    }
                    catch(JSONException ex){
                        ex.printStackTrace();
                    }*/


                    switch (type) {
                        case "Authenticated":

                            SharedPreferences sharedPref = getPreferences(Context.MODE_PRIVATE);
                            SharedPreferences.Editor editor = sharedPref.edit();
                            editor.putString("ProjectManagerToken", messageContent);
                            editor.apply();
                            toProjectOverview(userID, messageContent);

                            break;
                        case "NonAuthenticated":

                            toastToUI(getApplicationContext(), messageContent, Toast.LENGTH_LONG);

                            break;
                        case "Error":

                            toastToUI(getApplicationContext(), messageContent, Toast.LENGTH_LONG);

                            break;
                        default:

                            toastToUI(getApplicationContext(), messageContent, Toast.LENGTH_LONG);

                            break;
                    }

                }

                @Override
                public void onFailure(Call<Message> call, Throwable t) {

                    toastToUI(getApplicationContext(), "Failed", Toast.LENGTH_LONG);

                }
            });
        }
        else if(dataToPost instanceof String){
            Call<Message> call = apiService.authoriseToken((String) dataToPost);
            call.enqueue(new Callback<Message>() {
                @Override
                public void onResponse(Call<Message> call, Response<Message> response) {

                    String type = response.body().Type;
                    String messageContent = response.body().Message;
                    String userID = response.body().UserID;


                    switch (type) {
                        case "Authenticated":

                            toProjectOverview(userID, dataToPost.toString());

                            break;
                        case "NonAuthenticated":

                            toastToUI(getApplicationContext(), messageContent, Toast.LENGTH_LONG);

                            break;
                        case "Error":

                            toastToUI(getApplicationContext(), messageContent, Toast.LENGTH_LONG);

                            break;
                    }

                    mButtonLogin.setEnabled(true);

                }

                @Override
                public void onFailure(Call<Message> call, Throwable t) {

                    Toast.makeText(getApplicationContext(), "Failed", Toast.LENGTH_LONG);

                }
            });
        }
    }

    public interface LoginAPIEndpointInterface {
        // Request method and URL specified in the annotation
        // Callback for the parsed response is the last parameter

//        @GET("users/{username}")
//        Call<User> getUser(@Path("username") String username);
//
//        @GET("group/{id}/users")
//        Call<List<User>> groupList(@Path("id") int groupId, @Query("sort") String sort);

        @POST("authoriseCredentials")
        Call<Message> authoriseUser(@Body UserDetails userDetails);

        @POST("authoriseToken")
        Call<Message> authoriseToken(@Body String token);
    }

    public void toastToUI(final Context context, final String message, final int duration){

        runOnUiThread(new Runnable() {
            @Override
            public void run() {

                Toast.makeText(context ,message ,duration).show();

            }
        });

    }

    @Override
    protected void onDestroy() {
        super.onDestroy();


    }

    //    Manual implementation, could not send data in body
//    public String postJSON(String url, String[] params, int timeout) {
//        HttpURLConnection connection = null;
//        try {
//            JSONObject postParams = new JSONObject();
//            try{
//                postParams.put("User", params[0]);
//                postParams.put("Password", params[1]);
//            }
//            catch( JSONException e){
//                e.printStackTrace();
//            }
//            String urlParams = postParams.toString();
//            byte[] bytesToSend = urlParams.getBytes();
//            URL u = new URL(url);
//            connection = (HttpURLConnection) u.openConnection();
//            connection.setRequestMethod("POST");
//            connection.setDoOutput(true);
//            connection.setDoInput(true);
//            connection.setUseCaches(false);
//            connection.setAllowUserInteraction(false);
//            connection.setConnectTimeout(timeout);
//            connection.setReadTimeout(timeout);
//            connection.connect();
//            //connection.getOutputStream().write(bytesToSend);
//            DataOutputStream printout = new DataOutputStream(connection.getOutputStream ());
//            printout.write(bytesToSend);
//            printout.flush ();
//            printout.close ();
//            int status = connection.getResponseCode();
//
//            Log.i("Status Code", Integer.toString(status));
//
//            switch (status) {
//                case 200:
//                case 201:
//                    BufferedReader br = new BufferedReader(new InputStreamReader(connection.getInputStream()));
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
//            if (connection != null) {
//                try {
//                    connection.disconnect();
//                } catch (Exception ex) {
//                    Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
//                }
//            }
//        }
//        return null;
//    }
}