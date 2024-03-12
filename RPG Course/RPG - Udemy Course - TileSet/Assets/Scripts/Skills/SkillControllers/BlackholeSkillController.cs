using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] GameObject hotkeyPrefab;
    [SerializeField] List<KeyCode> KeycodeList;

    float maxSize;
    float growSpeed;
    float shrinkSpeed;
    float blackholeDuration;

    bool canGrow = true;
    bool canShrink;
    bool canCreateHotkeys = true;
    bool cloneAttackReleased;
    bool playerCanDisappear = true;


    int numAttacks = 4;
    float cloneAttackCooldown = 0.3f;
    float cloneAttackTimer;

    List<Transform> targets = new List<Transform>();
    List<GameObject> createdKeys = new List<GameObject>();

    public bool playerCanExitState {  get; private set; }


    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _numAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        numAttacks = _numAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeDuration = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)    //if youre casting crystals instead of clones, do not disappear, looked weird.
            playerCanDisappear = false;
    }

    private void Update()
    {

        cloneAttackTimer -= Time.deltaTime;
        blackholeDuration -= Time.deltaTime;

        if (blackholeDuration <= 0 )           //If dont press anything during black hole.
        {
            blackholeDuration = Mathf.Infinity;     //Ensures only does it once.

            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);    //Lerp grows from one size to max size overtime. Gets slower at end, like curve.
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);    //Lerp grows from one size to max size overtime. 

            if (transform.localScale.x < 0)       //Destroy black hole when gets small.
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;

        DestroyHotkeys();
        cloneAttackReleased = true;
        canCreateHotkeys = false;

        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            if(SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {

                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector2(xOffset, 0));   //Create clone for random targets. Has an offset of -2 or 2.
            }

            numAttacks--;

            if (numAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", 1f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotkeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;

    }

    void DestroyHotkeys()
    {
        if (createdKeys.Count <= 0)
            return;

        for (int i = 0; i < createdKeys.Count; i++)
        {
            Destroy(createdKeys[i]);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)     //Within black hole, check if collision is enemy, if it is add to targets list.
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotkey(collision);



            //targets.Add(collision.transform);
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<Enemy>() != null)
    //        collision.GetComponent<Enemy>().FreezeTime(false);
    //}*/

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);  //Same as above. ? checks null, if not null, performs action after dot.
    private void CreateHotkey(Collider2D collision)
    {
        if (KeycodeList.Count <= 0)
        {
            Debug.Log("Not enough hotkeys in keycode list.");
            return;
        }

        if (!canCreateHotkeys)
            return;


        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);   //Spawn prefab of hotkey 2 units above enemy, no rotation.
        createdKeys.Add(newHotkey);    //Adds created keys to list for tracking.

        KeyCode chosenKey = KeycodeList[Random.Range(0, KeycodeList.Count)];      //Choose random keycode from list.
        KeycodeList.Remove(chosenKey);     //Remove chosen key from list so doesn't come up twice.

        BlackholeHotkeyController newHotkeyScript = newHotkey.GetComponent<BlackholeHotkeyController>();   //Get hotkeycontroller script from key above enemy.

        newHotkeyScript.SetupHotkey(chosenKey, collision.transform, this);       //Setup the hotkey.
    }

    public void AddEnemyToList(Transform _enemy) => targets.Add(_enemy);

}
