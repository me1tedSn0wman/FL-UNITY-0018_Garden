using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUpgradeIconPrefab : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected Image image_Icon;
    [SerializeField] protected TextMeshProUGUI text_Title;
    [SerializeField] protected TextMeshProUGUI text_Description;
    [SerializeField] protected TextMeshProUGUI text_Price;

    [SerializeField] protected Button button_BuyUpgrade;
    [SerializeField] protected Button button_MaxUpgrade;

    [Header("Set Dynamically")]
    [SerializeField] public string gameUpgradeUID;


    public virtual void Start() {
        button_BuyUpgrade.onClick.AddListener(() =>
        {
            ClickedBuyButton();
        });

        Subscribe();
    }

    public void ClickedBuyButton() {
        GameManager.Instance.TryBuyUpgrade(gameUpgradeUID);
    }

    public void SetUpIcon(GameplayUpgrade gameplayUpgrade) {
        gameUpgradeUID = gameplayUpgrade.uid;

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
        GameManager.Instance.gameUpgradesLibrary.OnUpgradeLevelIncrease += CheckUpgrade;
    }

    public void Unsubscribe() {
        GameManager.Instance.gameUpgradesLibrary.OnUpgradeLevelIncrease -= CheckUpgrade;
    }

    public void CheckUpgrade(string upgradeUID) {

        Debug.Log("Check Upgrade");
        if (!gameUpgradeUID.Equals(upgradeUID))
            return;

        SetUpIcon(GameManager.Instance.gameUpgradesLibrary.GetUpgrade(gameUpgradeUID));
    }

    public void CheckUpgrade() {
        if (gameUpgradeUID == null) {
            return;
        }
        CheckUpgrade(gameUpgradeUID);
    }

    public void OnDestroy()
    {
        Unsubscribe();
    }
}
