package com.example.simeons.projectmanager.Model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by SimeonS on 16/11/2016.
 */

public class Message {

    @SerializedName("Message")
    public String Message;

    @SerializedName("Type")
    public String Type;

    public Message(String message, String type ) {
        this.Message = message;
        this.Type = type;
    }

}
