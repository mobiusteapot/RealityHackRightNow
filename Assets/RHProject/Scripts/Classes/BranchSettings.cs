using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BranchSettings 
{
    public Quaternion BranchRotationOffset;
    public float BranchGrowDuration;
    public int BranchGrowSteps = 4;
    // Parent prefab which has a BranchNode component
    public List<GameObject> BranchPrefabs;
    public GameObject FirstBranch;
    public GameObject SecondBigBranch;
    public GameObject Trunk;

    public GameObject GetTrunkPrefab()
    {
        return Trunk;
    }
    public GameObject GetFirstBranchPrefab()
    {
        return FirstBranch;
    }
    public GameObject GetSecondBigBranchPrefab()
    {
        return SecondBigBranch;
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
    
    public float GetBranchScale(int curStep){
        // If past branch grow steps, then return 1
        if (curStep >= BranchGrowSteps)
        {
            return 1.0f;
        }
        return (curStep / (float)BranchGrowSteps);
    }
}
