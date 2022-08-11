using UnityEngine;

namespace SkinlessZombie
{
    [CreateAssetMenu(fileName = "New Animationclip", menuName = "Create new Animationclip")]
    public class AnimationItemData : ScriptableObject
    {
        public AnimationClip animationClip;
        public GameObject weaponPrefab;
        public Vector3 rotationvalues;
    }
    
}