package com.example.simeons.projectmanager.Model;

/**
 * Created by SimeonS on 18/11/2016.
 */

public class Task {

    public String ID;
    public String Description;
    public String DateCreated;
    public String CreatedBy;
    public String TimeTaken;
    public String TimeEstimate;
    public String StoryID;
    public String Active;
    public String Complete;
    public WorkLog[] WorkLogs;

    public Task(String iD, String description, String dateCreated, String createdBy, String timeTaken, String timeEstimate, String storyID, String active, String complete, WorkLog[] workLogs) {

        this.ID = iD;
        this.Description = description;
        this.DateCreated = dateCreated;
        this.CreatedBy = createdBy;
        this.TimeTaken = timeTaken;
        this.TimeEstimate = timeEstimate;
        this.StoryID = storyID;
        this.Active = active;
        this.Complete = complete;
        this.WorkLogs = workLogs;

    }

}
