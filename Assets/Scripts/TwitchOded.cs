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

    public int GameLength;
    public int StartLength;
    public int PauseLength;

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

    public Text LogRightText;
    public Text LogLeftText;

    public Text Message;

    public List<string> playersLeft;
    public List<string> playersRight;
    bool CurrentLeftRight = false;

    float StarGameTimer;
    float EndGameTimer;
    float NewGameTimer;

    void Start()
    {
        //Subscribe for events
        TwitchIrc.Instance.OnChannelMessage += OnChannelMessage;
        TwitchIrc.Instance.OnUserLeft += OnUserLeft;
        TwitchIrc.Instance.OnUserJoined += OnUserJoined;
        TwitchIrc.Instance.OnServerMessage += OnServerMessage;
        TwitchIrc.Instance.OnExceptionThrown += OnExceptionThrown;
        StartNewGame();
    }
    
    void EndGame()
    {
    }

    void StartNewGame()
    {
        StarGameTimer = Time.time + StartLength;
        EndGameTimer = StarGameTimer + GameLength;
        NewGameTimer = EndGameTimer + PauseLength;

         rightRock=0;
         rightScissors=0;
         rightPaper=0;

         leftRock=0;
         leftScissors=0;
         leftPaper=0;

        LogRightText.text = "";
        LogLeftText.text = "";


        rightRockText.text = rightRock.ToString();
        rightScissorsText.text = rightScissors.ToString();
        rightPaperText.text = rightPaper.ToString();
        leftRockText.text = leftRock.ToString();
        leftScissorsText.text = leftScissors.ToString();
        leftPaperText.text = leftPaper.ToString();

        //TwitchIrc.Instance.Message("New Game starts in " + (PauseLength + StarGameTimer ) + "seconds");

    }
    void Update()
    {
        if (StarGameTimer > Time.time) // before game
        {
            Message.text = "Game Starts in :\n" + (StarGameTimer - Time.time).ToString("F0");
        } else // game is runining
        {
            if (EndGameTimer > Time.time) // game in progress
            {
                Message.text = "Time Left : \n" + (EndGameTimer - Time.time).ToString("F2");
            } else // game over
            {
                Message.text = "Results:" + GetWinner();
                if (NewGameTimer < Time.time)
                {
                    StartNewGame();
                }
            }
        }

    }

    string GetWinner()
    {

        if (rightRock == 0 &&
         rightScissors == 0 &&
         rightPaper == 0 &&

         leftRock == 0 &&
         leftScissors == 0 &&
         leftPaper == 0)
        {
            TwitchIrc.Instance.Message("Results: no oneplayed!");
            return "No One Played!";
        }

        string rightWin="";
        if (rightRock > rightScissors)
        {
            if (rightRock > rightPaper)
            {
                rightWin = "r";
            } else
            {
                rightWin = "p";
            }
        } else
        {
            if (rightScissors > rightPaper)
            {
                rightWin = "s";
            }
            else
            {
                rightWin = "p";
            }
        }

        

        string leftWin="";
        // left
        if (leftRock > leftScissors)
        {
            if (leftRock > leftPaper)
            {
                leftWin = "r";
            }
            else
            {
                leftWin = "p";
            }
        }
        else
        {
            if (leftScissors > leftPaper)
            {
                leftWin = "s";
            }
            else
            {
                leftWin = "p";
            }
        }

        string winner = "";

        if (leftWin == "s")
        {
            if (rightWin == "s")
                winner = "tie";
            if (rightWin == "r")
                winner = "<color=red>right</color>";
            if (rightWin == "p")
                winner = "<color=blue>left</color>";
        }

        if (leftWin == "p")
        {
            if (rightWin == "s")
                winner = "<color=red>right</color>";
            if (rightWin == "r")
                winner = "<color=blue>left</color>";
            if (rightWin == "p")
                winner = "tie";
        }

        if (leftWin == "r")
        {
            if (rightWin == "s")
                winner = "<color=blue>left</color>";
            if (rightWin == "r")
                winner = "tie";
            if (rightWin == "p")
                winner = "<color=red>right</color>";
                    
        }




        string rps = "Left: R=" + leftRock + " P=" + leftPaper + "S=" + leftScissors + " Right R=" + rightRock + " P=" + rightPaper + " S=" + rightScissors;
        TwitchIrc.Instance.Message(rps);
        string win1 = "left: " + leftWin + " right:" + rightWin;
        string win2 = "Winner : " + winner;
        TwitchIrc.Instance.Message("Results:" + win1);
        TwitchIrc.Instance.Message(win2);
        string winners = win1 + "\n" + win2; 

        return winners;
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


        if ((StarGameTimer > Time.time) || (EndGameTimer < Time.time))
        {
            // game isn't running.
            return;
        }

        bool left = findPlayerGroup(channelMessageArgs.From);

        if (left) {
            LogLeftText.text += "<color=blue>" + channelMessageArgs.From + ":" + channelMessageArgs.Message[1].ToString().ToString()
                +"</color>\n";
        }
        else {
            LogRightText.text += "<color=red>" + channelMessageArgs.From + ":" + channelMessageArgs.Message[1].ToString().ToString()
               + "</color>\n";
        }

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

        TwitchIrc.Instance.Message("Welcome " + user + " You in the " + (CurrentLeftRight ? "Left":"Right") + " team");

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
        ChatText.text += "<b>" + "USER LEFT" + ":</b> " + userLeftArgs.User + "\n";
        Debug.Log("USER LEFT: " + userLeftArgs.User);

        playersLeft.Remove(userLeftArgs.User);
        playersRight.Remove(userLeftArgs.User);
    }

    //Receive exeption if something goes wrong
    private void OnExceptionThrown(Exception exeption)
    {
        Debug.Log(exeption);
    }
   
}
