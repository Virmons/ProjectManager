package com.example.simeons.projectmanager;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.net.UnknownHostException;
import java.util.logging.Level;
import java.util.logging.Logger;

import static com.example.simeons.projectmanager.Constants.LOGIN_API_LOGIN;

public class LoginActivity extends Activity  {
    Button mButtonLogin, mButtonCancel;
    EditText mEditTextUsername, mEditTextPassword;

    TextView tx1;

    String[] userDetails;
    loginTask myLoginTask;

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
                if(Validate()) {

                    userDetails = new String[]{mEditTextUsername.getText().toString(), mEditTextPassword.getText().toString()};

                    myLoginTask = new loginTask();

                    myLoginTask.execute(userDetails);
                }
            }
        });

        mButtonCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });


    }

    public boolean Validate(){
        return true;
    }

    class loginTask extends AsyncTask<String, Void, Integer> {

        String url;

        @Override
        protected Integer doInBackground(String... params) {


            url = LOGIN_API_LOGIN;

            postJSON(url, params, 30000);
            return null;
        }

        protected void onPreExecute() {

            Toast.makeText(getApplicationContext(), "Redirecting...", Toast.LENGTH_SHORT).show();

        }

        protected void onPostExecute(Integer result) {

            Intent intent = new Intent(LoginActivity.this, ProjectActivity.class);
            startActivity(intent);
            finish();

        }

    }

    public String postJSON(String url, String[] params, int timeout) {
        HttpURLConnection connection = null;
        try {
            JSONObject postParams = new JSONObject();
            try{
                postParams.put("User", params[0]);
                postParams.put("Password", params[1]);
            }
            catch( JSONException e){
                e.printStackTrace();
            }
            String urlParams = postParams.toString();
            byte[] bytesToSend = urlParams.getBytes();
            URL u = new URL(url);
            connection = (HttpURLConnection) u.openConnection();
            connection.setRequestMethod("POST");
            connection.setDoOutput(true);
            connection.setDoInput(true);
            connection.setUseCaches(false);
            connection.setAllowUserInteraction(false);
            connection.setConnectTimeout(timeout);
            connection.setReadTimeout(timeout);
            connection.connect();
            //connection.getOutputStream().write(bytesToSend);
            DataOutputStream printout = new DataOutputStream(connection.getOutputStream ());
            printout.write(bytesToSend);
            printout.flush ();
            printout.close ();
            int status = connection.getResponseCode();

            Log.i("Status Code", Integer.toString(status));

            switch (status) {
                case 200:
                case 201:
                    BufferedReader br = new BufferedReader(new InputStreamReader(connection.getInputStream()));
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
            if (connection != null) {
                try {
                    connection.disconnect();
                } catch (Exception ex) {
                    Logger.getLogger(getClass().getName()).log(Level.SEVERE, null, ex);
                }
            }
        }
        return null;
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();


    }
}