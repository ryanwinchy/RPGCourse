using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Dash Trail FX")]
    [SerializeField] GameObject trailImagePrefab;
    [SerializeField] float colourLossRate;
    [SerializeField] float dashTrailCooldown;
    float dashTrailCooldownTimer;

    [Header("Screen Shake FX")]
    [SerializeField] float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;
    CinemachineImpulseSource screenShake;

    [Space]
    [SerializeField] ParticleSystem dustFx;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }
    private void Update()
    {
        dashTrailCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y * shakeMultiplier);
        screenShake.GenerateImpulse();
    }

    public void CreateDashTrail()
    {
        if (dashTrailCooldownTimer < 0)
        {
            dashTrailCooldownTimer = dashTrailCooldown;

            GameObject newTrailImage = Instantiate(trailImagePrefab, transform.position, transform.rotation);
            newTrailImage.GetComponent<DashTrailFX>().SetupDashTrail(colourLossRate, spriteRenderer.sprite);

        }
    }

    public void PlayDustFX()
    {
        if (dustFx != null)
            dustFx.Play();
    }
}
