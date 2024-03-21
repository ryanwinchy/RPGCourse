using System;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType { Regular, Bounce, Pierce, Spin }    //Enum of type of sword, all have a number also.

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;   //Regular sword by default.

    [Header("Bounce Info")]
    [SerializeField] SkillTreeSlotUI bounceUnlockButton;
    [SerializeField] int numBounces;
    [SerializeField] float bounceGravity;
    [SerializeField] float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] SkillTreeSlotUI pierceUnlockButton;
    [SerializeField] int pierceAmount;
    [SerializeField] float pierceGravity;       //This controls how straight the shot is.

    [Header("Spin Info")]
    [SerializeField] SkillTreeSlotUI spinUnlockButton;
    [SerializeField] float hitCooldown = 0.35f;   //So 3 attacks per second.
    [SerializeField] float maxTravelDistance = 7;
    [SerializeField] float spinDuration = 2f;
    [SerializeField] float spinGravity;


    [Header("Skill Info")]
    [SerializeField] SkillTreeSlotUI throwSwordUnlockButton;
    public bool throwSwordUnlocked { get; private set; }
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchForce;
    [SerializeField] float swordGravity;
    [SerializeField] float freezeTimeDuration;
    [SerializeField] float returnSpeed;

    [Header("Passive Skills")]
    [SerializeField] SkillTreeSlotUI timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] SkillTreeSlotUI vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] int numDots;
    [SerializeField] float spaceBetweenDots;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] Transform dotsParent;

    GameObject[] dots;

    new Camera camera;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        camera = FindObjectOfType<Camera>();

        SetupGravity();


        throwSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockThrowSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBouncySword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerability);

    }

    void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;

    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y); //Take aim direction * force to get direction for sword to go.
                                                                                                                              //Normalized sets vector length to one but keeps direction.

        if (Input.GetKey(KeyCode.Mouse1))    //if mouse held.
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);    //Num dots * space between.
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);    //Instantiate sword at player pos with default rotation.
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();     //Get access to script of newly created sword.

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, numBounces, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);



        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);      //Assign new sword to player, so player knows it has one sword.

        DotsActive(false);    //Turn off dots when sword actually created.
    }

    #region Unlock Region

    void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    void UnlockVulnerability()
    {
        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }

    void UnlockThrowSword()
    {
        if (throwSwordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            throwSwordUnlocked = true;
        }
    }

    void UnlockBouncySword()
    {
        if (bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }

    void UnlockPierceSword()
    {
        if (pierceUnlockButton.unlocked)
        {
            swordType = SwordType.Pierce;
        }
    }

    void UnlockSpinSword()
    {
        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }



    #endregion

    #region Aim Region
    public Vector2 AimDirection()   //Returns direction of aim, by getting mouse - player pos.
    {
        Vector2 playerPosition = player.transform.position;     //Starting pos.
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);            //Camera.main shortcut was not working, had to make a reference to camera.


        Vector2 direction = mousePos - playerPosition;

        return direction;

    }


    void GenerateDots()
    {
        dots = new GameObject[numDots];     //Set up array of game objects with num of dots.
        for (int i = 0; i < numDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);   //Go thru array, instantiate dots.
            dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y)
            * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);
        //Getting direction and multiplying by gravity which drags down the vector.


        return position;
    }

    #endregion

}
