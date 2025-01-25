using System.Linq;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mobtp.KettleTools.Core
{

    /// <summary>
    /// ScriptableObject singleton that persists between scenes and is accessible from anywhere.
    /// Works on both runtime and editor. Intended for settings only.
    /// </summary>
    /// <typeparam name="T"></typeparam>

    // Execution order is set to -150 to ensure it loads before TextMeshPro things, which seems like a common usecase
    // Unsure what other complications this may cause. Would love feedback from anyone who's familiar with these systems.
    //
    // This asset is for runtime reading from only. If you want gameplay data to be saved, use a different system.
    [DefaultExecutionOrder(-150)]
    public abstract class SettingsSOSingleton<T> : ScriptableObject where T : Object
    {
        public static T _instance;
        // Loads lazily if hasn't been otherwise accessed yet. Should be initialized in OnEnable beforehand
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                // On runtime, should already be initialized, so this potentially expensive/frequent check can be skipped
                if (_instance == null)
                {
                    LoadSettings();
                }
#endif
                return _instance;
            }
            private set => _instance = value;
        }
        private bool hasBeenCreated = false;

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (_instance != null && _instance != this)
            {
                Debug.LogError("Multiple instances of " + typeof(T).Name + " found. Only one should exist in the project.");
                return;
            }
#endif

            _instance = this as T;

#if UNITY_EDITOR
            if (!hasBeenCreated)
            {
                hasBeenCreated = true;
                AddToPreloadedAssets();
            }
            else
            {
                ValidateInPreloadedAssets();
            }
#endif
        }
        // Todo: Clean up added assets on reload

#if UNITY_EDITOR
        public static void AddToPreloadedAssets()
        {
            // Add the config asset to the build
            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            // Prevent duplicates of settings (since they are referenced via instance)
            preloadedAssets.RemoveAll(x => x != null && x.GetType() == typeof(T));
            preloadedAssets.Add(_instance);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
        // As long as it has been added to the preloaded assets, it will be loaded on build
        // Should be loaded automatically on first access, but is loaded "lazily" if not otherwise accessed.
        // I have concerns about race conditions, but InitializeOnLoad and RuntimeInitializeOnLoad both don't work correctly with generics
        private static void LoadSettings()
        {
            if (_instance == null)
            {
                _instance = PlayerSettings.GetPreloadedAssets().OfType<T>().FirstOrDefault();
            }
            // If still null, try to load from assetdatabase
            if (_instance == null)
            {
                var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                if (guids.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = AssetDatabase.LoadAssetAtPath<T>(path);
                }
            }
        }
        // Check if asset is already in preloaded assets, if not, add it
        public static void ValidateInPreloadedAssets()
        {
            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            if (!preloadedAssets.Contains(_instance))
            {
                AddToPreloadedAssets();
            }
        }
#endif
    }
}