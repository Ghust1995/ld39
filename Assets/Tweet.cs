using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tweet : MonoBehaviour {

    public Text usernameText;
    public Text tweetText;
    public Image iconImage;
    public Sprite iconSprite;
    public TwitterApp.Twitter.TweetData tweetData;
    public string username
    {
        get
        {
            return tweetData.user;
        }
    }
    public string tweet
    {
        get
        {
            return tweetData.message;
        }
    }

    public void Setup(TwitterApp.Twitter.TweetData tweetData)
    {
        this.tweetData = tweetData;
        iconSprite = Resources.Load<Sprite>("usericons/" + username);
        usernameText.text = "@" + username;
        tweetText.text = tweet;
        iconImage.sprite = iconSprite;
    }
	
}
