using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour
{
    public bool isOccupied = false; // Tracks if the cell is already filled with an item
    public GameObject placedItem;  // The item placed in this cell

    public void PlaceItem(GameObject item)
    {
        if (!isOccupied)
        {
            placedItem = item;
            isOccupied = true;

            // Lock the item into place by making it unmovable
            item.transform.SetParent(transform);
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Place it at the center of the cell
        }
    }
}
