using UnityEngine;

public class BranchSocket : MonoBehaviour
{
    public BranchNode BranchNode;

    public bool IsFull => BranchNode != null;

    public virtual bool TryAddNewBranch(string topic, SequenceState sequenceState = SequenceState.Default)
    {
        if (IsFull)
        {
            return false;
        }

        // Create the prefab
        var branchSettings = RealityHackSettings.Instance.BranchSettings;
        var rotOffset = transform.rotation * branchSettings.BranchRotationOffset;
        GameObject branchPrefab = GetCorrectBranchPrefab(sequenceState);

        var newBranchNodeObject = Instantiate(branchPrefab, transform.position, rotOffset, transform);
        var newBranchNode = newBranchNodeObject.GetComponent<BranchNode>();
        if (newBranchNode == null)
        {
            Debug.LogError("Missing BranchNode component on prefab.");
            Destroy(newBranchNodeObject);
            return false;
        }

        newBranchNode.InitializeBranch(topic);

        BranchNode = newBranchNode;
        return true;
    }


    private GameObject GetCorrectBranchPrefab(SequenceState sequenceState)
    {
        switch (sequenceState)
        {
            case SequenceState.FirstBranch:
                return RealityHackSettings.Instance.BranchSettings.GetFirstBranchPrefab();
            case SequenceState.FirstTrunkPiece:
                return RealityHackSettings.Instance.BranchSettings.GetTrunkPrefab();
            case SequenceState.SecondBigBranch:
                return RealityHackSettings.Instance.BranchSettings.GetSecondBigBranchPrefab();
            default:
                return RealityHackSettings.Instance.BranchSettings.GetRandomBranchPrefab();
        }
    }
}
