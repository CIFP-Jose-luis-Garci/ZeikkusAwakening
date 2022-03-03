using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprites : MonoBehaviour
{
    private static Transform minimapCamera;

    private void Start()
    {
        if (!minimapCamera)
            minimapCamera = FindObjectOfType<MinimapManager>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(90f, minimapCamera.eulerAngles.y, 0f);
    }
}
