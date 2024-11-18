using UnityEngine;

public class UpgradesUI : WindowUI
{
    [Header("Upgrades UI")]
    [Header("Content")]
    [SerializeField] private GameObject go_Content;

    public void OnEnable()
    {
        UpdateContent();
    }

    public void UpdateContent() { 
    
    }
}
