using Core.Extension;
using Core.SO;
using Core.Utility;
using System.Linq;
using UnityEngine;

namespace Core.Gameplay
{
    public partial class CharacterRenderer
    {
        [SerializeField][DebugOnly] protected Transform[] _attackBounds;
        [SerializeField][DebugOnly] protected Transform[] _attackFXs;

        protected virtual void DoneGotHit()
        {
            _controller.CharacterState = ECharacterState.IDLE;
        }

        protected virtual void DoneLightAtk1()
        {
            _controller.CharacterState = ECharacterState.IDLE;
        }

        protected virtual void DoneLightAtk2()
        {
            _controller.CharacterState = ECharacterState.IDLE;
        }

        protected virtual void DoneLightAtk3()
        {
            _controller.CharacterState = ECharacterState.IDLE;
        }

        protected virtual void DoneHeavyAtk()
        {
            _controller.CharacterState = ECharacterState.IDLE;
        }

        protected virtual void DoneSkill1()
        {
            _controller.CharacterState = ECharacterState.IDLE;
        }

        protected virtual void DoneSkill2()
        {
            _controller.CharacterState = ECharacterState.IDLE;
        }

        protected virtual void SetupAttackCollision(Transform tr, AttackTypeIndex atkIdx)
        {
            tr.GetComponent<AttackContainer>().
                Setup(_controller.CharacterConfigSO.
                    AttackSOs[(int)atkIdx]);

            tr.SetActive(true);
        }

        protected virtual void SpawnSkill1FX()
        {
            SetupAttackCollision(_attackFXs[0], AttackTypeIndex.Skill1);
        }

        protected virtual void SpawnSkill2FX()
        {
            SetupAttackCollision(_attackFXs[1], AttackTypeIndex.Skill2);
        }

        private void SetupAttackBound(int index, AnimationEvent evt, AttackTypeIndex atkIdx)
        {
            if (evt.intParameter != 0)
                SetupAttackCollision(_attackBounds[index], atkIdx);
            else _attackBounds[index].SetActive(evt.intParameter != 0);
        }

        protected virtual void SetActiveLightAttack1Bound(AnimationEvent evt)
        {
            SetupAttackBound(0, evt, AttackTypeIndex.Light1);
        }

        protected virtual void SetActiveLightAttack2Bound(AnimationEvent evt)
        {
            SetupAttackBound(1, evt, AttackTypeIndex.Light2);
        }

        protected virtual void SetActiveLightAttack3Bound(AnimationEvent evt)
        {
            SetupAttackBound(2, evt, AttackTypeIndex.Light3);
        }

        protected virtual void SetActiveHeavyAttack3Bound(AnimationEvent evt)
        {
            if (evt.intParameter != 0)
                SetupAttackCollision(_attackBounds.Last(), AttackTypeIndex.Heavy);
            else _attackBounds.Last().SetActive(evt.intParameter != 0);
        }
    }
}