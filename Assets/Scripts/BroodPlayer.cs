using Invector.CharacterController;
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

    [SerializeField]
    private float minCameraDistance = 2;
    [SerializeField]
    private float maxCameraDistance = 10;
    [SerializeField]
    private AudioSource CicadaHitSound;
    [SerializeField]
    private FootfallEmitter footFallEmitter;

    public Camera PlayerCamera;
    private vThirdPersonCamera tpc;
    private vThirdPersonController invectorController;
    private vThirdPersonInput invectorInput;
    public Vector3 Velocity { get; private set; }
    private Vector3 respawnLocation = Vector3.zero;
    private float RespawnTime = 3;
    private float DeadElapsed = 0;
    private CapsuleCollider playerCollider;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        invectorController = GetComponent<vThirdPersonController>();
        invectorInput = GetComponent<vThirdPersonInput>();
        anim = GetComponent<Animator>();
        SetRagdoll(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        respawnLocation = gameObject.transform.position;
        tpc = PlayerCamera.GetComponent<vThirdPersonCamera>();
        footFallEmitter = GetComponentInChildren<FootfallEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        tpc.defaultDistance += -scroll;
        if (tpc.defaultDistance > maxCameraDistance) { tpc.defaultDistance = maxCameraDistance; }
        if (tpc.defaultDistance < minCameraDistance) { tpc.defaultDistance = minCameraDistance; }
        if (Input.GetKeyDown(KeyCode.R)) { Die(); }
        if (Dead)
        {
            DeadElapsed += Time.deltaTime;
            if (DeadElapsed >= RespawnTime)
            {
                DeadElapsed = 0;
                Respawn();
            }
        }
        Running = Input.GetKey(KeyCode.LeftShift);
        Stationary = (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0);
        Velocity = rb.velocity;
    }

    private void SetRagdoll(bool ragdollActivated)
    {
        var colliders = GetComponentsInChildren<Collider>();
        playerCollider.enabled = !ragdollActivated;
        anim.enabled = !ragdollActivated;
        rb.velocity = Vector3.zero;
        rb.isKinematic = ragdollActivated;
        foreach (var collider in colliders)
        {
            if (collider != playerCollider)
            {
                collider.isTrigger = !ragdollActivated;
                collider.attachedRigidbody.isKinematic =!ragdollActivated;
                collider.attachedRigidbody.velocity = Vector3.zero;
            }
        }
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
        if (other.gameObject.name == "Goal")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void Die()
    {
        CicadaHitSound.Play();
        footFallEmitter.enabled = false;
        rb.constraints = (RigidbodyConstraints.None);
        Dead = true;
        invectorController.enabled = false;
        invectorInput.enabled = false;
        SetRagdoll(true);
    }

    void Respawn()
    {
        this.gameObject.transform.rotation = Quaternion.identity;
        this.gameObject.transform.position = respawnLocation;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        footFallEmitter.enabled = true;
        Dead = false;
        invectorController.enabled = true;
        invectorInput.enabled = true;
        SetRagdoll(false);
        
    }

}
