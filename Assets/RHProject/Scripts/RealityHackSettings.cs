using Mobtp.KettleTools.Core;
using UnityEngine;

// Singleton for 
[CreateAssetMenu(fileName = "RealityHackSettings", menuName = "RealityHack/VisualSettings")]
public class RealityHackSettings : SettingsSOSingleton<RealityHackSettings>
{
    public BranchSettings BranchSettings;
}
