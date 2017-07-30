using UnityEngine;
using System.Collections.Generic;

public class User
{
    public string username { get; private set; }
    public object profilePicture { get; private set; }
    List<string> usefulTemplates, uselessTemplates;
    public User(string name, List<string> userUsefulTemplates, List<string> userUselessTemplates)
    {
        username = name;
        usefulTemplates = userUsefulTemplates;
        uselessTemplates = userUselessTemplates;
    }
    public TweetData usefulTweet(PokemonInfo args, float time)
    {
        string message = usefulTemplates[Random.Range(0, usefulTemplates.Count)];
        message = string.Format(message, args.data.name, args.mapInfo.streetX.name, args.mapInfo.streetY.name);
        return new TweetData(message, this, time);
    }
    public TweetData uselessTweet(float time)
    {
        return new TweetData(uselessTemplates[Random.Range(0, uselessTemplates.Count)], this, time);
    }

}