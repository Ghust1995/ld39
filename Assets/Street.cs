using UnityEngine;
using UnityEngine.UI;

public struct StreetInfo
{
    public float position;
    public bool isMain;
    public string name;
}

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class Street : MonoBehaviour
{
    public Text streetName;
    public RectTransform rectTransform
    {
        get
        {
            return GetComponent<RectTransform>();
        }
    }

    public Image image
    {
        get
        {
            return GetComponent<Image>();
        }
    }
    public StreetInfo streetInfo;
    public void Setup(StreetInfo info) {
        streetInfo = info;
        streetName.text = info.name;

    }
}
