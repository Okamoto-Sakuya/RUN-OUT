using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.I.OnCrystalCollected(); // Å©Ç±Ç±ÅI
        Destroy(gameObject);
    }
}
