using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Slider slider;

    [SerializeField] Image dashImage;
    [SerializeField] Image parryImage;
    [SerializeField] Image crystalImage;
    [SerializeField] Image swordImage;
    [SerializeField] Image blackholeImage;
    [SerializeField] Image flaskImage;

    [SerializeField] TextMeshProUGUI currentCurrency;

    SkillManager skills;
    void Start()
    {
        if (playerStats != null)
            playerStats.OnHealthChanged += UpdateHealthUI;      //Subscribe to onHealthChanged event.

        skills = SkillManager.instance;       //Quicker than typing out all the time.

    }

    void Update()
    {

        currentCurrency.text = PlayerManager.instance.GetCurrency().ToString("#,#");  //Formats with thousand commas for nums.

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.throwSwordUnlocked)
            SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        GetCooldownOf(dashImage, skills.dash.cooldown);           //Some of these dont have cooldowns. Just putting here for completeness. You could put cooldowns for anything.
        GetCooldownOf(parryImage, skills.parry.cooldown);
        GetCooldownOf(crystalImage, skills.crystal.cooldown);
        GetCooldownOf(swordImage, skills.sword.cooldown);
        GetCooldownOf(blackholeImage, skills.blackhole.cooldown);
        GetCooldownOf(flaskImage, Inventory.instance.flaskCooldown);  //Whichever flask currently equipped.

    }

    void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    void SetCooldownOf(Image _image)     //Send the image, like dash image for eg. This will make the cooldown (darker) unfill over cooldown.
    {
        if (_image.fillAmount <= 0)         //If image has no fill (skill has no cooldown).
            _image.fillAmount = 1;
    }

    void GetCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)         //Skill has some cooldown left.
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;     //So does it over time. This is correct, graphic filling matches cooldown.
        }
    }



}
