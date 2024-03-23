using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Player player;

    [Header("Pup Up Text")]
    [SerializeField] GameObject popUpTextPrefab;





    [Header("Flash FX")]
    [SerializeField] Material hitMaterial;
    Material originalMaterial;
    [SerializeField] float flashTime = 0.2f;

    [Header("Ailment Colours")]
    [SerializeField] Color[] chillColour;
    [SerializeField] Color[] igniteColour;     //Array as want to flash between multiple red colours, like on fire
    [SerializeField] Color[] shockColour;

    [Header("Ailment Particles")]
    [SerializeField] ParticleSystem igniteFX;
    [SerializeField] ParticleSystem chillFX;
    [SerializeField] ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] GameObject hitFx;
    [SerializeField] GameObject critHitFx;



    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;

        originalMaterial = spriteRenderer.material;

    }

    private void Update()
    {

    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(1, -1);
        float randomY = Random.Range(1.5f, 3);


        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
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

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillColourFx", 0, 0.17f);
        Invoke("CancelColourChange", _seconds);
    }
    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColourFx", 0, 0.17f);    //Repeat calling so looks like burn.
        Invoke("CancelColourChange", _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();
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

    public void CreateHitFx(Transform _target, bool _crit)
    {

        float zRotation = Random.Range(-90, 90);
        float xOffset = Random.Range(-0.5f, 0.5f);
        float yOffset = Random.Range(-0.5f, 0.5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if (_crit)
        {
            hitPrefab = critHitFx;     //prefab is crit prefab instead.

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);     //Smaller rotation variance.

            if (GetComponent<Entity>().facingDir == -1)      //Always facing damage dealer.
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitfx = Instantiate(hitPrefab, _target.position + new Vector3(xOffset, yOffset), Quaternion.identity); //Instantiate.

        newHitfx.transform.Rotate(hitFxRotation);   //Rotate randomly.



        Destroy(newHitfx, 0.5f);       //Destroy after 0.5 seconds.
    }



}
