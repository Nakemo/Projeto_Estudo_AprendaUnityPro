using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCam : MonoBehaviour
{
    public GameObject vCamera2;
    public GameObject vCamera3;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CamTrigger":
                vCamera2.SetActive(true);
                break;
            case "CamTrigger2":
                vCamera3.SetActive(true);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CamTrigger":
                vCamera2.SetActive(false);
                break;

            case "CamTrigger2":
                vCamera3.SetActive(false);
                break;
        }
    }
}
