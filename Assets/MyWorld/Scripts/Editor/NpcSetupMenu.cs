using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using MyWorld.Npc;

namespace MyWorld.EditorTools
{
    /// <summary>
    /// Menu: MyWorld → NPC → Setup Selected As Wandering NPC
    /// </summary>
    public static class NpcSetupMenu
    {
        [MenuItem("MyWorld/NPC/Setup Selected As Wandering NPC")]
        private static void SetupSelected()
        {
            var go = Selection.activeGameObject;
            if (go == null)
            {
                EditorUtility.DisplayDialog(
                    "NPC Setup",
                    "Select a character GameObject in the Hierarchy first.",
                    "OK");
                return;
            }

            var agent = go.GetComponent<NavMeshAgent>();
            if (agent == null) agent = Undo.AddComponent<NavMeshAgent>(go);
            agent.speed = 1.4f;
            agent.angularSpeed = 120f;
            agent.acceleration = 8f;
            agent.stoppingDistance = 0.35f;
            agent.height = 1.8f;
            agent.radius = 0.35f;

            if (go.GetComponent<NpcWander>() == null)
                Undo.AddComponent<NpcWander>(go);

            var anim = go.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                var wander = go.GetComponent<NpcWander>();
                var so = new SerializedObject(wander);
                so.FindProperty("animator").objectReferenceValue = anim;
                so.ApplyModifiedProperties();
            }

            EditorUtility.SetDirty(go);
            Debug.Log($"[NpcSetup] {go.name} ready — bake NavMesh (MASTER_GUIDE §16), then Play.", go);
        }

        [MenuItem("MyWorld/NPC/Setup Selected As Wandering NPC", true)]
        private static bool SetupSelectedValidate() => Selection.activeGameObject != null;
    }
}
