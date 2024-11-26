using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUpgradeIconPrefab : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected Image image_Icon;
    [SerializeField] protected TextMeshProUGUI text_Title;
    [SerializeField] protected TextMeshProUGUI text_Description;
    [SerializeField] protected TextMeshProUGUI text_Price;

    [SerializeField] protected Button button_BuyUpgrade;
    [SerializeField] protected Button button_MaxUpgrade;

    [Header("Set Dynamically")]
    [SerializeField] public string gameplayUpgradeUID;


    public virtual void Start() {
        button_BuyUpgrade.onClick.AddListener(() =>
        {
            ClickedBuyButton();
        });

        Subscribe();
    }

    public void ClickedBuyButton() {
        GameplayManager.Instance.TryBuyUpgrade(gameplayUpgradeUID);
    }

    public void SetUpIcon(GameplayUpgrade gameplayUpgrade) {
        gameplayUpgradeUID = gameplayUpgrade.uid;

        image_Icon.sprite = gameplayUpgrade.sprite;
        text_Title.text = gameplayUpgrade.title;
        text_Description.text = gameplayUpgrade.description;
        text_Price.text = gameplayUpgrade.GetUpgradePrice().ToString();

        button_MaxUpgrade.gameObject.SetActive(gameplayUpgrade.IsMaxLevel());
    }

    public void SetParent(Transform newParent) { 
        transform.SetParent(newParent);
    }

    public void Subscribe() {
        GameplayManager.Instance.gameplayUpgradesLibrary.OnUpgradeLevelIncrease += CheckUpgrade;
    }

    public void Unsubscribe()
    {
        if (!GameplayManager.instanceExists) return;
        GameplayManager.Instance.gameplayUpgradesLibrary.OnUpgradeLevelIncrease -= CheckUpgrade;
    }

    public void CheckUpgrade(string upgradeUID) {
        if (!gameplayUpgradeUID.Equals(upgradeUID))
            return;

        SetUpIcon(GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgrade(gameplayUpgradeUID));
    }

    public void OnDestroy()
    {
        Unsubscribe();
    }
}
