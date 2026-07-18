#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using MyWorld.Vehicles;

namespace MyWorld.EditorTools
{
    /// <summary>
    /// Fast car setup: select a car mesh in the Hierarchy, then
    /// MyWorld → Vehicles → Create Car Rig From Selection.
    /// </summary>
    public static class CarRigSetup
    {
        private const string PrefabFolder = "Assets/MyWorld/Prefabs/Vehicles";

        [MenuItem("MyWorld/Vehicles/Create Car Rig From Selection", false, 10)]
        public static void CreateCarRigFromSelection()
        {
            var meshRoot = Selection.activeGameObject;
            if (meshRoot == null)
            {
                EditorUtility.DisplayDialog(
                    "Car Rig",
                    "Select the car mesh (or model root) in the Hierarchy first.",
                    "OK");
                return;
            }

            Undo.IncrementCurrentGroup();
            int undo = Undo.GetCurrentGroup();

            string carName = meshRoot.name.StartsWith("Car_") ? meshRoot.name : "Car_" + meshRoot.name;
            var car = new GameObject(carName);
            Undo.RegisterCreatedObjectUndo(car, "Create Car Rig");
            car.transform.SetPositionAndRotation(meshRoot.transform.position, meshRoot.transform.rotation);

            Undo.SetTransformParent(meshRoot.transform, car.transform, "Parent Mesh");
            meshRoot.transform.localPosition = Vector3.zero;
            meshRoot.transform.localRotation = Quaternion.identity;

            Bounds bounds = GetWorldBounds(meshRoot);
            Vector3 localCenter = car.transform.InverseTransformPoint(bounds.center);
            Vector3 size = bounds.size;

            // Body collider
            var box = Undo.AddComponent<BoxCollider>(car);
            box.center = localCenter;
            box.size = new Vector3(
                Mathf.Max(0.5f, size.x * 0.9f),
                Mathf.Max(0.4f, size.y * 0.55f),
                Mathf.Max(0.5f, size.z * 0.9f));

            var rb = Undo.AddComponent<Rigidbody>(car);
            rb.mass = 1600f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Center of mass
            var com = new GameObject("COM");
            Undo.RegisterCreatedObjectUndo(com, "COM");
            com.transform.SetParent(car.transform, false);
            com.transform.localPosition = localCenter + Vector3.down * (size.y * 0.25f);

            // Wheels from bounds (assumes model faces +Z after you fix mesh rotation)
            float halfX = size.x * 0.35f;
            float halfZ = size.z * 0.35f;
            float wheelY = localCenter.y - size.y * 0.35f;
            float radius = Mathf.Clamp(size.y * 0.18f, 0.25f, 0.55f);

            var wcFL = CreateWheel(car.transform, "WC_FL", new Vector3(-halfX, wheelY, halfZ), radius);
            var wcFR = CreateWheel(car.transform, "WC_FR", new Vector3(halfX, wheelY, halfZ), radius);
            var wcRL = CreateWheel(car.transform, "WC_RL", new Vector3(-halfX, wheelY, -halfZ), radius);
            var wcRR = CreateWheel(car.transform, "WC_RR", new Vector3(halfX, wheelY, -halfZ), radius);

            var seat = new GameObject("Seat");
            Undo.RegisterCreatedObjectUndo(seat, "Seat");
            seat.transform.SetParent(car.transform, false);
            seat.transform.localPosition = localCenter + new Vector3(-size.x * 0.15f, size.y * 0.05f, size.z * 0.05f);

            var exit = new GameObject("ExitPoint");
            Undo.RegisterCreatedObjectUndo(exit, "ExitPoint");
            exit.transform.SetParent(car.transform, false);
            exit.transform.localPosition = localCenter + new Vector3(-size.x * 0.7f, 0f, size.z * 0.05f);

            var vehicleSeat = Undo.AddComponent<VehicleSeat>(car);
            var enterExit = Undo.AddComponent<VehicleEnterExit>(car);
            var controller = Undo.AddComponent<CarController>(car);

            // Wire VehicleSeat.exitPoint
            var seatSo = new SerializedObject(vehicleSeat);
            seatSo.FindProperty("exitPoint").objectReferenceValue = exit.transform;
            seatSo.ApplyModifiedPropertiesWithoutUndo();

            // Wire CarController wheels + COM
            var carSo = new SerializedObject(controller);
            carSo.FindProperty("wheelFL").objectReferenceValue = wcFL;
            carSo.FindProperty("wheelFR").objectReferenceValue = wcFR;
            carSo.FindProperty("wheelRL").objectReferenceValue = wcRL;
            carSo.FindProperty("wheelRR").objectReferenceValue = wcRR;
            carSo.FindProperty("centerOfMass").objectReferenceValue = com.transform;
            carSo.FindProperty("wheelRadius").floatValue = radius;
            carSo.ApplyModifiedPropertiesWithoutUndo();

            // Wire EnterExit → controller
            var eeSo = new SerializedObject(enterExit);
            var ctrlProp = eeSo.FindProperty("controller");
            if (ctrlProp != null) ctrlProp.objectReferenceValue = controller;
            eeSo.ApplyModifiedPropertiesWithoutUndo();

            Selection.activeGameObject = car;
            Undo.CollapseUndoOperations(undo);

            EditorUtility.DisplayDialog(
                "Car Rig Created",
                "Done.\n\n" +
                "1) Blue Z arrow on the root must point out the HOOD.\n" +
                "   If not: rotate only the MESH child by Y=180.\n" +
                "2) Nudge WC_FL / FR / RL / RR onto the visual wheels.\n" +
                "3) Move Seat into the driver seat.\n" +
                "4) Play → E to enter.\n" +
                "5) MyWorld → Vehicles → Save Selected Car As Prefab",
                "OK");
        }

