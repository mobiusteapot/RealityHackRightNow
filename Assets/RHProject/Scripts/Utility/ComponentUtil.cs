using System.Collections.Generic;
using UnityEngine;

namespace Mobtp.KT.Core.Utils {
    public static class ComponentUtil {
        // Iterate through all children of the gameobject and try to get the component
        // If searchAllChildren is true, then recursively search through children's children
        /// <summary>
        /// Iterates through all immediate children of a game object and tries to get the component.
        /// If searchAllChildren is true, then recursively searches through children's children.
        /// Intended for limited use. This operation is potentially expensive.
        /// </summary>
        /// <typeparam name="T">Type of component to search for.</typeparam>
        /// <param name="gameObject">The GameObject in the hiearchy to search the children of.</param>
        /// <param name="component">The component that has been obtained, or null if no component has been found.</param>
        /// <param name="searchAllChildren">If true, searches all children for the given component.</param>
        /// <returns>True if component is found, false if it is not.</returns>
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool searchAllChildren = false) where T : Component{
            component = null;
            GameObject[] children = new GameObject[gameObject.transform.childCount];
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                children[i] = gameObject.transform.GetChild(i).gameObject;
            }
            for (int i = 0; i < children.Length; i++) {
                if (children[i].TryGetComponent(out component)) {
                    return true;
                }
                else if (searchAllChildren) {
                    if (children[i].TryGetComponentInChildren(out component, true)) {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// <inheritdoc cref="TryGetComponentInChildren"/>
        /// </summary>
        /// <typeparam name="T"><inheritdoc cref="TryGetComponentInChildren" path="/typeparam"/></typeparam>
        /// <param name="component">The component who's GameObject hiearchy should be searched for any matching children.</param>
        /// <param name="result"><inheritdoc cref="TryGetComponentInChildren" path="/param"/></param>
        /// <param name="searchAllChildren"><inheritdoc cref="TryGetComponentInChildren" path="/param"/></param>
        /// <returns><inheritdoc cref="TryGetComponentInChildren" path="/param"/></returns>
        public static bool TryGetComponentInChildren<T>(this Component component, out T result, bool searchAllChildren = false) where T : Component {
            return component.gameObject.TryGetComponentInChildren(out result, searchAllChildren);
        }
        /// <summary>
        /// Iterates through all immediate children of a game object and tries to an array of components.
        /// </summary>
        /// <typeparam name="T"><inheritdoc cref="TryGetComponentInChildren" path="/typeparam"/></typeparam>
        /// <param name="gameObject"><inheritdoc cref="TryGetComponentInChildren" path="/param"/></param>
        /// <param name="components">The components that have been obtained, or null if no components have been found.</param>
        /// <param name="searchAllChildren"><inheritdoc cref="TryGetComponentInChildren" path="/param"/></param>
        /// <returns>True if any component is found, false if none are found.</returns>
        public static bool TryGetComponentsInChildren<T>(this GameObject gameObject, out T[] components, bool searchAllChildren = false) where T : Component {
            components = gameObject.GetComponentsInChildren<T>();
            if (components.Length > 0) {
                return true;
            }
            else if (searchAllChildren) {
                GameObject[] children = new GameObject[gameObject.transform.childCount];
                for (int i = 0; i < gameObject.transform.childCount; i++) {
                    children[i] = gameObject.transform.GetChild(i).gameObject;
                }
                for (int i = 0; i < children.Length; i++) {
                    if (children[i].TryGetComponentsInChildren(out components, true))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// <inheritdoc cref="TryGetComponentsInChildren"/>
        /// </summary>
        /// <typeparam name="T"><inheritdoc cref="TryGetComponentInChildren"/></typeparam>
        /// <param name="component"><inheritdoc cref="TryGetComponentInChildren{T}(Component, out T, bool)" path="/param[@name='component']" /></param>
        /// <param name="result"><inheritdoc cref="TryGetComponentsInChildren" path="/param[@name='searchAllChildren']" /></param>
        /// <param name="searchAllChildren"><inheritdoc cref="TryGetComponentInChildren"/></param>
        /// <returns><inheritdoc cref="TryGetComponentsInChildren"/></returns>
        public static bool TryGetComponentsInChildren<T>(this Component component, out T[] result, bool searchAllChildren = false) where T : Component {
            return component.gameObject.TryGetComponentsInChildren(out result, searchAllChildren);
        }

                /// <summary>
        /// <inheritdoc cref="TryGetComponentsInChildren"/>
        /// </summary>
        /// <typeparam name="T"><inheritdoc cref="TryGetComponentInChildren"/></typeparam>
        /// <param name="component"><inheritdoc cref="TryGetComponentInChildren{T}(Component, out T, bool)" path="/param[@name='component']" /></param>
        /// <param name="result"><inheritdoc cref="TryGetComponentsInChildren" path="/param[@name='searchAllChildren']" /></param>
        /// <param name="searchAllChildren"><inheritdoc cref="TryGetComponentInChildren"/></param>
        /// <returns><inheritdoc cref="TryGetComponentsInChildren"/></returns>
        public static bool TryGetComponentsInChildren<T>(this GameObject gameObject, out List<T> result, bool searchAllChildren = false) where T : Component {
            result = new List<T>(gameObject.GetComponentsInChildren<T>());
            if (result.Count > 0) {
                return true;
            }
            else if (searchAllChildren) {
                GameObject[] children = new GameObject[gameObject.transform.childCount];
                for (int i = 0; i < gameObject.transform.childCount; i++) {
                    children[i] = gameObject.transform.GetChild(i).gameObject;
                }
                for (int i = 0; i < children.Length; i++) {
                    if (children[i].TryGetComponentsInChildren(out result, true))
                        return true;
                }
            }
            return false;
        }

        // Similar method but outputs a list instead of an array

        /// <summary>
        /// <inheritdoc cref="TryGetComponentsInChildren"/>
        /// </summary>
        /// <typeparam name="T"><inheritdoc cref="TryGetComponentInChildren"/></typeparam>
        /// <param name="component"><inheritdoc cref="TryGetComponentInChildren{T}(Component, out T, bool)" path="/param[@name='component']" /></param>
        /// <param name="result"><inheritdoc cref="TryGetComponentsInChildren" path="/param[@name='searchAllChildren']" /></param>
        /// <param name="searchAllChildren"><inheritdoc cref="TryGetComponentInChildren"/></param>
        /// <returns><inheritdoc cref="TryGetComponentsInChildren"/></returns>
        public static bool TryGetComponentsInChildren<T>(this Component component, out List<T> result, bool searchAllChildren = false) where T : Component {
            result = new List<T>(component.GetComponentsInChildren<T>());
            if (result.Count > 0) {
                return true;
            }
            else if (searchAllChildren) {
                GameObject[] children = new GameObject[component.transform.childCount];
                for (int i = 0; i < component.transform.childCount; i++) {
                    children[i] = component.transform.GetChild(i).gameObject;
                }
                for (int i = 0; i < children.Length; i++) {
                    if (children[i].TryGetComponentsInChildren(out result, true))
                        return true;
                }
            }
            return false;
        }
    }
}