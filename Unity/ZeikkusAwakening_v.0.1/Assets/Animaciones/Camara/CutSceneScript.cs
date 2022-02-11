using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class CutSceneScript : MonoBehaviour
{
    public GameObject vCamera;
    public Animator animatorBadZ;
    public RuntimeAnimatorController animatorBadZController;

    public void DesactivarTimeline()
    {
        vCamera.SetActive(false);
        animatorBadZ.runtimeAnimatorController = animatorBadZController;
    }

}
