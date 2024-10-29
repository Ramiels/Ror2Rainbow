using RoR2;
using RoR2.Items;
using UnityEngine;

[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace Rainbow.ItemBodyBehaviors
{
	internal class AttackSpeedScarfBehavior : BaseItemBodyBehavior
	{
		[ItemDefAssociation(useOnServer = true, useOnClient = false)]
		// [BaseItemBodyBehavior.ItemDefAssociationAttribute(useOnServer = true, useOnClient = false)]

		public static ItemDef GetItemDef() => Rainbow.Items.AttackSpeedScarf.ItemDef;
		private bool wasOutOfCombat = false;
		private float buffTimer;
		private readonly float buffInterval = 10/ (6 + (4 * this.stack));
		private SkillLocator skillLocator;

		// Handle setup/cleanup
		private void OnDisable()
		{
			if (this.body)
			{
				this.body.onSkillActivatedServer -= this.OnSkillActivated;
				while (this.body.HasBuff(RoR2Content.Buffs.WhipBoost)) { this.body.RemoveBuff(RoR2Content.Buffs.WhipBoost); }
			}
			this.skillLocator = null;
		}
		private void OnEnable()
		{
			if (this.body)
			{
				this.body.onSkillActivatedServer += this.OnSkillActivated;
				this.skillLocator = this.body.GetComponent<SkillLocator>();
			}
		}

		private void FixedUpdate()
		{
			// Exited Combat
			if ((wasOutOfCombat != base.body.outOfCombat) && base.body.outOfCombat)
			{
				Log.Debug("Exited Combat");
				this.buffTimer = 0f;
			}
			wasOutOfCombat = base.body.outOfCombat;

			// Timer (reset when exiting combat)
			this.buffTimer -= Time.fixedDeltaTime;
			if (this.buffTimer > 0f) { return; }
			this.buffTimer = this.buffInterval;

			// Apply stacks of buff based on item stacks
			if (base.body.GetBuffCount(RoR2Content.Buffs.WhipBoost) < 6 + (4 * this.stack))
			{
				base.body.AddBuff(RoR2Content.Buffs.WhipBoost);
			}
		}

		// Consume stacks when attacking (todo replace buff)
		private void OnSkillActivated(GenericSkill skill)
		{
			SkillLocator skillLocator = this.skillLocator;
			if ((skillLocator?.primary) == skill && this.body.GetBuffCount(RoR2Content.Buffs.WhipBoost) > 0)
			{
				base.body.RemoveBuff(RoR2Content.Buffs.WhipBoost);
			}
		}
	}
}
