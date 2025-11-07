using UnityEngine;
using TMPro;

public class SceneTextDisplay : MonoBehaviour
{
    [Header("表示したいテキストUI (TextMeshPro)")]
    public GameObject messageObject; // TextMeshProのオブジェクト

    [Header("表示時間（秒）")]
    public float displayTime = 5f; // ← 5秒で自動的に消える

    void Start()
    {
        if (messageObject != null)
        {
            messageObject.SetActive(true); // 表示ON
            Invoke(nameof(HideMessage), displayTime); // 指定秒後に消す
        }
    }

    void HideMessage()
    {
        if (messageObject != null)
            messageObject.SetActive(false);
    }
}
