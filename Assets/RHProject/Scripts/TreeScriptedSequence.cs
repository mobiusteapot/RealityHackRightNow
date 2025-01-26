using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScriptedSequence : MonoBehaviour
{
    // Singleton that can be called to get the right scripted tree branch
    private static TreeScriptedSequence _instance;
    public static TreeScriptedSequence Instance;

    SequenceState sequenceState = SequenceState.FirstBranch;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public SequenceState GetCurrentState(){
        var ssOut = sequenceState;
        // Increment the state to the next state, or if at the last state, stay at the last state
        sequenceState = sequenceState == SequenceState.Default ? SequenceState.Default : sequenceState + 1;
        return ssOut;
    }
}
