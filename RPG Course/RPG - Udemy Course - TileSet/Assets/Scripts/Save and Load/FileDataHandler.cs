using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

//This script has a lot of weird syntax. Even experienced devs have to look it up, its not something we do often, only once per project. Completely fine :)
public class FileDataHandler
{
    string dataDirPath = "";
    string dataFileName = "";

    bool encryptData;
    string codeWord = "FullMetalAlchemistIsAwesome$£%£Kanji";        //Used for encryption and decryption. Can only decode it if know this code word, so make long and weird and complex.

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)   //Constructor to set up this class.
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void Save(GameData _data)    //This is the actual mechanics of saving to file.
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);      //Give full file path.

        try                 //Try tries to do something, if it doesnt succeed, it goes to catch.
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));     //Creates directory at path or overwrites if there already.

            string dataToStore = JsonUtility.ToJson(_data, true);           //Convert GameData to json, serialization.

            if (encryptData)        //if want encryption, run data thru function.
                dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))      //Filestream allows us to work with files, create them.
            {
                using(StreamWriter writer = new StreamWriter(stream))     //Stream writer allows us to write to files.
                {
                    writer.Write(dataToStore);         //Writes data to file.
                }
            }
        }
        catch (Exception e)        //Error log if fails to save.
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()              //This is the actual mechanics of loading from file.
    {

        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))  //Filestream allows us to work with files, open them.
                {
                    using (StreamReader reader = new StreamReader(stream))     //New reader instance.
                    {
                        dataToLoad = reader.ReadToEnd();           //Read string from file.
                    }
                }

                if (encryptData)                                   //Decrypts the data to readable format if we encrypted it, becuase dont want vars changeable in save file.
                    dataToLoad = EncryptDecrypt(dataToLoad);

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);     //Convert from json back to GameData.
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }
        


        public void DeleteSave()          //Delete save within Unity.
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }


        string EncryptDecrypt(string _data)               // j ^ R = 1      1 ^ R = j      Simple encryption / decryption. might want better one for professional game. ESP IF ANYTHING ONLINE!
        {
            string modifiedData = "";

            for (int i = 0; i < _data.Length; i++)   //cycle thru each char in _data string.
            {
                modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);          //% gives you remainder of division.    j ^ R = 1      1 ^ R = j 
            }

            return modifiedData;
        }

    
}
