using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        // シーン変更を監視
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Title") // Title以外なら停止して破棄
        {
            audioSource.Stop();
            Destroy(gameObject);
        }
    }
}
