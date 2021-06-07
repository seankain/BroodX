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
    private float CooldownTime = 0;
    private GameObject[] SpawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        PrefabPool = new List<GameObject>();
        SpawnLocations = GameObject.FindGameObjectsWithTag("CicadaSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        CooldownTime += Time.deltaTime;
        if(CooldownTime >= SpawnTime)
        {
            if (PrefabPool.Count < CicadaPoolSize)
            {
                var cicadaGameObject = Instantiate(CicadaPrefab, SpawnLocations[(int)Random.Range(0, SpawnLocations.Length - 1)].transform.position, Quaternion.identity, null);
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
                    }
                }
            }
        }
    }
}
