using System.Collections;
using System.Collections.Generic;
using Meta.XR.EnvironmentDepth;
using UnityEngine;
[RequireComponent(typeof(EnvironmentDepthManager))]
public class RemoveHands : MonoBehaviour
{
    [SerializeField] private EnvironmentDepthManager environmentDepthManager;
    private void Reset()
    {
        TryGetComponent(out environmentDepthManager);
    }
    private void Start()
    {
        environmentDepthManager.RemoveHands = true;
    }
}
