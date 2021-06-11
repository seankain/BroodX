using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicadaSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject CicadaPrefab;
    [SerializeField]
    private int CicadaPoolSize = 10;
    private List<GameObject> PrefabPool;
    [SerializeField]
    private float SpawnTime = 5;
    private float ScanHeight = 15;
    private float ScanResolution = 0.1f;
    [SerializeField]
    private GameObject SpawnBound1;
    [SerializeField]
    private GameObject SpawnBound2;
    [SerializeField]
    private GameObject SpawnBound3;
    [SerializeField]
    private GameObject SpawnBound4;

    private float CooldownTime = 0;
    private GameObject[] SpawnLocations;
    private List<Vector3> spawnLocations;
    private List<Vector3> landLocations;

    // Start is called before the first frame update
    void Start()
    {
        PrefabPool = new List<GameObject>();
        SpawnLocations = GameObject.FindGameObjectsWithTag("CicadaSpawn");
        spawnLocations = new List<Vector3>();
        landLocations = new List<Vector3>();
        GeneratePositions();
    }

    private void GeneratePositions()
    {
        var xmin = Mathf.Infinity;
        var ymin = Mathf.Infinity;
        var xmax = Mathf.NegativeInfinity;
        var ymax = Mathf.NegativeInfinity;
        var zmin = Mathf.Infinity;
        var zmax = Mathf.NegativeInfinity;
        var bounds = new GameObject[] { SpawnBound1, SpawnBound2, SpawnBound3, SpawnBound4 };
        foreach(var b in bounds)
        {
            if(b.transform.position.x < xmin)
            {
                xmin = b.transform.position.x;
            }
            if(b.transform.position.y < ymin)
            {
                ymin = b.transform.position.y;
            }
            if (b.transform.position.y < zmin)
            {
                zmin = b.transform.position.z;
            }
            if (b.transform.position.x > xmax)
            {
                xmax = b.transform.position.x;
            }
            if (b.transform.position.y > ymax)
            {
                ymax = b.transform.position.y;
            }
            if (b.transform.position.z > zmax)
            {
                zmax = b.transform.position.z;
            }
        }
        for(var x = xmin; x <= xmax; x+=ScanResolution)
        {
            for(var z = zmin; z <= zmax; z+=ScanResolution)
            {
                var pos = new Vector3(x, ScanHeight, z);
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                {
                    
                    if(hit.collider.gameObject.tag == "Ground")
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.white);
                        landLocations.Add(hit.point);
                    }
                    if(hit.collider.gameObject.tag == "Tree")
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
                        spawnLocations.Add(hit.point);
                    }
                    //Debug.Log("Did Hit");
                }
                else
                {
                    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    //Debug.Log("Did not Hit");
                }
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        CooldownTime += Time.deltaTime;
        if(CooldownTime >= SpawnTime)
        {
            if (PrefabPool.Count < CicadaPoolSize)
            {
                //var cicadaGameObject = Instantiate(CicadaPrefab, SpawnLocations[(int)Random.Range(0, SpawnLocations.Length - 1)].transform.position, Quaternion.identity, null);
                var cicadaGameObject = Instantiate(CicadaPrefab, spawnLocations[(int)Random.Range(0, spawnLocations.Count - 1)], Quaternion.identity, null);
                CooldownTime = 0;
                PrefabPool.Add(cicadaGameObject);
            }
            else
            {
                foreach(var c in PrefabPool)
                {
                    if (!c.activeInHierarchy)
                    {
                        c.transform.position = SpawnLocations[(int)Random.Range(0, SpawnLocations.Length - 1)].transform.position;
                        c.SetActive(true);
                        c.GetComponent<Cicada>().Respawn();
                    }
                }
            }
        }
    }

    public Vector3 RandomLandLocation()
    {
        return landLocations[(int)Random.Range(0, landLocations.Count - 1)];
    }

}
