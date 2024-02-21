using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [Header("Flash FX")]
    [SerializeField] Material hitMaterial;
    Material originalMaterial;
    [SerializeField] float flashTime = 0.2f;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalMaterial = spriteRenderer.material;

    }

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.material = originalMaterial;
    }

    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)   //flash red and white.
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    public void CancelRedBlink()
    {
        CancelInvoke();       //Cancels the invoke, if invoke repeating has been used on a method.
        spriteRenderer.color = Color.white;
    }

}
