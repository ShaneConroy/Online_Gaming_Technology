using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public GameObject itemPrefab; 
    public int itemCount = 5; 

    public void OnClickToDrag()
    {
        if (itemCount > 0)
        {
            // Create an item for dragging
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            DraggableItem draggableItem = newItem.GetComponent<DraggableItem>();
            draggableItem.isDragFromItemSlot = true;

            itemCount--;
        }
    }
}
