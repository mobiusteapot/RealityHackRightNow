using UnityEngine;
using Fusion;

public class SharedAnchor : NetworkBehaviour
{
    // Called once this NetworkObject spawns on every client
    public override void Spawned()
    {
        base.Spawned();

        SharedAnchor existing = FindObjectOfType<SharedAnchor>();
        if (existing != null && existing != this)
        {
            // This is a duplicate, so despawn it
            Debug.LogWarning("[SharedAnchor] Duplicate detected. Despawning this copy.");
            Runner.Despawn(Object);
            return;
        }

        // Otherwise, we are the unique instance. Proceed with anchor logic (colocation, etc.)
        Debug.Log("[SharedAnchor] This is the primary instance in the session.");
    }
}
