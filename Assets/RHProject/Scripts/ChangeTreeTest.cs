using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class ChangeTreeTest : NetworkBehaviour
{
    [SerializeField]
    private BranchNode parentBranchNode;

    [SerializeField]
    private InputField inputField;

    public void OnInputSent()
    {
        Debug.Log("OnInputSent from ChangeTreeTest");
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

            RPC_RequestAddBranch(inputField.text);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestAddBranch(string topic, RpcInfo info = default)
    {
        bool success = parentBranchNode.TryAddNewBranch(topic);
        Debug.Log($"[Server] TryAddNewBranch({topic}) success = {success}");

        RPC_ApplyAddBranch(topic);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ApplyAddBranch(string topic, RpcInfo info = default)
    {
        parentBranchNode.TryAddNewBranch(topic);

        Debug.Log($"[All Clients] TryAddNewBranch({topic})");
    }
}
