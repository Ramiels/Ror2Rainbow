using RoR2;
using RoR2.Items;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace Rainbow.ItemBodyBehaviors
{
	internal class AttackSpeedScarfBehavior : BaseItemBodyBehavior
	{
		[ItemDefAssociation(useOnServer = true, useOnClient = false)]
		// [BaseItemBodyBehavior.ItemDefAssociationAttribute(useOnServer = true, useOnClient = false)]

		public static ItemDef GetItemDef() => Rainbow.Items.AttackSpeedScarf.ItemDef;
		private bool providingBuff;
		private bool wasOutOfCombat = false;
		private float buffTimer;
		private readonly float buffInterval = 10/ (6 + (4 * this.stack));

		// Handle in/out of combat behavior
		private void OnDisable()
		{
			this.SetProvidingBuff(false);
		}
		private void FixedUpdate()
		{
			this.SetProvidingBuff(base.body.outOfCombat);
		}

		private void SetProvidingBuff(bool shouldProvideBuff)
		{
			// Exited Combat
			if ((wasOutOfCombat != shouldProvideBuff) && shouldProvideBuff)
			{
				Log.Debug("Exited Combat");
				this.buffTimer = 0f;
			}
			wasOutOfCombat = shouldProvideBuff;

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
			if (((skillLocator != null) ? skillLocator.primary : null) == skill && this.body.GetBuffCount(RoR2Content.Buffs.WhipBoost) > 0)
			{
				base.body.RemoveBuff(RoR2Content.Buffs.WhipBoost);
			}
		}
	}
}
