using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Map
{


    public struct StreetCrossing
    {
        public StreetInfo streetX;
        public StreetInfo streetY;
    }
    public List<StreetInfo> StreetsX = new List<StreetInfo>();
    public List<StreetInfo> StreetsY = new List<StreetInfo>();

    public int NumStreets;
    public float CitySize;

    public Map(float size, int numStreets)
    {
        CitySize = size;
        NumStreets = numStreets;
        var minDist = CitySize / NumStreets / 3;
        var stnumx = new List<float>();
        var stnumy = new List<float>();
        for (int i = 0; i < numStreets; i++)
        {

            for (int k = 0; k < numStreets * 100; k++)
            {
                var randomx = Random.Range(-CitySize / 2, CitySize / 2);

                float closestDist = CitySize;
                stnumx.ForEach((st) =>
                {
                    closestDist = Mathf.Min(Mathf.Abs(randomx - st), closestDist);
                });
//                Debug.Log(closestDist);

                if (closestDist > minDist)
                {
                    stnumx.Add(randomx);
                    break;
                }
            }

            for (int k = 0; k < numStreets * 100; k++)
            {
                var randomy = Random.Range(-CitySize / 2, CitySize / 2);
                float closestDist = CitySize;
                stnumy.ForEach((st) =>
                {
                    closestDist = Mathf.Min(Mathf.Abs(randomy - st), closestDist);
                });

                if (closestDist > minDist)
                {
                    stnumy.Add(randomy);
                    break;
                }
            }
        }
        stnumx.Sort();
        stnumy.Sort();

        var j = -numStreets / 2 - 1;
        StreetsX = stnumx.Select((snum) =>
        {
            return new StreetInfo()
            {
                position = snum,
                name = string.Format("{0} street", ++j == 0 ? "main" : ((j < 0 ? "SW " : "NE ") + Mathf.Abs(j).ToString())),
                isMain = j == 0,
            };
        }).ToList();

        j = -numStreets / 2;
        StreetsY = stnumy.Select((snum) =>
        {
            return new StreetInfo()
            {
                position = snum,
                name = string.Format("{0} street", ++j == 0 ? "main" : ((j < 0 ? "NW " : "SE ") + Mathf.Abs(j).ToString())),
                isMain = j == 0,
            };
        }).ToList();
    }

    public StreetCrossing GetNearestCrossing(Vector2 position)
    {
        var streetX = StreetsX.Aggregate(StreetsX[0], (res, el) =>
        {
            if (Mathf.Abs(el.position - position.x) < Mathf.Abs(res.position - position.x))
                return el;
            return res;
        });
        var streetY = StreetsY.Aggregate(StreetsY[0], (res, el) =>
        {
            if (Mathf.Abs(el.position - position.y) < Mathf.Abs(res.position - position.y))
                return el;
            return res;
        });
        return new StreetCrossing()
        {
            streetX = streetX,
            streetY = streetY,
        };
    }
}

public class MapsApp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{



    public Map map;
    public float mapsize;
    public int numStreets;
    // Use this for initialization
    //void Start()
    //{

    //}

        /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (map == null) return;
        foreach (var st in map.StreetsX)
        {
            var posy = st.position;
            Gizmos.DrawLine(new Vector3(-mapsize / 2, posy, 0.0f), new Vector3(mapsize / 2, posy, 0.0f));
            //Debug.Log(st.name);
        }
        Gizmos.color = Color.green;
        foreach (var st in map.StreetsY)
        {
            var posx = st.position;
            Gizmos.DrawLine(new Vector3(posx, -mapsize / 2, 0.0f), new Vector3(posx, mapsize / 2, 0.0f));
            //Debug.Log(st.name);
        }
    }
    */

    public Street streetImage;
    public RectTransform mapContainer;
    public RectTransform streetsContainer;
    public ScrollRect scrollRect;
    public float streetWidth;
    public float cityScale = 200.0f;
    public float deltaZoom = 0.1f;
    public float minZoom = 1.0f;
    public float maxZoom = 0.3f;
    public void ZoomIn()
    {
        streetsContainer.localScale += Vector3.one * deltaZoom;
        if (streetsContainer.localScale.x > minZoom)
        {
            streetsContainer.localScale = Vector3.one * minZoom;
        }
        mapContainer.sizeDelta = streetsContainer.localScale.x * cityScale * new Vector2(map.CitySize, map.CitySize);
    }
    public void ZoomOut()
    {
        streetsContainer.localScale -= Vector3.one * deltaZoom;
        if (streetsContainer.localScale.x < maxZoom)
        {
            streetsContainer.localScale = Vector3.one * maxZoom;
        }
        mapContainer.sizeDelta = streetsContainer.localScale.x * cityScale * new Vector2(map.CitySize, map.CitySize);
    }

    public void CreateStreets(Map map)
    {
        mapContainer.sizeDelta = cityScale * new Vector2(map.CitySize, map.CitySize);
        foreach (var st in map.StreetsX)
        {
            var posy = st.position;
            Street newStreet = Instantiate(streetImage, streetsContainer.transform);
            newStreet.Setup(st);
            newStreet.rectTransform.localPosition = cityScale * new Vector3(0.0f, posy, 0.0f);
            newStreet.rectTransform.sizeDelta = cityScale * new Vector2(map.CitySize, streetWidth * 1.5f);
            newStreet.transform.SetSiblingIndex(0);
            newStreet.image.color = newStreet.streetInfo.isMain ? Color.yellow : Color.white;
        }
        foreach (var st in map.StreetsY)
        {
            var posx = st.position;
            Street newStreet = Instantiate(streetImage, streetsContainer.transform);
            newStreet.Setup(st);
            newStreet.rectTransform.localPosition = cityScale * new Vector3(posx, 0.0f, 0.0f);
            newStreet.rectTransform.sizeDelta = cityScale * new Vector2(map.CitySize, streetWidth * 1.5f);
            newStreet.rectTransform.Rotate(Vector3.forward, 90);
            newStreet.transform.SetSiblingIndex(0);
            newStreet.image.color = newStreet.streetInfo.isMain ? Color.yellow : Color.white;
        }

    }

    // Update is called once per frame
    void Start()
    {
        map = new Map(mapsize, numStreets);
        CreateStreets(map);
    }

    public Player player;
    public Image playerImage;
    private void Update()
    {
        playerImage.rectTransform.localPosition = cityScale * player.position;

    }

    public int numPointerUp = 0;
    public int numPointerDown = 0;
    public void OnPointerUp(PointerEventData eventData)
    {
        numPointerUp++;
        //StartCoroutine(
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        numPointerDown++;
        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapContainer, eventData.position, eventData.pressEventCamera, out localCursor);
        StartCoroutine(CheckMouseHold(numPointerDown, localCursor));
    }

    public float timeToHold;
    public IEnumerator CheckMouseHold(int mouseClicks, Vector2 mousePosition)
    {
        yield return new WaitForSeconds(timeToHold);
        if (numPointerDown > numPointerUp && mouseClicks == numPointerDown)
        {
            mapPin.gameObject.SetActive(true);
            Debug.Log("lolo " + mouseClicks + " " + mousePosition);
            mapPin.rectTransform.localPosition = mousePosition;
            moveToPanel.gameObject.SetActive(true);
        }
    }

    public Image mapPin;
    public RectTransform moveToPanel;

    public void MovePlayer()
    {
        player.MoveTowards(mapPin.rectTransform.localPosition / cityScale, () =>
          {
              mapPin.gameObject.SetActive(false);
          });
        moveToPanel.gameObject.SetActive(false);
    }

}
