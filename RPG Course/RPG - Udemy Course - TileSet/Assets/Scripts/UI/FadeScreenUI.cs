using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenUI : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut() => animator.SetTrigger("FadeOut");
    public void FadeIn() => animator.SetTrigger("FadeIn");

    // Update is called once per frame
    void Update()
    {
        
    }
}
