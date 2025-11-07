using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("基本設定")]
    public NavMeshAgent agent;
    public Transform player;
    public Animator animator;
    public AudioSource footstepSource;

    [Header("足音設定")]
    public AudioClip[] footstepClips; // 足音クリップ複数登録可能
    public float footstepInterval = 0.6f;
    private float footTimer = 0f;

    [Header("予測・先回り設定")]
    public bool usePrediction = false;
    public float predictTime = 0.6f;
    public float stoppingDistance = 1f;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        agent.stoppingDistance = stoppingDistance;

        // AudioSourceの自動設定
        if (footstepSource == null) footstepSource = GetComponent<AudioSource>();
        if (footstepSource != null)
        {
            footstepSource.spatialBlend = 1f; // 3D音にする
            footstepSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (player == null) return;

        // --- 予測追跡（usePrediction=trueなら先回り） ---
        Vector3 targetPos = player.position;
        if (usePrediction)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                Vector3 v = pc.currentVelocity;
                targetPos = player.position + v * predictTime;
            }
        }

        agent.SetDestination(targetPos);

        // --- アニメーション更新 ---
        if (animator != null)
        {
            float speed = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speed);
        }

        // --- 足音 ---
        if (agent.velocity.magnitude > 0.1f)
        {
            footTimer += Time.deltaTime;
            if (footTimer >= footstepInterval)
            {
                footTimer = 0f;
                PlayFootstep();
            }
        }
        else
        {
            footTimer = footstepInterval;
        }
    }

    void PlayFootstep()
    {
        if (footstepSource == null || footstepClips.Length == 0) return;

        // ランダムに音を選ぶ
        int i = Random.Range(0, footstepClips.Length);
        footstepSource.clip = footstepClips[i];
        footstepSource.pitch = Random.Range(0.9f, 1.1f); // 少し揺らす
        footstepSource.Play();
    }
}
