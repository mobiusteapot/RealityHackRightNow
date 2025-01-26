using UnityEngine;

// Empty class to mark which parent socket holds the first branch node
public class BranchParentSocket : MonoBehaviour
{
    [field: SerializeField]
    public BranchNode BranchNode { get; private set; }
}
