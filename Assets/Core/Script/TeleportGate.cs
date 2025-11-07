using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportGate : MonoBehaviour
{
    public string clearSceneName = "ClearScene";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // テキストを消す
        TeleportMessageUI.I?.HideMessage();

        // シーン遷移
        SceneManager.LoadScene(clearSceneName);
    }
}
