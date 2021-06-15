using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private AudioSource audioSource;

    public delegate void PlayerScoreHandler(object sender);
    public event PlayerScoreHandler PlayerScored;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioSource.Play();
            if(PlayerScored != null)
            {
                PlayerScored.Invoke(this);
            }
        }
    }

}
