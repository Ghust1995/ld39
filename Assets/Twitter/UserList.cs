using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct Templates
{
    public List<string> usefulTemplates, uselessTemplates;
}

public class UserList
{
    List<User> useful, useless;
    public UserList(Dictionary<string, Templates> dictionary)
    {
            useful = new List<User>();
            useless = new List<User>();
        foreach (KeyValuePair<string, Templates> entry in dictionary)
        {
            User newUser = new User(entry.Key, entry.Value.usefulTemplates, entry.Value.uselessTemplates);
            if (entry.Value.uselessTemplates.Count != 0)
            {
                //Debug.Log("added new useless user " + newUser.username);
                useless.Add(newUser);
            }

            if (entry.Value.usefulTemplates.Count != 0)
            {
                //Debug.Log("added new useful user " + newUser.username);
                useful.Add(newUser);
            }
        }

    }
    public TweetData randomUsefulTweet(PokemonInfo args, float time)
    {
        return useful[Random.Range(0, useful.Count)].usefulTweet(args, time);
    }
    public TweetData randomUselessTweet(float time)
    {
        var sel = Random.Range(0, useless.Count);
        return useless[sel].uselessTweet(time);
    }

}
