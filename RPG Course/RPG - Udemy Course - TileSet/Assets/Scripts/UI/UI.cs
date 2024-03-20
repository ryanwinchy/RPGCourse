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

    public ItemTooltipUI itemTooltip;
    public StatTooltipUI statTooltip;
    public CraftWindowUI craftWindow;
    public SkillTooltipUI skillTooltip;


    private void Start()
    {
        SwitchTo(null);           //Dont want any menu UI when start game.
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
            return;
        }

        SwitchTo(_menu);     //If not active, activate it.
    }
}
