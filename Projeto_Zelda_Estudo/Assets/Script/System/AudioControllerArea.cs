using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerArea : MonoBehaviour
{
    public AudioSource musicOnArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !musicOnArea.isPlaying) 
        { 
            musicOnArea.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && musicOnArea.isPlaying) 
        {
            musicOnArea.Stop();
        }
    }


}
