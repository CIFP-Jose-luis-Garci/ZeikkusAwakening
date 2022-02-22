using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFireManager : Magic
{
    private AnimatorManager animatorManager;
    void Start()
    {
        animatorManager = transform.parent.GetComponent<AnimatorManager>();
        animatorManager.PlayTargetAnimation("firecast magic", true);
        Destroy(gameObject);
    }
}
