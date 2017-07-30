using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TweetData
{
    public string message { get; private set; }
    public string user { get; private set; }
    public float time { get; private set; }
    public TweetData(string tweetText, User tweetUser, float timeFloat)
    {
        message = tweetText;
        user = tweetUser.username;
        time = timeFloat;
    }

}
