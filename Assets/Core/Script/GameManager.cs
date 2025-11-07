using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("?? クリスタル設定")]
    public int initialCrystalCount = 200;
    private int remainingCrystals;

    [Header("?? 敵プレハブ設定")]
    public GameObject firstEnemyPrefab;   // 最初の敵
    public GameObject secondEnemyPrefab;  // 2体目
    public GameObject thirdEnemyPrefab;   // 3体目

    [Header("?? 敵スポーン位置")]
    public Transform firstEnemySpawnPoint;
    public Transform secondEnemySpawnPoint;
    public Transform thirdEnemySpawnPoint;

    [Header("?? 追加条件")]
    public int spawnAtRemaining150 = 150; // 残り150で2体目
    public int spawnAtRemaining130 = 130; // 残り130で3体目

    [Header("?? テレポートゲート設定")]
    public Transform[] teleportPoints;
    public GameObject teleportGatePrefab;
    private bool gateSpawned = false;

    private bool isPlayerCaught = false;

    void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);

        remainingCrystals = initialCrystalCount;
    }

    void Start()
    {
        // ?? ゲーム開始時、最初の敵を出す
        if (firstEnemyPrefab != null && firstEnemySpawnPoint != null)
        {
            Instantiate(firstEnemyPrefab, firstEnemySpawnPoint.position, Quaternion.identity);
            Debug.Log("?? 最初の敵をスポーンしました。");
        }
    }

    public void OnCrystalCollected()
    {
        remainingCrystals = Mathf.Max(0, remainingCrystals - 1);
        Debug.Log($"?? 残りクリスタル: {remainingCrystals}");

        // 敵追加条件チェック
        if (remainingCrystals == spawnAtRemaining150)
        {
            SpawnEnemy(secondEnemyPrefab, secondEnemySpawnPoint, "2体目の敵");
        }
        else if (remainingCrystals == spawnAtRemaining130)
        {
            SpawnEnemy(thirdEnemyPrefab, thirdEnemySpawnPoint, "3体目の敵");
        }

        // 全部集めたらゲート出現
        if (remainingCrystals == 0 && !gateSpawned)
        {
            SpawnTeleportGate();
            gateSpawned = true;
        }
    }

    private void SpawnEnemy(GameObject prefab, Transform spawnPoint, string label)
    {
        if (prefab == null || spawnPoint == null)
        {
            Debug.LogWarning($"?? {label} のPrefabまたはSpawnPointが設定されていません！");
            return;
        }

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"? {label} を出現させました。");
    }

    private void SpawnTeleportGate()
    {
        if (teleportPoints == null || teleportPoints.Length == 0 || teleportGatePrefab == null) return;

        int index = Random.Range(0, teleportPoints.Length);
        Instantiate(teleportGatePrefab, teleportPoints[index].position, Quaternion.identity);
        Debug.Log($"?? テレポートゲート出現（ポイント {index}）");
    }

    // ?? プレイヤーが捕まった処理（カメラ演出＋GameOverへ）
    public void OnPlayerCaught(Transform enemyHead)
    {
        if (isPlayerCaught) return;
        isPlayerCaught = true;
        StartCoroutine(HandlePlayerCaughtRoutine(enemyHead));
    }

    private IEnumerator HandlePlayerCaughtRoutine(Transform enemyHead)
    {
        var cam = Camera.main.transform;
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        Vector3 targetPos = enemyHead.position;
        Quaternion targetRot = Quaternion.LookRotation(enemyHead.position - cam.position);

        float t = 0f;
        float duration = 0.8f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cam.position = Vector3.Lerp(startPos, targetPos, t / duration);
            cam.rotation = Quaternion.Slerp(startRot, targetRot, t / duration);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("ResultScene");
    }

    public int GetRemainingCrystals() => remainingCrystals;
}
