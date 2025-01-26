using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BranchSettings 
{
    public Quaternion BranchRotationOffset;
    public float BranchSizeMult;
    // Parent prefab which has a BranchNode component
    public List<GameObject> BranchPrefabs;
    public GameObject FirstBranch;
    public GameObject Trunk;

    public GameObject GetTrunkPrefab()
    {
        if (Trunk == null)
        {
            Debug.LogError("No trunk prefab has been set in the settings asset.");
            return null;
        }

        return Trunk;
    }
    public GameObject GetFirstBranchPrefab()
    {
        if (FirstBranch == null)
        {
            Debug.LogError("No first branch prefab has been set in the settings asset.");
            return null;
        }

        return FirstBranch;
    }

    public GameObject GetRandomBranchPrefab()
    {
        if (BranchPrefabs.Count == 0)
        {
            Debug.LogError("No branch prefabs have been set in the settings asset.");
            return null;
        }

        return BranchPrefabs[Random.Range(0, BranchPrefabs.Count)];
    }
}
