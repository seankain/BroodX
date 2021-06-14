using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootfallEmitter : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> StepSounds;
    [SerializeField]
    private float WalkFrequency = 2;
    [SerializeField]
    private float RunFrequency = 4;
    private BroodPlayer player;
    private float interstepElapsed = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<BroodPlayer>();   
    }

    // Update is called once per frame
    void Update()
    {
        interstepElapsed += Time.deltaTime;
       
        if (player.Grounded && !player.Stationary)
        {
            if (player.Running)
            { 
            if(interstepElapsed >= (1 / RunFrequency))
                {
                    PlayStep();
                    interstepElapsed = 0;
                }
            }
            else
            {
                if (interstepElapsed >= 1 / WalkFrequency)
                {
                    PlayStep();
                    interstepElapsed = 0;
                }
            }
        }
    }

    private void PlayStep() {
        GetRandomFootstep().Play();
    }


    private AudioSource GetRandomFootstep()
    {
        return StepSounds[(int)Random.Range(0, StepSounds.Count - 1)];
    }
}
