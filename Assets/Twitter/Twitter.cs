using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using UnityEngine;

public class Twitter
{
    UserList userList;
    public List<TweetData> tweets { get; private set; }
    public class MyTwitterEvent : UnityEvent<TweetData> { }
    public MyTwitterEvent tweetEvent;

    float totalTime, nextTweet;
    const float tweetDelay = 1;
    public Twitter(Dictionary<string, Templates> dictionary, float mindelay, float maxdelay)
    {
        userList = new UserList(dictionary);
        tweets = new List<TweetData>();
        totalTime = 0;
            tweetEvent = new MyTwitterEvent();
        minDelay = mindelay;
        maxDelay = maxdelay;
    }
    public TweetData GetUsefulTweet(PokemonInfo args, float time)
    {
        return userList.randomUsefulTweet(args, time);
    }
    public TweetData GetUselessTweet(float time)
    {
        return userList.randomUselessTweet(time);
    }
    public void ClearTweets()
    {
        tweets = new List<TweetData>();
    }
    public float minDelay, maxDelay;
    public void Update(float dt, List<PokemonInfo> pokemonInformation)
    {
        totalTime += dt;
        if (totalTime > nextTweet)
        {
            //Random random = new Random();
            TweetData newTweet;
            if (Random.Range(0, 2) == 0 && pokemonInformation.Count != 0)
            {
                var pokemonTypes = pokemonInformation.Select((pi) => pi.data.name).Distinct().ToList();
                var randomPoke = pokemonTypes[Random.Range(0, pokemonTypes.Count)];
                var pokeOfType = pokemonInformation.Where((pi) => pi.data.name == randomPoke).ToList();
                int randomNumber = Random.Range(0, pokeOfType.Count);
                newTweet = this.GetUsefulTweet(pokeOfType[randomNumber], totalTime);
                tweets.Add(newTweet);
                if (Random.Range(0, 2) == 0)
                {
                    //pokemonInformation.Remove(pokemonInformation[randomNumber]);
                }
            }
            else
            {
                newTweet = this.GetUselessTweet(totalTime);
                tweets.Add(newTweet);
            }
            tweetEvent.Invoke(newTweet);
            nextTweet = totalTime + Random.Range(minDelay, maxDelay);
        }
    }
}