        [MenuItem("MyWorld/Vehicles/Create Car Rig From Selection", true)]
        private static bool ValidateCreate() => Selection.activeGameObject != null;

        [MenuItem("MyWorld/Vehicles/Save Selected Car As Prefab", false, 11)]
        public static void SaveSelectedAsPrefab()
        {
            var car = Selection.activeGameObject;
            if (car == null)
            {
                EditorUtility.DisplayDialog("Save Prefab", "Select the car root in the Hierarchy.", "OK");
                return;
            }

            if (!AssetDatabase.IsValidFolder("Assets/MyWorld"))
                AssetDatabase.CreateFolder("Assets", "MyWorld");
            if (!AssetDatabase.IsValidFolder("Assets/MyWorld/Prefabs"))
                AssetDatabase.CreateFolder("Assets/MyWorld", "Prefabs");
            if (!AssetDatabase.IsValidFolder(PrefabFolder))
                AssetDatabase.CreateFolder("Assets/MyWorld/Prefabs", "Vehicles");

            string path = $"{PrefabFolder}/{car.name}.prefab";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            PrefabUtility.SaveAsPrefabAssetAndConnect(car, path, InteractionMode.UserAction);
            EditorUtility.DisplayDialog("Saved", $"Prefab:\n{path}\n\nNext car: duplicate this prefab, swap the mesh child.", "OK");
        }

        [MenuItem("MyWorld/Vehicles/Duplicate Selected Car (fast clone)", false, 12)]
        public static void DuplicateSelectedCar()
        {
            var src = Selection.activeGameObject;
            if (src == null) return;

            var clone = Object.Instantiate(src);
            Undo.RegisterCreatedObjectUndo(clone, "Duplicate Car");
            clone.name = src.name + "_Copy";
            clone.transform.position = src.transform.position + src.transform.right * 4f;
            Selection.activeGameObject = clone;
        }

        private static WheelCollider CreateWheel(Transform parent, string name, Vector3 localPos, float radius)
        {
            var go = new GameObject(name);
            Undo.RegisterCreatedObjectUndo(go, name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.identity;
            var wc = Undo.AddComponent<WheelCollider>(go);
            wc.radius = radius;
            wc.suspensionDistance = 0.25f;
            return wc;
        }

        private static Bounds GetWorldBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>();
            if (renderers == null || renderers.Length == 0)
                return new Bounds(root.transform.position, new Vector3(2f, 1.5f, 4f));

            Bounds b = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                b.Encapsulate(renderers[i].bounds);
            return b;
        }
    }
}
#endif
