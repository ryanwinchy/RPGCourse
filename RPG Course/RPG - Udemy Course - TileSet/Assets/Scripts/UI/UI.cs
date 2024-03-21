using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour        //Goes on menus to allow switching.
{

    [SerializeField] GameObject characterUI;
    [SerializeField] GameObject skillTreeUI;
    [SerializeField] GameObject craftUI;
    [SerializeField] GameObject optionsUI;
    [SerializeField] GameObject inGameUI;

    public ItemTooltipUI itemTooltip;
    public StatTooltipUI statTooltip;
    public CraftWindowUI craftWindow;
    public SkillTooltipUI skillTooltip;


    private void Awake()
    {
        SwitchTo(skillTreeUI);            //Switch on skill tree quickly. So can assign events on skill tree slots before on skill slots. Bug fix. Not necessary unless skill tree off in inspector.
    }

    private void Start()
    {
        SwitchTo(inGameUI);           //Dont want any menu UI when start game.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))       //Change menu with key press.
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)       //Cycle through all children of canvas.
        {
            transform.GetChild(i).gameObject.SetActive(false);  //Set them all inactive.
        }

        if (_menu != null)               //Set menu we want to active.
            _menu.SetActive(true);
    }

    public void SwitchWithKeyTo(GameObject _menu)          //Change UI menu with key presses.
    {
        if (_menu != null && _menu.activeSelf)     //Checks if _menu gameobject currently active.
        {
            _menu.SetActive(false);        //If active, deactivate.

            CheckForInGameUI();   //Checks if needs to go back to in gameplay UI.

            return;
        }

        SwitchTo(_menu);     //If not active, activate it.
    }

    void CheckForInGameUI()     //Checks if any menu active. If all off, switch to in game UI, so health and such in gameplay.
    {
        for (int i = 0; i < transform.childCount; i++)       
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;

            SwitchTo(inGameUI);
        }
    }


}
