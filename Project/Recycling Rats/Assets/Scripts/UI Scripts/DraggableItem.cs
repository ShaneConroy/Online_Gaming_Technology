using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public bool isDragFromItemSlot = false; // Whether the item is being dragged from an Item Slot

    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition; // Save the original position of the item

        // Make the item semi-transparent and non-blocking during drag
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = eventData.position; // Follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // If the item is dropped on a valid cell, we handle placing it
        if (eventData.pointerEnter != null)
        {
            DropSlot dropCell = eventData.pointerEnter.GetComponent<DropSlot>();

            if (dropCell != null && !dropCell.isOccupied)
            {
                dropCell.PlaceItem(gameObject);
                return; // Item successfully placed on grid, no need to reset position
            }
        }

        // Return to original position if the drop was not successful
        rectTransform.anchoredPosition = originalPosition;
    }
}
