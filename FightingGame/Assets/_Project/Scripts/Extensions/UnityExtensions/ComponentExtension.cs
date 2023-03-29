using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Explorer;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extension
{
    public static class ComponentExtension
    {
        public static T SetActive<T>(this T comp, bool active) where T : Component
        {
            comp.gameObject.SetActive(active);
            return comp;
        }

        public static bool ActiveSelf<T>(this T comp) where T : Component
        {
            return comp.gameObject.activeSelf;
        }

        public static AnimationClip FindAnimation(this Animator animator, string name)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.ToLower().Contains(name.ToLower()))
                {
                    return clip;
                }
            }

            return null;
        }

        public static Transform[] GetAllChildren(this Transform transform)
        {
            List<Transform> lst = new();
            foreach(Transform tr in transform)
                lst.Add(tr);
            return lst.ToArray();
        }
    }
}