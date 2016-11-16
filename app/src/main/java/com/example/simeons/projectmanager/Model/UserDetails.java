package com.example.simeons.projectmanager.Model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by SimeonS on 16/11/2016.
 */

public class UserDetails {

    @SerializedName("User")
    public String User;

    @SerializedName("Password")
    public String Password;

    public UserDetails(String user, String password) {
        this.User = user;
        this.Password = password;
    }

}
