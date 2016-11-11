package com.example.simeons.projectmanager.Model;

/**
 * Created by SimeonS on 31/10/2016.
 */

public class Project {
    private String ID;
    private String ProjectName;
    private String DateCreated;
    private String Active;
    private Story[] Stories;
    private Person[] Personnel;

public Project(String iD,String projectName,String dateCreated,String active, Story[] stories, Person[] personnel){
    this.ID=iD;
    this.ProjectName=projectName;
    this.DateCreated=dateCreated;
    this.Active=active;
    this.Stories = stories;
    this.Personnel = personnel;
    }
}
