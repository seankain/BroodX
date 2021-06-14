using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPan : MonoBehaviour
{
    [SerializeField]
    private string CameraPanTag = "CameraPan";
    [SerializeField]
    private float Speed = 0.5f;
    private Vector3 CenterFocus = Vector3.zero;
    private Camera cam;
    private GameObject[] panLocations;
    private int currentIndex = 0;
    private Vector3 currentTarget = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        panLocations = GameObject.FindGameObjectsWithTag(CameraPanTag);
        cam.transform.position = panLocations[0].transform.position;
        cam.transform.LookAt(CenterFocus);
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.LookAt(CenterFocus);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position,currentTarget,Time.deltaTime * Speed);
        if(Vector3.Distance(cam.transform.position,currentTarget) < 1)
        {
            SetLocation();
        }
    }

    private void OnEnable()
    {
        cam.transform.position = panLocations[0].transform.position;
        cam.transform.LookAt(CenterFocus);
    }

    private void SetLocation() 
    {
        currentTarget = panLocations[currentIndex].transform.position;
        currentIndex++;
        if (currentIndex > panLocations.Length) { currentIndex = 0; }
    }

}
