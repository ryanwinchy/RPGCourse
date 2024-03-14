using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    KeyCode hotkey;
    TextMeshProUGUI text;

    Transform enemy;
    BlackholeSkillController blackholeScript;

    public void SetupHotkey(KeyCode _hotkey, Transform _enemy, BlackholeSkillController _blackholeScript)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();   //get text component from child of hotkey game object.

        enemy = _enemy;
        blackholeScript = _blackholeScript;

        hotkey = _hotkey;
        text.text = _hotkey.ToString();      //Change to text to what we assigned hotkey to be.
    }
        
    
        


    private void Update()
    {
        if (Input.GetKeyDown(hotkey))
        {
            blackholeScript.AddEnemyToList(enemy);

            text.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }


}
