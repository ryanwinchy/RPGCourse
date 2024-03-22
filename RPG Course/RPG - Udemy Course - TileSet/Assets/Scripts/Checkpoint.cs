using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator animator;

    public string id;
    public bool isActivated;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]  //So we can generate ID on right click menu in Unity.
    void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();    //This will generate an ID for the checkpoint so we can save / load it.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)       //When player collides, activate active anim.
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        isActivated = true;
        animator.SetBool("active", true);
    }
}
