using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveFlowerArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<bool> OnPointerInRemoveArea;
    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("ThereArePointer");
        OnPointerInRemoveArea?.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("ThereAreNoPointer");
        OnPointerInRemoveArea?.Invoke(false);
    }

}