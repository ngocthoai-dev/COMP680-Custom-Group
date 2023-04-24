using Core.Extension;
using Core.SO;
using Core.Utility;
using Cysharp.Threading.Tasks;
using Shared.Extension;
using System;
using System.Collections;
using UnityEngine;

namespace Core.Gameplay
{
	[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
	public partial class CharacterRenderer : MonoBehaviour
	{
		public class ParamKey
		{
			public static readonly int DEAD_TRIGGER = Animator.StringToHash("Dead");
			public static readonly int ISRUN_BOOL = Animator.StringToHash("IsRun");
			public static readonly int RUNSPD_FLOAT = Animator.StringToHash("RunSpd");
			public static readonly int BEHIT_TRIGGER = Animator.StringToHash("BeHit");
			public static readonly int SPRINT_BOOl = Animator.StringToHash("IsSprint");
			public static readonly int ATKSPD_FLOAT = Animator.StringToHash("AtkSpd");
			public static readonly int ATKINDEX_INT = Animator.StringToHash("AtkIndex");
			public static readonly int SKILLINDEX_INT = Animator.StringToHash("SkillIndex");
		}

		public class AnimKey
		{
			public const string LightAtk1 = "A_LightAtk1";
			public const string LightAtk2 = "A_LightAtk2";
			public const string LightAtk3 = "A_LightAtk3";
			public const string HeavyAtk = "A_HeavyAtk";
			public static readonly string[] ATTACK = { "", LightAtk1, LightAtk2, LightAtk3, HeavyAtk };
			public const string Skill1 = "A_Skill1";
			public const string Skill2 = "A_Skill2";
			public static readonly string[] SKILL = { "", Skill1, Skill2 };
			public const string BeHit = "A_BeHit";
		}

		[SerializeField][DebugOnly] private SpriteRenderer _spriteRenderer;
		[SerializeField][DebugOnly] private Animator _animator;
		[SerializeField][DebugOnly] protected CharacterController2D _controller;
		[SerializeField][DebugOnly] private bool _isFacingRight;
		[SerializeField][DebugOnly] private Transform _shield;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_controller = GetComponentInParent<CharacterController2D>();

			foreach (AttackTypeIndex atkIdx in Enum.GetValues(typeof(AttackTypeIndex)))
			{
				_attackBounds[atkIdx] = transform.Find("AttackBound").Find(atkIdx.ToString());
				_attackFXs[atkIdx] = transform.Find("AttackFX").Find(atkIdx.ToString());
			}
			_shield = transform.Find("FX/Shield");
		}

		public void SetColor(Color color)
		{
			_spriteRenderer.color = color;
		}

		public void Flip(bool left, bool right)
		{
			if (left) _isFacingRight = false;
			else if (right) _isFacingRight = true;
			transform.eulerAngles = new Vector3(0f, !_isFacingRight ? 180f : 0f, 0f);
		}

		public void Run(bool isRun, float runSpeed = 1f)
		{
			_animator.SetFloat(ParamKey.RUNSPD_FLOAT, runSpeed);
			_animator.SetBool(ParamKey.ISRUN_BOOL, isRun);
		}

		public int GetAnimAttackIndex()
		{
			return _animator.GetInteger(ParamKey.ATKINDEX_INT);
		}

		public int GetAnimSkillIndex()
		{
			return _animator.GetInteger(ParamKey.SKILLINDEX_INT);
		}

		public async UniTask Attack(int atkIdx = 1, float atkSpeed = 1f)
		{
			_animator.SetFloat(ParamKey.ATKSPD_FLOAT, 1 / atkSpeed);
			_animator.SetInteger(ParamKey.ATKINDEX_INT, atkIdx);
			string anim = AnimKey.ATTACK[atkIdx];
			if (!anim.IsNullOrEmpty()) await WaitForAnimation(anim);
		}

		public async UniTask UseSkill(int skillIdx = 1, float atkSpeed = 1f)
		{
			_animator.SetFloat(ParamKey.ATKSPD_FLOAT, 1 / atkSpeed);
			_animator.SetInteger(ParamKey.SKILLINDEX_INT, skillIdx);
			string anim = AnimKey.SKILL[skillIdx];
			if (!anim.IsNullOrEmpty()) await WaitForAnimation(anim);
		}

		public async UniTask BeHit()
		{
			TriggerAnimation(ParamKey.BEHIT_TRIGGER);
			await WaitForAnimation(AnimKey.BeHit);
		}

		public void Sprint(bool isSprint)
		{
			_animator.SetBool(ParamKey.SPRINT_BOOl, isSprint);
		}

		public void Parry(bool isParry)
		{
			if (isParry) _animator.SetBool(ParamKey.ISRUN_BOOL, false);
			_shield.SetActive(isParry);
		}

		public void Dead()
		{
			TriggerAnimation(ParamKey.DEAD_TRIGGER);
		}

		public void TriggerAnimation(int animationHash)
		{
			_animator.SetTrigger(animationHash);
		}

		public IEnumerator WaitForAnimation(string animName)
		{
			var anim = _animator.FindAnimation(animName);
			yield return new WaitForSeconds(anim.length);
		}
	}
}