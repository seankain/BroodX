using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodPlayer : MonoBehaviour
{

    private Rigidbody rb;
    public bool Dead { get; private set; }
    public bool Grounded { get; private set; }
    public bool Running { get; private set; }
    public bool Stationary { get; private set; }
    public Vector3 Velocity { get; private set; }
    private Vector3 respawnLocation = Vector3.zero;
    private float RespawnTime = 3;
    private float DeadElapsed = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            DeadElapsed += Time.deltaTime;
            if(DeadElapsed >= RespawnTime)
            {
                DeadElapsed = 0;
                Respawn();
            }
        }
        Running = Input.GetKey(KeyCode.LeftShift);
        Stationary = (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0);
        Velocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var cicada = collision.collider.gameObject.GetComponent<Cicada>();
        if (cicada != null) { Die(); }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") { Grounded = true; }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") { Grounded = false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Goal")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void Die()
    {
        rb.constraints = (RigidbodyConstraints.None);
        Dead = true;
    }

    void Respawn()
    {
        this.gameObject.transform.rotation = Quaternion.identity;
        this.gameObject.transform.position = respawnLocation;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Dead = false;
    }

}
