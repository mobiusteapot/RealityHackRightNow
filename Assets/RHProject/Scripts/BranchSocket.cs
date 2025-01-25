using UnityEngine;

public class BranchSocket : MonoBehaviour
{
    public BranchNode BranchNode;

    public bool IsFull => BranchNode != null;

    public bool TryAddNewBranch(string topic)
    {
        if (IsFull)
        {
            return false;
        }

        var branchSettings = RealityHackSettings.Instance.BranchSettings;
        var rotOffset = transform.rotation * branchSettings.BranchRotationOffset;
        // Todo: Get the prefab from the settings asset
        var newBranchNodeObject = Instantiate(branchSettings.GetRandomBranchPrefab(), transform.position, rotOffset, transform);
        Debug.Log($"Instantiated new branch node object: {newBranchNodeObject.name}");
        newBranchNodeObject.transform.localScale = Vector3.one * branchSettings.BranchSizeMult;
        var newBranchNode = newBranchNodeObject.GetComponent<BranchNode>();

        if (newBranchNode == null)
        {
            Debug.LogError("The provided prefab does not have a BranchNode component.");
            Destroy(newBranchNodeObject);
            return false;
        }

        newBranchNode.InitializeBranch(topic);
        BranchNode = newBranchNode;
        return true;
    }
}
