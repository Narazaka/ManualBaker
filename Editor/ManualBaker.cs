using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRC.SDK3.Avatars.Components;
using System.Reflection;

namespace net.narazaka.vrchat.manualbaker
{
    public class ManualBaker
    {
        [MenuItem("Tools/Manual Bake!", true)]
        static bool Check()
        {
            var avatar = Selection.activeGameObject;
            return avatar && avatar.GetComponent<VRCAvatarDescriptor>() != null;
        }

        [MenuItem("Tools/Manual Bake!", false)]
        static void Do()
        {
            EditorApplication.ExecuteMenuItem("Tools/Modular Avatar/Manual bake avatar");
            EditorApplication.ExecuteMenuItem("Tools/Avatar Optimizer/Manual Bake Avatar");

            var avatar = Selection.activeGameObject;
            if (avatar == null || avatar.GetComponent<VRCAvatarDescriptor>() == null) return;
            DestroyRecursive(avatar);
        }

        static void DestroyRecursive(GameObject subject)
        {
            var targets = FindDestroyTargets(subject);
            foreach (var target in targets)
            {
                Object.DestroyImmediate(target, false);
            }
        }

        static IEnumerable<GameObject> FindDestroyTargets(GameObject subject)
        {
            // FindGameObjectsWithTag does not return inactive GameObjects
            if (subject.tag == "EditorOnly")
            {
                return new List<GameObject> { subject };
            }
            else
            {
                var targets = new List<GameObject>();
                foreach (Transform child in subject.transform)
                {
                    targets.AddRange(FindDestroyTargets(child.gameObject));
                }
                return targets;
            }
        }
    }
}
