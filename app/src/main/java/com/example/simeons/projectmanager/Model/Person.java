package com.example.simeons.projectmanager.Model;

/**
 * Created by SimeonS on 10/11/2016.
 */

public class Person {

    public String ID;
    public String Name;
    public String Initials;
    public String DateCreated;
    public String Administrator;
    public String Active;

    public Person(String iD, String name, String initials, String dateCreated, String administrator, String active)
    {
        this.ID = iD;
        this.Name = name;
        this.Initials = initials;
        this.DateCreated = dateCreated;
        this.Administrator = administrator;
        this.Active = active;
    }
}
