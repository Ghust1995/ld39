using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        return new StreetCrossing()
        {
            streetX = StreetsX.Find((s) => s.position > position.x),
            streetY = StreetsY.Find((s) => s.position > position.y),
        };
    }
}

[ExecuteInEditMode]
public class MapsApp : MonoBehaviour
{



    public Map map;
    public float mapsize;
    public int numStreets;
    // Use this for initialization
    void Start()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var st in map.StreetsX)
        {
            var posy = st.position;
            Gizmos.DrawLine(new Vector3(-mapsize / 2, posy, 0.0f), new Vector3(mapsize / 2, posy, 0.0f));
            Debug.Log(st.name);
        }
        Gizmos.color = Color.green;
        foreach (var st in map.StreetsY)
        {
            var posx = st.position;
            Gizmos.DrawLine(new Vector3(posx, -mapsize / 2, 0.0f), new Vector3(posx, mapsize / 2, 0.0f));
            Debug.Log(st.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        map = new Map(mapsize, numStreets);
    }
}
