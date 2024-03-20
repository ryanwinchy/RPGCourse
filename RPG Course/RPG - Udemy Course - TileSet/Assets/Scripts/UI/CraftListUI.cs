using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListUI : MonoBehaviour, IPointerDownHandler         //This script is so you select sword in craft menu, it populates all the swords you can craft.
{

    [SerializeField] Transform craftSlotParent;        //all craft slots will be nested under here.
    [SerializeField] GameObject craftSlotPrefab;        

    [SerializeField] List<ItemDataEquipment> craftEquipment;    //List of items we can craft. Drag all swords in for weapon craft list for eg.

    
    void Start()
    {
        transform.parent.GetChild(0).GetComponent<CraftListUI>().SetupCraftList();  //Get access to the parent (the craft list panel), get first child in list (weapon craft list), then set this up. So basically, default display is first in craft list, weapons.
        SetupDefaultCraftWindow();
    }



    public void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)   //Clear craft slots. As have selected a new craft list. Like changing from weapon list to armour for eg.
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);    
        }


        for (int i = 0; i < craftEquipment.Count; i++)      //Go thru list of items we can craft (swords if selected weapon for eg).
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);       //Make new craft slot for how many craftable items there are.
            newSlot.GetComponent<CraftSlotUI>().SetupCraftSlot(craftEquipment[i]);       //Setup craft slots with each craftable item.
        }
    }

    public void OnPointerDown(PointerEventData eventData)    //Same as button basically.
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()      //By default display first craftable item in list.
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }
}
