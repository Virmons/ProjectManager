package com.example.simeons.projectmanager.Model;

/**
 * Created by SimeonS on 10/11/2016.
 */

public class Story {

    public String ID;
    public String StoryName;
    public String DateCreated;
    public String Theme;
    public String Actor;
    public String IWantTo;
    public String SoThat;
    public String Notes;
    public String Priority;
    public String Estimate;
    public String TimeEstimate;
    public String PercentageCompletion;
    public String Status;
    public String CreatedBy;
    public String ProjectID;
    public String SprintID;
    public String Active;
    public String LastEdited;
    public String LastEditedBy;

    public Story(String iD, String storyName, String dateCreated, String theme, String actor, String iWantTo, String soThat, String notes, String priority, String estimate, String timeEstimate, String percentageCompletion, String status, String createdBy, String projectID, String sprintID, String active, String lastEdited, String lastEditedBy){
        this.ID = iD;
        this.StoryName = storyName;
        this.DateCreated = dateCreated;
        this.Theme = theme;
        this.Actor = actor;
        this.IWantTo = iWantTo;
        this.SoThat = soThat;
        this.Notes = notes;
        this.Priority = priority;
        this.Estimate = estimate;
        this.TimeEstimate = timeEstimate;
        this.PercentageCompletion = percentageCompletion;
        this.Status = status;
        this.CreatedBy = createdBy;
        this.ProjectID = projectID;
        this.SprintID = sprintID;
        this.Active = active;
        this.LastEdited = lastEdited;
        this.LastEditedBy = lastEditedBy;
    }

}
