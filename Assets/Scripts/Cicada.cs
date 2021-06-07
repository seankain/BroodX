using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cicada : MonoBehaviour
{
    [SerializeField]
    private AudioSource CallSound;
    [SerializeField]
    private float LimpLifetimeSeconds = 10;
    private float LimpElapsed = 0;
    private bool Limped = false;
    private float Speed = 0.5f;
    [SerializeField]
    private float EmitProbability = 0.1f;
    private Vector3 Destination = Vector3.zero;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (Limped)
        {
            LimpElapsed += Time.deltaTime;
            if(LimpElapsed >= LimpLifetimeSeconds) { gameObject.SetActive(false); LimpElapsed = 0; }
            return;
        }
        this.transform.LookAt(Destination);
        if (!CallSound.isPlaying)
        {
            if(Random.value > (1 - EmitProbability))
            {
                CallSound.Play();
            }
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, Destination, Time.deltaTime * Speed);
        if (Vector3.Distance(this.transform.position, Destination) < 0.5) { GoLimp(); }
    }

    private void SetDestination() 
    {
        //var cicadaSpawners = FindObjectsOfType<CicadaSpawner>();
        var landings = GameObject.FindGameObjectsWithTag("CicadaLanding");
        var idx = (int)(Random.value * landings.Length);
        Destination = landings[idx].transform.position + new Vector3(Random.value,Random.value,Random.value);
    }

    /// <summary>
    /// Do the thing that 17 year cicadas do where they just give up and roll around on the ground
    /// </summary>
    private void GoLimp()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        Limped = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GoLimp();
    }


}
