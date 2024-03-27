using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    Entity entity => GetComponentInParent<Entity>();
    CharacterStats stats => GetComponentInParent<CharacterStats>();
    RectTransform rectTransform;    //Canvas, so is rect transform.

    Slider slider;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
       
        UpdateHealthUI();    //Run once at start.
    }

    private void OnEnable()
    {
        entity.OnFlipped += FlipUI;       //subscribes to event. Whenever onflipped event triggered in entity script, FlipUI on this script will also run!
        stats.OnHealthChanged += UpdateHealthUI;  //Subscribes to event, when OnHealthChanged event triggered in char stats script, this will run. More efficient than putting this in update func, good practice.
    }

    void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    void FlipUI()                     //Makes health bar flip again whenever entity changes direction. Without this, health bar would flip with entity and look weird.
    {
        rectTransform.Rotate(0, 180, 0);

    }

    private void OnDisable()    //Good to unsubscribe from events. Good for resources, and prevent event leak (object kept alive only because actively subscribed to event).
    {
        if (entity != null)
            entity.OnFlipped -= FlipUI;
        if (stats != null)
            stats.OnHealthChanged -= UpdateHealthUI;
    }
}
