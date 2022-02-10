using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutSceneScript : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainFollowVCam, bossVcam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossVcam.Priority = mainFollowVCam.Priority + 1;

        }
    }

    /*private void OnTriggerExit()
    {
        bossVcam.Priority = mainFollowVCam.Priority - 1;
    }*/

    /* Start is called before the first frame update
    void Start()
    {
       
    }*/

    /* Update is called once per frame
    void Update()
    {
        
    }*/
    
}
