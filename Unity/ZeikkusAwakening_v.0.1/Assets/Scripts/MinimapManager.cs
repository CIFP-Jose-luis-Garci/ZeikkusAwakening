using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public Transform player, camera;
    
    // Start is called before the first frame update
    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(90f, camera.eulerAngles.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
