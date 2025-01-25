using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class BranchNode : MonoBehaviour
{
    public List<BranchSocket> BranchSockets = new List<BranchSocket>();

    [SerializeField] private MeshRenderer mr;

    [field: SerializeField] public string BranchTopic { get; private set; }

    [field: SerializeField] public int AttemptCount { get; private set; }

    private void Reset()
    {
        // Grab the mesh renderer and any BranchSockets under this node
        TryGetComponent(out mr);
        BranchSockets.AddRange(GetComponentsInChildren<BranchSocket>());
    }

    public void InitializeBranch(string topic)
    {
        SetBranchTopic(topic);
    }

    public void SetBranchTopic(string topic)
    {
        BranchTopic = topic;
        gameObject.name = $"BranchNode({topic})";
    }

    public bool TryAddNewBranch(string newBranchTopic)
    {
        BranchNode existingNode = FindNodeInSubtree(newBranchTopic);
        if (existingNode != null)
        {
            existingNode.IncrementTopicAttempts(newBranchTopic);
            return true;
        }

        foreach (var socket in BranchSockets)
        {
            if (!socket.IsFull)
            {
                socket.TryAddNewBranch(newBranchTopic);

                if (socket.BranchNode != null)
                {
                    socket.BranchNode.IncrementTopicAttempts(newBranchTopic);
                }

                return true;
            }
        }

        foreach (var socket in BranchSockets)
        {
            if (socket.BranchNode != null && socket.BranchNode.TryAddNewBranch(newBranchTopic))
            {
                return true;
            }
        }
        return false;
    }

    public BranchNode FindNodeInSubtree(string topic)
    {
        if (BranchTopic == topic)
        {
            return this;
        }

        foreach (var socket in BranchSockets)
        {
            if (socket.BranchNode == null) continue;

            var found = socket.BranchNode.FindNodeInSubtree(topic);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds a branch with "newBranchTopic" as a child of the node whose topic matches "topic".
    /// If this node matches, it calls TryAddNewBranch. Otherwise, it recurses through children.
    /// </summary>
    public bool AddBranchAtTopic(string topic, string newBranchTopic)
    {
        if (BranchTopic == topic)
        {
            return TryAddNewBranch(newBranchTopic);
        }

        // Otherwise, recurse down the subtree
        foreach (var socket in BranchSockets)
        {
            if (socket.BranchNode != null && socket.BranchNode.AddBranchAtTopic(topic, newBranchTopic))
            {
                return true;
            }
        }

        return false;
    }

    public void PrintTree(int depth = 0)
    {
        string indent = new string('-', depth * 2);
        Debug.Log($"{indent}{BranchTopic} (Attempts: {AttemptCount})");

        foreach (var socket in BranchSockets)
        {
            if (socket.BranchNode != null)
            {
                socket.BranchNode.PrintTree(depth + 1);
            }
        }
    }

    public void IncrementTopicAttempts(string topic)
    {
        if (topic == BranchTopic)
        {
            AttemptCount++;
        }
    }

    public int GetTopicAttempts(string topic)
    {
        return topic == BranchTopic ? AttemptCount : 0;
    }
}
