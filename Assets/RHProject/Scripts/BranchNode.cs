using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    private void Start(){
        // Attempt count starts at 1
        AttemptCount = 1;
    }

    public void InitializeBranch(string topic)
    {
        SetBranchTopic(topic);
        // Start scale close to 0
        transform.localScale = Vector3.zero + Vector3.one * 0.01f;
        SetBranchGrow(1);
    }

    public void SetBranchTopic(string topic)
    {
        BranchTopic = topic;
        gameObject.name = $"BranchNode({topic})";
    }

    public bool TryAddNewBranch(string newBranchTopic, SequenceState topicState = SequenceState.Default)
    {
        // If topic state is not default, try to find a matching MajorBranchSocket
        if (topicState != SequenceState.Default)
        {
            MajorBranchSocket majorSocket = FindFirstMajorBranchSocketInSubtree(topicState);
            if (majorSocket != null)
            {
                return majorSocket.TryAddNewBranch(newBranchTopic, topicState);
            }
        }
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
                socket.TryAddNewBranch(newBranchTopic, topicState);

                if (socket.BranchNode != null)
                {
                    socket.BranchNode.IncrementTopicAttempts(newBranchTopic);
                }

                return true;
            }
        }

        foreach (var socket in BranchSockets)
        {
            if (socket.BranchNode != null && socket.BranchNode.TryAddNewBranch(newBranchTopic, topicState))
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
            SetBranchGrow(AttemptCount);
        }
    }

    public int GetTopicAttempts(string topic)
    {
        return topic == BranchTopic ? AttemptCount : 0;
    }

    public void SetBranchGrow(int curStep){
        // Start a coroutine to scale the branch up from 0 to 1 using BranchSettings
        StartCoroutine(ScaleBranch(curStep));
    }
    private IEnumerator ScaleBranch(int curStep)
    {
        var bs = RealityHackSettings.Instance.BranchSettings;
        float scale = bs.GetBranchScale(curStep);
        bool isFirstStep = curStep == 1;
        float duration = isFirstStep ? bs.BranchGrowDuration * 2 : bs.BranchGrowDuration;
        float elapsedTime = 0.0f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(scale, scale, scale);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0, 1, t);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
    }
        public MajorBranchSocket FindFirstMajorBranchSocketInSubtree(SequenceState desiredState)
    {
        // 1) Check *my* immediate sockets first
        foreach (var socket in BranchSockets)
        {
            // Is it a MajorBranchSocket?
            if (socket is MajorBranchSocket majorSock)
            {
                // Does it match the desired state?
                if (majorSock.AcceptedState == desiredState)
                {
                    return majorSock; // Found it!
                }
            }
        }

        // 2) If not found, recurse into child nodes
        foreach (var socket in BranchSockets)
        {
            if (socket.BranchNode != null)
            {
                MajorBranchSocket found = socket.BranchNode.FindFirstMajorBranchSocketInSubtree(desiredState);
                if (found != null)
                {
                    return found;
                }
            }
        }

        // Not found in this subtree
        return null;
    }
}
