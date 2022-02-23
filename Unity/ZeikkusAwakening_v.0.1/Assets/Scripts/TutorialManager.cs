using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private float time;
    private bool destroying;
    // Update is called once per frame
    void Update()
    {
        if (destroying) return;
        time += Time.deltaTime;
        if (time > 20) 
            ReverseAndDestroy();
    }

    public void ReverseAndDestroy()
    {
        if (destroying) return;
        destroying = true;
        StartCoroutine(_ReverseAndDestroy());
    }

    private IEnumerator _ReverseAndDestroy()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("fadeOut", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
