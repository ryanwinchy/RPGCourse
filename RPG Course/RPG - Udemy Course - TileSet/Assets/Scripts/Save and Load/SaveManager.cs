using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] string fileName;
    [SerializeField] bool encryptData;

    GameData gameData;
    List<ISaveManager> saveManagers;
    FileDataHandler dataHandler;



    private void Awake()      //Singleton.
    {
        if (instance != null)        //if already have instance, destroy new one it tries to create.
            Destroy(instance.gameObject);
        else               //if none, instance is this.
            instance = this;
    }

    private void Start()  //At start always try to load game.
    {
        saveManagers = FindAllSaveManagers();
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);     //For directory, application.persistentDataPath is used because we dont know the system. Could be mac, windows, iphone, linux, this works for all. Can lookup in unity docs. For windows for eg its users - app data... This is how sea of stars did it lol cos I found my save data in same spot!

        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();      //Constructor sets gameData vars like currency to 0.
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();    //Performs actual deserialization and converts json save file back to gameData.

        if (this.gameData == null)          //'this' isnt necessary just makes clearer. Saying the gameData of this instance of SaveManager, but there is only one as singleton.
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)  //For all scripts implementing ISaveManager, load gameData, like gameData.currency depending on implementation.
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {

        foreach (ISaveManager saveManager in saveManagers)   //For all scripts implementing ISaveManager, update gameData in this script with their data, like currency. Ref type means implementing function can change argument val.
        {
            saveManager.SaveData(ref gameData);
        }                                          //Reference sends the actual ref to the original variable, not a copy like normal arguments. So changing the parameter it receives also changes the original.


        dataHandler.Save(gameData);         //Save the gameData from this script, we got the data from all classes implementing ISaveManager interface.

    }

    private void OnApplicationQuit()        //When exit , will save for you.
    {
        SaveGame();
    }

    List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();  //Return objects with monobehaviour implementing ISaveManager.
                                                                                                          //Ienumerable holds collection of objects (saveManagers). Its iterable, like a list but more general.
        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }


    [ContextMenu("Delete Save File")]
    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.DeleteSave();
    }

}
