package com.example.simeons.projectmanager.Model;

/**
 * Created by SimeonS on 18/11/2016.
 */

public class WorkLog {

    public String ID;
    public String TaskID;
    public String PersonID;
    public String Person;
    public String Time;
    public String DateCreated;

    public WorkLog(String iD, String taskID, String personID, String person, String time, String dateCreated){

        this.ID = iD;
        this.TaskID = taskID;
        this.Person = person;
        this.PersonID = personID;
        this.Time = time;
        this.DateCreated = dateCreated;

    }

}
