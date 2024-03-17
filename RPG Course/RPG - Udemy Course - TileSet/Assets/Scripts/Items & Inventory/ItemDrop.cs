using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour               //When drop an item from an enemy, we pass to the item info about its data and velocity.
{
    [SerializeField] int maxDropAmount;       //how many items an enemy can drop.
    [SerializeField] ItemData[] possibleDrops;
    List<ItemData> dropList = new List<ItemData>();

    [SerializeField] GameObject dropPrefab;

    public virtual void GenerateDrop()     // I dont really like the logic of this. Change in my games. This says, go thru each possible drop, and see if can drop based on RNG drop chance.
    {                                //If roll successful, add to list of dropped items. Then, cycle thru how many drops there are, and choose randomly from the list until no more drops left. Only one of each as well.

        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrops[i].dropChance)     //If random < drop chance, can drop.
            {
                dropList.Add(possibleDrops[i]);   //Add to dropped items list.
            }
        }



        for (int i = 0; i < maxDropAmount; i++)
        {
            if (dropList.Count <= 0)      //If nothing to drop, exit.
                return;

            ItemData randomItem = dropList[Random.Range(0, dropList.Count)];

            dropList.Remove(randomItem);     //If you remove this, can get multiple of same item dropped. Seems more normal. Like 2x iron or leather.
            DropItem(randomItem);
        }
    }


    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);   //Spawn drop prefab on position of object script is on, no rotation.

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
