
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{

    public static GameManager instance;

    Transform player;

    [SerializeField] Checkpoint[] checkpoints;   //Array of all checkpoints.
    string closestCheckpointId;

    [Header("Lost Currency")]
    [SerializeField] GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] float lostCurrencyPosX;
    [SerializeField] float lostCurrencyPosY;


    private void Awake()        //Singleton
    {
        if (instance != null)    //Check if any instance, if is, destroy it. If none, assign it. This is because we only want one instance of player manager. When change scenes,tries make two.
            Destroy(instance.gameObject);
        else
            instance = this; // first instance assigned, all others destroyed.
    }

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();            //Finds all checkpoints in the game.

        player = PlayerManager.instance.player.transform;
    }




    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }



    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));        //Slight delay on loading, we have to make sure PlayerManager.instance is loaded first, had bug.
    }

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.savedCheckpoints)   //Cycle thru saved data dictionary (list) of saved checkpoints.
        {
            foreach (Checkpoint checkpoint in checkpoints)  //Cycle thru checkpoints in this script (all checkpoints in game).
            {
                if (checkpoint.id == pair.Key && pair.Value == true)   //If we find same checkpoint in game and save data, and saved checkpoint says its active.
                    checkpoint.ActivateCheckpoint();      //activate it in game.

            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyPosX = _data.lostCurrencyPosX;
        lostCurrencyPosY = _data.lostCurrencyPosY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector2(lostCurrencyPosX, lostCurrencyPosY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;  //After loaded it once, set to 0. They have one chance to loot their body.
    }

    IEnumerator LoadWithDelay(GameData _data)  //To  prevent bugs when try to load instantly
    {
        yield return new WaitForSeconds(0.1f);

        LoadCheckpoints(_data);
        PlacePlayerAtClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }

    private void PlacePlayerAtClosestCheckpoint(GameData _data)
    {
        if (_data.savedClosestCheckpointID == null)
            return;


        closestCheckpointId = _data.savedClosestCheckpointID;        //Saves closest checkpoint to this script also for reference.

        foreach (Checkpoint checkpoint in checkpoints)    //Cycle thru checkpoints in game.
        {
            if (closestCheckpointId == checkpoint.id)      //When find the closest checkpoint id from saved data that matches the CheckPoint id in game.
            {
                player.position = checkpoint.transform.position;   //Set player position to it.
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyPosX = player.position.x;
        _data.lostCurrencyPosY = player.position.y;

        if (FindClosestCheckpoint() != null)               //So dont get error when try to save with no checkpoint activated yet.
            _data.savedClosestCheckpointID = FindClosestCheckpoint().id;       //Stores closest checkpoint id for saving.

        _data.savedCheckpoints.Clear();     //Always clear old saved data before save or will get errors.

        foreach (Checkpoint checkpoint in checkpoints)     //Cycle thru checkpoints in game.
        {
            _data.savedCheckpoints.Add(checkpoint.id, checkpoint.isActivated);  //To checkpoints dictionary in Gamedata script, add id and activated for each checkpoint in this script.
        }
    }

    Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.isActivated)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }

        }

        return closestCheckpoint;

    }


    public void PauseGame(bool _pause)    //Simple function to pause and unpause game.
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }


}
