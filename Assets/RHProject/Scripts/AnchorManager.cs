using UnityEngine;
using Fusion;

public class AnchorManager : MonoBehaviour
{
    [SerializeField] private NetworkObject sharedAnchorPrefab;

    // Called when the session is ready
    public void SpawnSharedAnchor(NetworkRunner runner)
    {
        // Optionally, only the host or a single “spawner” calls this.
        // In Shared mode, you could do: if (!runner.IsSharedModeMasterClient) return;

        // Spawn the anchor prefab once
        runner.Spawn(sharedAnchorPrefab, Vector3.zero, Quaternion.identity, null);
    }
}
