using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [Header("?? 配置範囲 (複数BoxCollider対応)")]
    [Tooltip("クリスタルを並べたい通路やエリアの BoxCollider を登録")]
    public BoxCollider[] spawnAreas;

    [Header("?? クリスタルプレハブ")]
    public GameObject crystalPrefab;

    [Header("?? 生成設定")]
    [Tooltip("クリスタル同士の間隔（大きいほど間が広くなる）")]
    public float spacing = 2f;

    [Tooltip("障害物を避けるためのレイヤー")]
    public LayerMask obstacleMask;

    [Tooltip("地面を探すためのRay距離")]
    public float groundCheckDistance = 10f;

    [Tooltip("地面からの浮かせ高さ")]
    public float groundOffset = 0.3f;

    void Start()
    {
        if (spawnAreas == null || spawnAreas.Length == 0)
        {
            Debug.LogWarning("?? spawnAreas が設定されていません。");
            return;
        }

        int total = 0;
        foreach (var area in spawnAreas)
        {
            if (area == null) continue;
            total += SpawnLineInArea(area);
        }

        Debug.Log($"?? 合計 {total} 個のクリスタルを一列で生成しました！");
    }

    int SpawnLineInArea(BoxCollider areaCollider)
    {
        if (crystalPrefab == null) return 0;

        Vector3 center = areaCollider.bounds.center;
        Vector3 size = areaCollider.bounds.size;

        // ? 自動で「横長」か「縦長」か判定して一列の方向を決める
        float lineLength;
        Vector3 axis;
        if (size.x > size.z)
        {
            lineLength = size.x;
            axis = Vector3.right; // 横向きに一列
        }
        else
        {
            lineLength = size.z;
            axis = Vector3.forward; // 奥向きに一列
        }

        // spacingごとに配置個数を計算
        int count = Mathf.Max(1, Mathf.FloorToInt(lineLength / spacing));
        int spawned = 0;

        for (int i = 0; i < count; i++)
        {
            // コライダー中心から線状に配置
            Vector3 pos = center - (axis.normalized * (lineLength / 2)) + (axis.normalized * i * spacing);

            // 地面をRayで探してその上に置く
            if (Physics.Raycast(pos + Vector3.up * groundCheckDistance, Vector3.down, out RaycastHit hit, groundCheckDistance * 2))
            {
                // 障害物の上には置かない
                if (Physics.CheckSphere(hit.point, 0.3f, obstacleMask))
                    continue;

                Instantiate(crystalPrefab, hit.point + Vector3.up * groundOffset, Quaternion.identity);
                spawned++;
            }
        }

        Debug.Log($"?? {areaCollider.name} に {spawned} 個生成しました");
        return spawned;
    }
}
