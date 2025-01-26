using UnityEngine;

public class MajorBranchSocket : BranchSocket
{
    [Header("Which major SequenceState does this socket fit?")]
    [SerializeField] private SequenceState acceptedState = SequenceState.Default;

    /// <summary>
    /// SequenceState that this major socket is designed to handle.
    /// </summary>
    public SequenceState AcceptedState => acceptedState;

    // Everything else behaves just like a normal socket,
    // so no override needed unless you want specialized logic.
}
