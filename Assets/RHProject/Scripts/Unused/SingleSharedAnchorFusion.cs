using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;                // For NetworkBehaviour, Object.HasStateAuthority, etc.
using Meta.XR;             // For OVRSpatialAnchor, OVRAnchor, etc.
using Oculus.Platform;      // For Core.Initialize, OVRColocationSession
using Oculus.Platform.Models;

public class SingleSharedAnchorFusion : NetworkBehaviour
{
    [Header("Reference to the single, pre-placed OVRSpatialAnchor in the scene")]
    [SerializeField] private OVRSpatialAnchor anchorInScene;

    private Guid groupGuid = Guid.Empty;

    // Called after this object is spawned by Fusion on each client
    public override void Spawned()
    {
        // We skip room joining logic because that's handled elsewhere.

        // If we have State Authority (Fusion’s “host”), we advertise & share.
        // Otherwise, we discover & load.
        if (Object.HasStateAuthority)
        {
            Debug.Log("[Host] Starting advertisement & sharing anchor...");
            // Wrap in an async method
            _ = HostFlowAsync();
        }
        else
        {
            Debug.Log("[Client] Starting discovery & loading anchor...");
            _ = ClientFlowAsync();
        }
    }

    // --------------------------
    // HOST FLOW
    // --------------------------
    private async Task HostFlowAsync()
    {
        // 1) Initialize Oculus if needed. (Safe to call multiple times if your app hasn't done so.)
        InitializeOculusPlatform();

        // 2) Start advertisement to create the colocation group
        var advertResult = await OVRColocationSession.StartAdvertisementAsync(null /* optional metadata */);
        Debug.Log($"Advert result: {advertResult.Status}");
        if (!advertResult.Success || !advertResult.TryGetValue(out groupGuid))
        {
            Debug.LogError("StartAdvertisementAsync failed! No group created.");
            return;
        }
        Debug.Log($"[Host] groupGuid = {groupGuid}");

        // 3) Ensure our single anchor is saved so it can be shared
        await EnsureAnchorIsSaved(anchorInScene);

        // 4) Share anchor with that group
        Debug.Log($"[Host] Sharing anchor ID={anchorInScene.Uuid} with group {groupGuid}");
        var shareResult = await OVRSpatialAnchor.ShareAsync(new[] { anchorInScene }, groupGuid);
        if (!shareResult.Success)
        {
            Debug.LogError($"ShareAsync failed: {shareResult.Status}");
            return;
        }
        Debug.Log("[Host] Anchor shared successfully!");
    }

    // --------------------------
    // CLIENT FLOW
    // --------------------------
    private async Task ClientFlowAsync()
    {
        // 1) Initialize Oculus if needed
        InitializeOculusPlatform();

        // 2) Listen for discovered sessions
        OVRColocationSession.ColocationSessionDiscovered += OnColocationSessionDiscovered;

        // 3) Start discovering sessions
        var discoResult = await OVRColocationSession.StartDiscoveryAsync();
        Debug.Log($"Discovery result: {discoResult.Status}");
        if (!discoResult.Success)
        {
            Debug.LogError("StartDiscoveryAsync failed!");
            return;
        }

        // 4) Wait a bit (or add a UI event) so the host can advertise, and we can discover
        Debug.Log("Waiting 5 seconds for group discovery...");
        await Task.Delay(5000);

        if (groupGuid == Guid.Empty)
        {
            Debug.LogError("[Client] No group discovered! Cannot load anchor.");
            return;
        }

        // 5) Load the anchor from that group
        await LoadAnchorFromGroup(groupGuid, anchorInScene);

        // 6) Optionally stop discovery now that we have what we need
        await OVRColocationSession.StopDiscoveryAsync();
        OVRColocationSession.ColocationSessionDiscovered -= OnColocationSessionDiscovered;
    }

    // Invoked when a group is discovered
    private void OnColocationSessionDiscovered(OVRColocationSession.Data data)
    {
        Debug.Log($"OnColocationSessionDiscovered: {data.AdvertisementUuid}");
        groupGuid = data.AdvertisementUuid;  // store the discovered group
    }

    // --------------------------
    // Helper: Ensure an anchor is saved
    // --------------------------
    private async Task EnsureAnchorIsSaved(OVRSpatialAnchor anchor)
    {
        // 1) If the anchor was never fully created, wait
        await anchor.WhenCreatedAsync();

        // 2) Attempt to save
        var saveResult = await anchor.SaveAnchorAsync();
        if (!saveResult.Success)
        {
            Debug.LogError($"SaveAnchorAsync failed: {saveResult.Status}");
        }
        else
        {
            Debug.Log($"Anchor saved: {anchor.Uuid}");
        }
    }

    // --------------------------
    // Helper: Load an anchor from a group into the same anchorInScene
    // --------------------------
    private async Task LoadAnchorFromGroup(Guid group, OVRSpatialAnchor localAnchor)
    {
        Debug.Log($"Loading anchor(s) from group: {group}");

        var unboundList = new List<OVRSpatialAnchor.UnboundAnchor>();
        var loadResult = await OVRSpatialAnchor.LoadUnboundSharedAnchorsAsync(group, unboundList);
        Debug.Log($"LoadUnboundSharedAnchorsAsync: {loadResult.Status}");
        if (!loadResult.Success)
        {
            Debug.LogError("Failed to load from group " + group);
            return;
        }

        if (unboundList.Count == 0)
        {
            Debug.LogWarning("No anchors found in group " + group);
            return;
        }

        var unbound = unboundList[0]; // We only want 1 anchor
        Debug.Log($"Found unbound anchor: {unbound.Uuid}");

        // The user specifically wants to re-use the same anchorInScene, not create new anchors
        localAnchor.gameObject.SetActive(false); // optional to avoid flicker

        try
        {
            unbound.BindTo(localAnchor);
        }
        catch (Exception e)
        {
            Debug.LogError($"BindTo() failed: {e.Message}");
            return;
        }

        bool localized = await localAnchor.WhenLocalizedAsync();
        if (!localized)
        {
            Debug.LogError("Anchor never localized!");
            return;
        }

        localAnchor.gameObject.SetActive(true);
        Debug.Log("Anchor loaded & localized from group!");
    }

    // --------------------------
    // Oculus Platform init
    // --------------------------
    private bool oculusInitialized = false;
    private void InitializeOculusPlatform()
    {
        if (oculusInitialized)
            return;

        try
        {
            Core.Initialize();
            Debug.Log("Oculus Platform initialized");
            oculusInitialized = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Oculus Platform may already be initialized or not needed. " + e.Message);
        }
    }

    // --------------------------
    // Cleanup
    // --------------------------
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);

        // If state authority: Stop advertisement
        if (Object.HasStateAuthority)
        {
            _ = OVRColocationSession.StopAdvertisementAsync();
        }
        else
        {
            _ = OVRColocationSession.StopDiscoveryAsync();
            OVRColocationSession.ColocationSessionDiscovered -= OnColocationSessionDiscovered;
        }
    }
}
