using UnityEngine;
using UnityEngine.UI;

public class WindowUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] protected Button button_CloseWindowCanvas;

    public virtual void Awake() {
        if (button_CloseWindowCanvas != null) {
            button_CloseWindowCanvas.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
            });
        }
    }

    public virtual void SetActive(bool value) { 
        gameObject.SetActive(value);
    }
}
