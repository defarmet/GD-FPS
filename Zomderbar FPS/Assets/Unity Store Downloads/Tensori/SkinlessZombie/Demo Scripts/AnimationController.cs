using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkinlessZombie
{
    public class AnimationController : MonoBehaviour
    {
        public Animator ZombieAnimator;
        [SerializeField] private Dropdown AnimationDropdown = null;
        private AnimationItemData currentanimationItemData;
        private GameObject weaponPrefabObject;
        private Vector3 weaponprefabRot;
        public Transform weaponPivot;
        public Transform rightHand;
        [SerializeField] private List<AnimationItemData> animationItemDatabase = new List<AnimationItemData>();
        Object[] animationDatabase;
        protected void Awake()
        {
            animationDatabase = Resources.LoadAll("ScriptableObjects/Animations");
        }

        private void Start()
        {
            ListAllAnimationDataClips();
            AnimationDropdown.onValueChanged.AddListener(UpdateDropDown);
            CreateDropDownDatabase();
            UpdateDropDown(0);
        }

        private void UpdateDropDown(int selectedIndex)
        {
            currentanimationItemData = animationItemDatabase[selectedIndex];
            if (currentanimationItemData.weaponPrefab != null)
            {
                weaponPrefabObject.SetActive(true);
            }
            else
            {
                weaponPrefabObject.SetActive(false);
            }
            ZombieAnimator.Play(currentanimationItemData.animationClip.name);

        }

        private void CreateDropDownDatabase()
        {
            var newOptionsData = new List<Dropdown.OptionData>();

            for (int i = 0; i < animationItemDatabase.Count; i++)
            {
                var currentClip = animationItemDatabase[i].animationClip;

                newOptionsData.Add(new Dropdown.OptionData()
                {
                    text = currentClip.name,
                });
            }

            AnimationDropdown.ClearOptions();
            AnimationDropdown.AddOptions(newOptionsData);
        }

        private void ListAllAnimationDataClips()
        {
            for (int i = 0; i < animationDatabase.Length; i++)
            {
                var animationItemData = animationDatabase[i] as AnimationItemData;
                GameObject _weaponPrefab = animationItemData.weaponPrefab;
                weaponprefabRot = animationItemData.rotationvalues;
                if (_weaponPrefab != null)
                {
                    weaponPrefabObject = Instantiate(animationItemData.weaponPrefab) as GameObject;
                    weaponPrefabObject.transform.parent = rightHand;
                    weaponPrefabObject.transform.position = weaponPivot.transform.position;
                    weaponPrefabObject.transform.localRotation = Quaternion.Euler(weaponprefabRot);
                    weaponPrefabObject.SetActive(false);
                }
                animationItemDatabase.Add(animationItemData);
            }
        }
    }
}
