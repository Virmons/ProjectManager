package com.example.simeons.projectmanager.Model;

/**
 * Created by SimeonS on 10/11/2016.
 */

public class Person {

    private String ID;
    private String Name;
    private String Initials;
    private String DateCreated;
    private String Administrator;
    private String Active;

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
