package com.example.simeons.projectmanager;

/**
 * Created by SimeonS on 01/11/2016.
 */

public interface Constants {

    //Base URL for the API
    public static final String DATA_API_URL = "http://79.77.23.117:2099/api/";

    //Projects API URL
    public static final String PROJECTS_API = DATA_API_URL + "Projects/";

    //Add Project Request
    public static final int ADD_PROJECT_REQUEST = 1;

    //Edit Project Request
    public static final int EDIT_PROJECT_REQUEST = 2;

    //Login Attempt
    public static final String LOGIN_API_LOGIN = DATA_API_URL + "Login/";
}
