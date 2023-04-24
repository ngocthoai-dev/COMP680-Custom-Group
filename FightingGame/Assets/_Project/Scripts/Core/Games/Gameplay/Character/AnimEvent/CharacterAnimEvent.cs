using Core.Extension;
using Core.SO;
using Core.Utility;
using System.Linq;
using UnityEngine;

namespace Core.Gameplay
{
	public partial class CharacterRenderer
	{
		[SerializeField][DebugOnly] protected SerializableDictionary<AttackTypeIndex, Transform> _attackBounds = new();
		[SerializeField][DebugOnly] protected SerializableDictionary<AttackTypeIndex, Transform> _attackFXs = new();

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

		protected virtual void SpawnHeavyAtkFX()
		{
			SetupAttackCollision(_attackFXs[AttackTypeIndex.Heavy], AttackTypeIndex.Heavy);
		}

		protected virtual void SpawnSkill1FX()
		{
			SetupAttackCollision(_attackFXs[AttackTypeIndex.Skill1], AttackTypeIndex.Skill1);
		}

		protected virtual void SpawnSkill2FX()
		{
			SetupAttackCollision(_attackFXs[AttackTypeIndex.Skill2], AttackTypeIndex.Skill2);
		}

		private void SetupAttackBound(AttackTypeIndex atkIdx, AnimationEvent evt)
		{
			if (evt.intParameter != 0)
				SetupAttackCollision(_attackBounds[atkIdx], atkIdx);
			else _attackBounds[atkIdx].SetActive(evt.intParameter != 0);
		}

		protected virtual void SetActiveLightAttack1Bound(AnimationEvent evt)
		{
			SetupAttackBound(AttackTypeIndex.Light1, evt);
		}

		protected virtual void SetActiveLightAttack2Bound(AnimationEvent evt)
		{
			SetupAttackBound(AttackTypeIndex.Light2, evt);
		}

		protected virtual void SetActiveLightAttack3Bound(AnimationEvent evt)
		{
			SetupAttackBound(AttackTypeIndex.Light3, evt);
		}

		protected virtual void SetActiveHeavyAttackBound(AnimationEvent evt)
		{
			SetupAttackBound(AttackTypeIndex.Heavy, evt);
		}
	}
}