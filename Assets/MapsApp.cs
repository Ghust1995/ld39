using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Map
{
    public struct StreetInfo
    {
        public float position;
        public string name;
    }

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
            var randomx = Random.Range(-CitySize / 2, CitySize / 2);
            var randomy = Random.Range(-CitySize / 2, CitySize / 2);
            stnumx.Add(randomx);
            stnumy.Add(randomy);
        }
        stnumx.Sort();
        stnumy.Sort();

        var j = -numStreets / 2 - 1;
        StreetsX = stnumx.Select((snum) =>
        {
            return new StreetInfo()
            {
                position = snum,
                name = string.Format("{0} street", ++j == 0 ? "main" : ((j < 0 ? "SW" : "NE") + Mathf.Abs(j).ToString())),
            };
        }).ToList();

        j = -numStreets / 2;
        StreetsY = stnumy.Select((snum) =>
        {
            return new StreetInfo()
            {
                position = snum,
                name = string.Format("{0} street", ++j == 0 ? "main" : ((j < 0 ? "NW" : "SE") + Mathf.Abs(j).ToString())),
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

public class MapsApp : MonoBehaviour
{



    public Map map;
    public float mapsize;
    public int numStreets;
    // Use this for initialization
    //void Start()
    //{

    //}

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

    public Image streetImage;
    public RectTransform mapContainer;
    public ScrollRect scrollRect;
    public float streetWidth;
    public float cityScale = 200.0f;
    public float deltaZoom = 0.1f;
    public float minZoom = 1.0f;
    public float maxZoom = 0.3f;
    public void ZoomIn()
    {
        mapContainer.localScale += Vector3.one * deltaZoom;
        if (mapContainer.localScale.x > minZoom)
        {
            mapContainer.localScale = Vector3.one * minZoom;
        }
    }
    public void ZoomOut()
    {
        mapContainer.localScale -= Vector3.one * deltaZoom;
        if (mapContainer.localScale.x < maxZoom)
        {
            mapContainer.localScale = Vector3.one * maxZoom;
        }
    }
    public void Recenter()
    {
        Canvas.ForceUpdateCanvases();

        mapContainer.anchoredPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(mapContainer.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(player.position);
    }

    public void CreateStreets(Map map)
    {
        mapContainer.sizeDelta = cityScale * new Vector2(map.CitySize, map.CitySize);
        foreach (var st in map.StreetsX)
        {
            var posy = st.position;
            Image newStreet = Instantiate(streetImage, mapContainer.transform);
            newStreet.rectTransform.localPosition = cityScale * new Vector3(0.0f, posy, 0.0f);
            newStreet.rectTransform.sizeDelta = cityScale * new Vector2(map.CitySize, streetWidth);
            newStreet.transform.SetSiblingIndex(0);
        }
        foreach (var st in map.StreetsY)
        {
            var posx = st.position;
            Image newStreet = Instantiate(streetImage, mapContainer.transform);
            newStreet.rectTransform.localPosition = cityScale * new Vector3(posx, 0.0f, 0.0f);
            newStreet.rectTransform.sizeDelta = cityScale * new Vector2(streetWidth, map.CitySize);
            newStreet.transform.SetSiblingIndex(0);
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
}
