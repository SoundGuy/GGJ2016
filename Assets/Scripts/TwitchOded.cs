using System;
using System.Collections.Generic;
using Irc;
using UnityEngine;
using UnityEngine.UI;

public class TwitchOded : MonoBehaviour
{
    //public InputField UsernameText;
    //public InputField TokenText;
    //public InputField ChannelText;

    public Text ChatText;
    public InputField MessageText;

    int rightRock;
    int rightScissors;
    int rightPaper;

    int leftRock;
    int leftScissors;
    int leftPaper;


    public Text rightRockText;
    public Text rightScissorsText;
    public Text rightPaperText;
    public Text leftRockText;
    public Text leftScissorsText;
    public Text leftPaperText;

    public Text rightPlayersText;
    public Text leftPlayersText;

    public List<string> playersLeft;
    public List<string> playersRight;
    bool CurrentLeftRight = false;

    void Start()
    {
        //Subscribe for events
        TwitchIrc.Instance.OnChannelMessage += OnChannelMessage;
        TwitchIrc.Instance.OnUserLeft += OnUserLeft;
        TwitchIrc.Instance.OnUserJoined += OnUserJoined;
        TwitchIrc.Instance.OnServerMessage += OnServerMessage;
        TwitchIrc.Instance.OnExceptionThrown += OnExceptionThrown;
    }
    

    //Send message
    public void MessageSend()
    {
        if (String.IsNullOrEmpty(MessageText.text))
            return;

        TwitchIrc.Instance.Message(MessageText.text);
        ChatText.text += "<b>" + TwitchIrc.Instance.Username + "</b>: " + MessageText.text +"\n";
        MessageText.text = "";
    }

    //Open URL
    public void GoUrl(string url)
    {
        Application.OpenURL(url);
    }

    //Receive message from server
    void OnServerMessage(string message)
    {
        ChatText.text += "<b>SERVER:</b> " + message + "\n";
        Debug.Log(message);
    }


    bool findPlayerGroup(string player) // reutrn true if left
    {
        if (playersLeft.Contains(player))
        {
            return true;
        }
        else if (playersRight.Contains(player))
        {
            
            return false;
        } else
        {
            addUser(player);
            return !CurrentLeftRight;
        }
            
    }

    //Receive username that has been left from channel 
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs)
    {
        Debug.Log("MESSAGE: " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        ChatText.text += "<b>" + channelMessageArgs.From + ":</b> " + channelMessageArgs.Message + "\n";

        bool left = findPlayerGroup(channelMessageArgs.From);
        switch  (channelMessageArgs.Message[1])
        {
            case 'r':
            case 'R':
                if (left)
                    leftRock++;
                else
                    rightRock++;                
                break;
            case 'p':
            case 'P':
                if (left)
                    leftPaper++;
                else
                    rightPaper++;
                break;
            case 's':
            case 'S':
                if (left)
                    leftScissors++;
                else
                    rightScissors++;
                break;
            default:
                break;
        }

        rightRockText.text = rightRock.ToString();
        rightScissorsText.text = rightScissors.ToString();
        rightPaperText.text = rightPaper.ToString();
        leftRockText.text = leftRock.ToString();
        leftScissorsText.text = leftScissors.ToString();
        leftPaperText.text = leftPaper.ToString();

        Debug.Log(channelMessageArgs.Message[1] + "  " + leftRock + "," + leftPaper + "," + leftScissors + " " + rightRock + "," + rightPaper + "," + rightScissors);

}

    void addUser(string user)
    {
        if (CurrentLeftRight)
        { // choose the one with less players
            playersLeft.Add(user);

        }
        else
        {
            playersRight.Add(user);
        }
        CurrentLeftRight = !CurrentLeftRight;

        rightPlayersText.text = "";
        foreach (string player in playersRight)
        {
            rightPlayersText.text += " " + player;
        }

        leftPlayersText.text = "";
        foreach (string playerL in playersLeft)
        {
            leftPlayersText.text += " " + playerL;
        }
    }

    //Get the name of the user who joined to channel 
    void OnUserJoined(UserJoinedEventArgs userJoinedArgs)
    {
        ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userJoinedArgs.User + "\n";
        Debug.Log("USER JOINED: " + userJoinedArgs.User);
        addUser(userJoinedArgs.User);

   


    }


    //Get the name of the user who left the channel.
    void OnUserLeft(UserLeftEventArgs userLeftArgs)
    {
        ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userLeftArgs.User + "\n";
        Debug.Log("USER JOINED: " + userLeftArgs.User);
    }

    //Receive exeption if something goes wrong
    private void OnExceptionThrown(Exception exeption)
    {
        Debug.Log(exeption);
    }
   
}
