package com.example.simeons.projectmanager;

/**
 * Created by SimeonS on 01/11/2016.
 */

public interface Constants {

    //Base URL for the API
    public static final String DATA_API_URL = "http://wolfwebtest1:2099/api/";

    //Projects Get Projects by UserInitials URL
    public static final String PROJECTS_API_GETALL = "http://79.77.23.117:2099/api/Projects/getAllProjects/";

    //Projects Add Project
    public static final String PROJECTS_API_ADD = "http://wolfwebtest1:2099/api/Projects/addProject/";

    //Add Project Request
    public static final int ADD_PROJECT_REQUEST = 1;

    //Edit Project Request
    public static final int EDIT_PROJECT_REQUEST = 2;

    //Login Attempt
    public static final String LOGIN_API_LOGIN = "http://79.77.23.117:2099/api/Login/authoriseCredentials/";
}
