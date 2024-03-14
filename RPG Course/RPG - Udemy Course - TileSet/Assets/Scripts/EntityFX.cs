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

    [Header("Ailment Colours")]
    [SerializeField] Color[] chillColour;
    [SerializeField] Color[] igniteColour;     //Array as want to flash between multiple red colours, like on fire
    [SerializeField] Color[] shockColour;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalMaterial = spriteRenderer.material;

    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            spriteRenderer.color = Color.clear;
        else
            spriteRenderer.color = Color.white;
    }

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        Color originalColour = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.color = originalColour;
        spriteRenderer.material = originalMaterial;
    }

    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)   //flash red and white.
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    public void CancelColourChange()
    {
        CancelInvoke();       //Cancels the invoke, if invoke repeating has been used on a method.
        spriteRenderer.color = Color.white;
    }

    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColourFx", 0, 0.17f);
        Invoke("CancelColourChange", _seconds);
    }
    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColourFx", 0, 0.17f);    //Repeat calling so looks like burn.
        Invoke("CancelColourChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColourFx", 0, 0.1f);    //Repeat calling so looks like burn.
        Invoke("CancelColourChange", _seconds);
    }

    void IgniteColourFx()
    {
        if (spriteRenderer.color != igniteColour[0])  //Swap between two burn colours.
            spriteRenderer.color = igniteColour[0];
        else
            spriteRenderer.color = igniteColour[1];
    }

    void ChillColourFx()
    {
        if (spriteRenderer.color != chillColour[0])  //Both same colour for chill. Did like this as works better in this system. If dont do invoke repeating, flashFx interrupts it.
            spriteRenderer.color = chillColour[0];
        else
            spriteRenderer.color = chillColour[1];
    }

    void ShockColourFx()
    {
        if (spriteRenderer.color != shockColour[0])  //Swap between two burn colours.
            spriteRenderer.color = shockColour[0];
        else
            spriteRenderer.color = shockColour[1];
    }

}
