using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Game.Ctrller.Character
{
    public sealed class CharacterSkinReplacer
    {
        private readonly Transform _tempParent;
        private readonly ExposedList<Slot> _currentSkinSlots;

        private readonly bool _enableDebug;

        /*
         *  public:
         */

        public CharacterSkinReplacer(GameObject skeletonAnimObj, Transform tempParent)
        {
            _tempParent = tempParent != null ? tempParent : throw new ArgumentNullException(nameof(tempParent));

            _currentSkinSlots = GetSlotsOnObject(skeletonAnimObj) ?? throw new ArgumentNullException(nameof(_currentSkinSlots));          
        }
        public void SwitchSkin(GameObject[] targetSkins)
        {
            // clean parent
            for (int i = 0; i < _tempParent.childCount; i++)
            {
                UnityEngine.Object.Destroy(_tempParent.GetChild(i).gameObject);
            }

            foreach (var skin in targetSkins)
            {
                SwitchSkinOnAttachment(skin);
            }
        }


        /*
         *  private:
         */

        private CharacterSkinReplacer() { }
        private void SwitchSkinOnAttachment(GameObject targetSkinObj)
        {
            var target = UnityEngine.Object.Instantiate(targetSkinObj, _tempParent);
            target.SetActive(false);

            var targetSkinSlots = GetSlotsOnObject(target);

            foreach (var currentSlot in _currentSkinSlots)
            {
                bool hasMatched = false;

                foreach (var targetSlot in targetSkinSlots)
                {
                    if (currentSlot.Data.Name == targetSlot.Data.Name && targetSlot.Attachment != null)
                    {
                        hasMatched = true;
                        currentSlot.Attachment = targetSlot.Attachment;

                        if (_enableDebug)
                            Debug.Log($"已更换 {currentSlot} 上的皮肤为 {targetSlot.Attachment.Name}.");
                    }
                }

                if (!hasMatched && _enableDebug) 
                    Debug.Log($"没有与 {currentSlot.Data.Name} 匹配的皮肤插槽.");
            }

        }
        private ExposedList<Slot> GetSlotsOnObject(GameObject obj)
        {
            SkeletonMecanim skeletonMecanim = 
                obj.GetComponent<SkeletonMecanim>() != null ? obj.GetComponent<SkeletonMecanim>() :
                throw new NullReferenceException($"[Spine] {obj} {nameof(SkeletonMecanim)} component not found.");

            Skeleton skeleton =
                skeletonMecanim.Skeleton ??
                throw new NullReferenceException($"[Spine] {obj} skeleton not found.");

            return
                skeleton.Slots ??
                throw new NullReferenceException($"[Spine] {obj} slot not found.");
        }
        private void PrintDebugInfo(GameObject targetSkinObj)
        {
            string msg = $"{targetSkinObj.name}: ";

            foreach (var slot in GetSlotsOnObject(targetSkinObj))
                msg += $", {slot.Data.Name}";

            Debug.Log(msg);
        }
    }
}