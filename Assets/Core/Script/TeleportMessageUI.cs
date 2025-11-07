using UnityEngine;
using TMPro;

public class TeleportMessageUI : MonoBehaviour
{
    public static TeleportMessageUI I;

    [Header("テレポートメッセージUI")]
    public TextMeshProUGUI messageText;

    private void Awake()
    {
        I = this;
        messageText.gameObject.SetActive(false);
    }

    /// <summary>
    /// メッセージを表示（非表示になるまでずっと表示）
    /// </summary>
    public void ShowMessage(string text)
    {
        messageText.text = text;
        messageText.gameObject.SetActive(true);
    }

    /// <summary>
    /// メッセージを消す
    /// </summary>
    public void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }
}
