using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwitterApp : MonoBehaviour
{

    /*
    public class Twitter
    {
        public class MyTwitterEvent : UnityEvent<TweetData> { }

        public Twitter()
        {
            tweetEvent = new MyTwitterEvent();
        }
        public MyTwitterEvent tweetEvent;
        public class TweetData
        {
            public string user;
            public string message;
            public float time;
        }
        float totalTime = 0.0f;
        float nextTweet = 1.0f;
        float tweetDelay = 3.0f;
        public List<TweetData> allTweets = new List<TweetData>();

        public void Update(float dt, List<PokemonInfo> pokemonInformation)
        {
            totalTime += dt;
            if (totalTime > nextTweet)
            {
                var newTweet = new TweetData()
                {
                    user = "donald",
                    message = "hello world " + allTweets.Count,
                    time = totalTime,
                };
                tweetEvent.Invoke(newTweet);
                allTweets.Add(newTweet);
                nextTweet = totalTime + tweetDelay;
                //Debug.Log("next tweet " + nextTweet);
            }
        }
    }
    */
    


    public List<TwitterUserData> TwitterUsers;

    public Twitter twitter;
    public Tweet tweetPrefab;
    public GameObject tweetContainer;
    public float minDelay, maxDelay;
    // Use this for initialization
    void Start()
    {
        var twitterDict = new Dictionary<string, Templates>();
        TwitterUsers.ForEach((user) =>
            twitterDict.Add(user.name, new Templates()
            {
                usefulTemplates = user.useful,
                uselessTemplates = user.useless,
            }));
        twitter = new Twitter(
            twitterDict,
            minDelay, 
            maxDelay);
        twitter.tweetEvent.AddListener(OnNewTweet);
    }

    private void Update()
    {
        twitter.Update(Time.deltaTime, FindObjectOfType<PokemongoApp>().pokemonContainer.SpawnedPokemon);
    }

    void OnNewTweet(TweetData tweetData)
    {
        Tweet tweetGO = Instantiate(tweetPrefab, tweetContainer.transform);
        tweetGO.Setup(tweetData);
        tweetGO.transform.SetSiblingIndex(0);
        if (FindObjectOfType<Cellphone>().selectedApp == App.Twitter)
        {
            FindObjectOfType<SfxManager>().PlayTweet();
        }
    }
}
