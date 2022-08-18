using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlot : MonoBehaviour, IPointerClickHandler
{
    public static QuickSlot Instance;
    
    

    [SerializeField]
    Slot quickslot;

    public Slot quickSlot => quickslot;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        if ( eventData.button == PointerEventData.InputButton.Right && quickslot.IsInRect(eventData.position) )
        {
            quickslot.Use((UsableItem)quickslot.item);
        }
    }
}
