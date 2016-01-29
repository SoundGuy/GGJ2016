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

    public Image rightImage;
    public Image leftImage;
    public Image arrowImage;

    public Sprite LeftRockSprite;
    public Sprite LeftPaperSprite;
    public Sprite LeftScissorsSprite;


    public Sprite RightRockSprite;
    public Sprite RightPaperSprite;
    public Sprite RightScissorsSprite;

    public Sprite blankSprite;


    public Sprite LeftArrowSprite;
    public Sprite RightArrowSprite;


    bool postedWinner;

    public AudioSource music;
    public GameObject iconParticlePF;
    public GameObject TextParticlePF;

    public GameObject HamletPF;

    public Hamlet hamlet;

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

        postedWinner = false;

        rightRockText.text = rightRock.ToString();
        rightScissorsText.text = rightScissors.ToString();
        rightPaperText.text = rightPaper.ToString();
        leftRockText.text = leftRock.ToString();
        leftScissorsText.text = leftScissors.ToString();
        leftPaperText.text = leftPaper.ToString();

        leftImage.sprite = blankSprite;
        rightImage.sprite = blankSprite;
        arrowImage.sprite = blankSprite;
        music.Play();
        if (TwitchIrc.Instance.isActiveAndEnabled)
        {
           TwitchIrc.Instance.Message("New Game starts in " + (PauseLength + StartLength) + "seconds");
        }

    }

    void updateArrow()
    {
        string winner = calcWinner(getLeftWinner(), getrightWinner());
        if (winner == "right")
            arrowImage.sprite = RightArrowSprite;
        else if (winner == "left")
            arrowImage.sprite = LeftArrowSprite;
        else
            arrowImage.sprite = blankSprite;
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

                setWinnerImageLeft(getLeftWinner());
                setWinnerImageRight(getrightWinner());
                updateArrow();

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

    string getLeftWinner()
    {
        return GetSideWInner(leftRock, leftScissors, leftPaper);
    }
    string getrightWinner()
    {
        return GetSideWInner(rightRock, rightScissors, rightPaper);
    }

    string GetSideWInner(int r, int s, int p)
    {
        if (r == 0 && s == 0 && p == 0)
            return "b";
        if (r > s)
        {
            if (r > p)
            {
                return "r";
            } else
            {
                return "p";
            }
        } else
        {
            if (s > p)
            {
                return "s";
            }
            else
            {
                return "p";
            }
        }
    }


    void setWinnerImageLeft(string rps)
    {

        //todo make switch case?
        if (rps == "r")
        {
            leftImage.sprite = LeftRockSprite;
        }
        if (rps == "p")
        {
            leftImage.sprite = LeftPaperSprite;
        }
        if (rps == "s")
        {
            leftImage.sprite = LeftScissorsSprite;
        }
        if (rps == "b")
        {
            leftImage.sprite = blankSprite;
        }

        
    }

    void setWinnerImageRight(string rps)
    {

        //todo make switch case?
        if (rps == "r")
        {
            rightImage.sprite = RightRockSprite;
        }
        if (rps == "p")
        {
            rightImage.sprite = RightPaperSprite;
        }
        if (rps == "s")
        {
            rightImage.sprite = RightScissorsSprite;
        }
        if (rps == "b")
        {
            rightImage.sprite = blankSprite;
        }


    }

    string colorifyWinner(string winner)
    {
        if (winner == "right")
            return "<color=red>right</color>";
        if (winner == "left")
            return "<color=blue>left</color>";
        return winner;
    }
        

    string calcWinner(string leftWin, string rightWin) // left true
    {
        string winner = "";
        if (leftWin == "s")
        {
            if (rightWin == "s")
                winner = "tie";
            if (rightWin == "r")
                winner = "right";
            if (rightWin == "p")
                winner = "left";
        }

        if (leftWin == "p")
        {
            if (rightWin == "s")
                winner = "right";
            if (rightWin == "r")
                winner = "left";
            if (rightWin == "p")
                winner = "tie";
        }

        if (leftWin == "r")
        {
            if (rightWin == "s")
                winner = "left";
            if (rightWin == "r")
                winner = "tie";
            if (rightWin == "p")
                winner = "right";

        }

        return winner;
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
            //TwitchIrc.Instance.Message("Results: no oneplayed!");
            return "No One Played!";
        }

        string rightWin=getrightWinner();

        setWinnerImageRight(rightWin);
        

        string leftWin= getLeftWinner();
        setWinnerImageLeft(leftWin);

        string winner = calcWinner(leftWin, rightWin);
        string colorWinner = colorifyWinner(winner);





        string rps = "Left: R=" + leftRock + " P=" + leftPaper + "S=" + leftScissors + " Right R=" + rightRock + " P=" + rightPaper + " S=" + rightScissors;
        //TwitchIrc.Instance.Message(rps);
        string win1 = "left: " + leftWin + " right:" + rightWin;
        string win2 = "Winner : " + colorWinner;
       // TwitchIrc.Instance.Message("Results:" + win1);
        //TwitchIrc.Instance.Message(win2);
        string winners = win1 + "\n" + win2;

        if (postedWinner == false)
            postWinner(rps,win1,"winner: " + winner);
        return winners;
    }

    void postWinner(string rps, string win1, string win2)
    {
        if (postedWinner == true)
            return;

        postedWinner = true;
        TwitchIrc.Instance.Message(rps);       
        TwitchIrc.Instance.Message("Results:" + win1);
        TwitchIrc.Instance.Message(win2);


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

    void MakeParticle(Image parent, Sprite sprite)
    {
        GameObject go = (GameObject) Instantiate(iconParticlePF, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        Vector2 pos = go.GetComponent<RectTransform>().position;
        pos.x = parent.GetComponent<RectTransform>().position.x;
        pos.y = parent.GetComponent<RectTransform>().position.y;
        go.GetComponent<RectTransform>().position = pos;
        go.GetComponent<Image>().sprite = sprite;

    }

    void MakeParticleHamlet(Image parent)
    {
        GameObject go = (GameObject)Instantiate(HamletPF, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        Vector2 pos = go.GetComponent<RectTransform>().position;
        pos.x = parent.GetComponent<RectTransform>().position.x;
        pos.y = parent.GetComponent<RectTransform>().position.y+80;
        go.GetComponent<RectTransform>().position = pos;
        //go.GetComponent<Image>().sprite = HamletSprite;

    }

    void MakeParticleText(Image parent, string word)
    {
        GameObject go = (GameObject)Instantiate(TextParticlePF, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        Vector2 pos = go.GetComponent<RectTransform>().position;
        pos.x = parent.GetComponent<RectTransform>().position.x;
        pos.y = parent.GetComponent<RectTransform>().position.y+10;
        go.GetComponent<RectTransform>().position = pos;
        go.GetComponent<Text>().text = word;

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


        Debug.Log("'" + channelMessageArgs.Message + "'");
        char c = channelMessageArgs.Message[0];
        if (c == ':')
            c = channelMessageArgs.Message[1];


        if (left) {
            LogLeftText.text += "<color=blue>" + channelMessageArgs.From + ":" + c
                +"</color>\n";
        }
        else {
            LogRightText.text += "<color=red>" + channelMessageArgs.From + ":" + c 
               + "</color>\n";
        }

        int hamletScore = 1;
        string[] words = channelMessageArgs.Message.Split(' ');
        MakeParticleText(left ? leftImage : rightImage, words[0]);

        if (hamlet.IsHamlet(words[0]))
        {
            MakeParticleHamlet(left ? leftImage : rightImage);
            hamletScore = 10;
        }

        /*
        string upperC = c.ToString().ToUpper(); 
        if (upperC != "R" && upperC != "P" && upperC !="S") {
            
        }*/


        switch  (c)
        {
            case 'r':
            case 'R':
                if (left)
                {
                    leftRock+= hamletScore;
                    MakeParticle(leftImage, LeftRockSprite);
                    
                }
                else
                {
                    rightRock+= hamletScore;
                    MakeParticle(rightImage, RightRockSprite);
                }
                break;
            case 'p':
            case 'P':
                if (left)
                {
                    leftPaper+= hamletScore;
                    MakeParticle(leftImage, LeftPaperSprite);
                }
                else
                {
                    rightPaper+= hamletScore;
                    MakeParticle(rightImage, RightPaperSprite);
                }
                break;
            case 's':
            case 'S':
                if (left)
                {
                    leftScissors+= hamletScore;
                    MakeParticle(leftImage, LeftScissorsSprite);
                }
                else
                {

                    rightScissors+= hamletScore;
                    MakeParticle(rightImage, RightScissorsSprite);
                }
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

        Debug.Log(c + "  " + leftRock + "," + leftPaper + "," + leftScissors + " " + rightRock + "," + rightPaper + "," + rightScissors);

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
