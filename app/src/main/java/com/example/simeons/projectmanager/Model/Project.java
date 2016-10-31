package com.example.simeons.projectmanager.Model;

/**
 * Created by SimeonS on 31/10/2016.
 */

public class Project {
    private String ID;
    private String ProjectName;
    private String DateCreated;
    private String Active;

public    Project(String ID,String ProjectName,String DateCreated,String Active){
        this.ID=ID;
        this.ProjectName=ProjectName;
        this.DateCreated=DateCreated;
        this.Active=Active;
    }
}
