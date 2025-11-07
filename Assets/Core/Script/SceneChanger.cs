using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // 移動先のシーン名をInspectorで設定
    [SerializeField] private string sceneName = "Main";

    // ボタンの OnClick() にこの関数を登録
    public void OnClickChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("?? シーン名が設定されていません！");
        }
    }
}