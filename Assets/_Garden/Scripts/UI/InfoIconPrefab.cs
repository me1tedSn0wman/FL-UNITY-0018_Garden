using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoIconPrefab : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected Image image_Icon;
    [SerializeField] protected TextMeshProUGUI text_Title;
    [SerializeField] protected TextMeshProUGUI text_Description;

    public void SetUpIcon(InfoDef def) {
        if (def.sprite != null)
        {
            image_Icon.sprite = def.sprite;
        }
        else {
            image_Icon.gameObject.SetActive(false);
        }

        text_Title.text = def.title;
        text_Description.text = def.description;
    }

    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }
}
