using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrailFX : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float colourLossRate;

    public void SetupDashTrail(float _colourLossRate, Sprite _spriteImage)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _spriteImage;
        colourLossRate = _colourLossRate;

    }

    private void Update()
    {
        float alpha = spriteRenderer.color.a - colourLossRate * Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if (spriteRenderer.color.a <= 0)
            Destroy(gameObject);
    }


}
