using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyWorld.Editor
{
    /// <summary>
    /// One-click fixes for common console spam in this project.
    /// </summary>
    public static class SceneIssueFixer
    {
        [MenuItem("My World/Fix Console Issues (Scene)")]
        public static void FixOpenScenes()
        {
            int lights = 0, boats = 0, terrains = 0;

            foreach (var light in Object.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                // URP only supports baked area lights
                if (light.type == LightType.Rectangle || light.type == LightType.Disc)
                {
                    if (light.lightmapBakeType != LightmapBakeType.Baked)
                    {
                        Undo.RecordObject(light, "Bake area light");
                        light.lightmapBakeType = LightmapBakeType.Baked;
                        lights++;
                        EditorUtility.SetDirty(light);
                    }
                }
            }

            foreach (var rb in Object.FindObjectsByType<Rigidbody>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (rb.isKinematic) continue;
                foreach (var mc in rb.GetComponentsInChildren<MeshCollider>(true))
                {
                    if (!mc.convex)
                    {
                        Undo.RecordObject(mc, "Make MeshCollider convex");
                        mc.convex = true;
                        boats++;
                        EditorUtility.SetDirty(mc);
                    }
                }
            }

            foreach (var terrain in Object.FindObjectsByType<Terrain>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                var tc = terrain.GetComponent<TerrainCollider>();
                if (tc != null && tc.enableTreeColliders)
                {
                    // Tree MeshColliders vs Terrain spam "MeshCollider is not supported on terrain"
                    Undo.RecordObject(tc, "Disable terrain tree colliders");
                    tc.enableTreeColliders = false;
                    terrains++;
                    EditorUtility.SetDirty(tc);
                }
            }

            if (lights + boats + terrains > 0)
                EditorSceneManager.MarkAllScenesDirty();

            Debug.Log($"[My World] Fixed console issues — area lights baked: {lights}, mesh colliders convex: {boats}, terrain tree colliders off: {terrains}");
        }
    }
}
