using UnityEditor;
using UnityEngine;
using MyWorld.World;

namespace MyWorld.EditorTools
{
    /// <summary>
    /// Menu: MyWorld → Animals → Setup Selected As Flying Bird (Pigeon)
    /// Wraps Tripo-style -90° meshes so flight faces the beak.
    /// </summary>
    public static class FlyingBirdSetupMenu
    {
        [MenuItem("MyWorld/Animals/Setup Selected As Flying Bird (Pigeon)")]
        private static void SetupPigeon()
        {
            var selected = Selection.activeGameObject;
            if (selected == null)
            {
                EditorUtility.DisplayDialog(
                    "Flying Bird Setup",
                    "Select your pigeon (Pigion) in the Hierarchy first.",
                    "OK");
                return;
            }

            Undo.SetCurrentGroupName("Setup Flying Bird");
            int group = Undo.GetCurrentGroup();

            Transform flyRoot = EnsureFlightRoot(selected);
            var fly = flyRoot.GetComponent<FlyingAnimal>();
            if (fly == null)
                fly = Undo.AddComponent<FlyingAnimal>(flyRoot.gameObject);

            fly.ApplyPigeonPreset();

            // Raise if sitting on the ground
            if (flyRoot.position.y < 4f)
            {
                Undo.RecordObject(flyRoot, "Raise bird");
                var p = flyRoot.position;
                p.y = 8f;
                flyRoot.position = p;
            }

            EditorUtility.SetDirty(flyRoot.gameObject);
            Selection.activeGameObject = flyRoot.gameObject;
            Undo.CollapseUndoOperations(group);

            Debug.Log(
                $"[FlyingBird] '{flyRoot.name}' ready — pigeon preset + obstacle avoidance. " +
                "Best placed near city / park rooftops (Y ≈ 8–20). Press Play.",
                flyRoot);
            EditorUtility.DisplayDialog(
                "Flying Bird Ready",
                "Pigeon setup done.\n\n" +
                "• FlyingAnimal added (obstacle avoidance on)\n" +
                "• Raised if it was on the ground\n" +
                "• Mesh kept as child if it had a -90° import tilt\n\n" +
                "Best place: CITY / PARK rooftops & plazas (pigeons are urban).\n" +
                "Press Play to watch it fly.",
                "OK");
        }

        [MenuItem("MyWorld/Animals/Setup Selected As Flying Bird (Pigeon)", true)]
        private static bool Validate() => Selection.activeGameObject != null;

        /// <summary>
        /// If the selection is a tilted mesh (common Tripo -90 X), parent it under a clean flight root.
        /// </summary>
        private static Transform EnsureFlightRoot(GameObject selected)
        {
            var existing = selected.GetComponent<FlyingAnimal>();
            if (existing != null)
                return selected.transform;

            // Already a wrapper? use it
            if (selected.name.EndsWith("_Fly") && selected.transform.childCount > 0)
                return selected.transform;

            bool looksLikeTiltedMesh =
                selected.GetComponent<MeshFilter>() != null
                && (Mathf.Abs(selected.transform.localEulerAngles.x - 270f) < 5f
                    || Mathf.Abs(selected.transform.localEulerAngles.x - 90f) < 5f
                    || Mathf.Abs(Mathf.DeltaAngle(selected.transform.localEulerAngles.x, -90f)) < 5f);

            if (!looksLikeTiltedMesh)
                return selected.transform;

            var parent = selected.transform.parent;
            var worldPos = selected.transform.position;
            var worldScale = selected.transform.lossyScale;

            var root = new GameObject(selected.name + "_Fly");
            Undo.RegisterCreatedObjectUndo(root, "Create flight root");
            root.transform.SetParent(parent, false);
            root.transform.position = worldPos;
            root.transform.rotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            Undo.SetTransformParent(selected.transform, root.transform, "Reparent bird mesh");
            selected.transform.localPosition = Vector3.zero;
            // Keep import tilt on the visual only
            selected.name = "Visual";

            // Preserve approximate world scale on the visual
            var ls = selected.transform.localScale;
            if (parent != null)
            {
                var ps = parent.lossyScale;
                ls = new Vector3(
                    SafeDiv(worldScale.x, ps.x),
                    SafeDiv(worldScale.y, ps.y),
                    SafeDiv(worldScale.z, ps.z));
            }
            else
            {
                ls = worldScale;
            }

            selected.transform.localScale = ls;
            return root.transform;
        }

        private static float SafeDiv(float a, float b) => Mathf.Abs(b) < 0.0001f ? a : a / b;
    }
}
