using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodPlayer : MonoBehaviour
{

    private Rigidbody rb;
    public bool Dead { get; private set; }

    private float RespawnTime = 3;
    private float DeadElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        var cicada = collision.collider.gameObject.GetComponent<Cicada>();
        if (cicada != null) { Die(); }
    }

    void Die()
    {
        rb.constraints = (RigidbodyConstraints.None);
        Dead = true;
    }

    void Respawn()
    {
        this.gameObject.transform.rotation = Quaternion.identity;
        this.gameObject.transform.position = new Vector3(0, 3, 0);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Dead = false;
    }

}
