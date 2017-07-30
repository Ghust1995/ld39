using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new twitter user", menuName = "Twitter/NewUser", order = 1)]
public class TwitterUserData : ScriptableObject {
    public List<string> useful;
    public List<string> useless;
}
