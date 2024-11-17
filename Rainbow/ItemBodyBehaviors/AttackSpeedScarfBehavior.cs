﻿using RoR2;
using RoR2.Items;
using UnityEngine;

[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace Rainbow.ItemBodyBehaviors
{
	internal class AttackSpeedScarfBehavior : BaseItemBodyBehavior
	{
		[ItemDefAssociation(useOnServer = true, useOnClient = false)]
		//[BaseItemBodyBehavior.ItemDefAssociationAttribute(useOnServer = true, useOnClient = false)]

		private static ItemDef GetItemDef() => Items.ItemBase.ItemDefs["AttackSpeedScarf"];
		private bool wasOutOfCombat = false;
		private float buffTimer;
		private SkillLocator skillLocator;

		// Handle setup/cleanup
		private void OnDisable()
		{
			if (this.body)
			{
				this.body.onSkillActivatedServer -= this.OnSkillActivated;
				while (this.body.HasBuff(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"])) { this.body.RemoveBuff(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"]); }
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
			// Only work outside combat
			if (!base.body.outOfCombat) { return; }

			int buffInterval = 10 / (3 + (2 * this.stack));
			// Reset timer outside combat
			if ((wasOutOfCombat != base.body.outOfCombat) && base.body.outOfCombat)
			{
				this.buffTimer = buffInterval;
			}
			wasOutOfCombat = base.body.outOfCombat;

			// Timer (reset when exiting combat)
			this.buffTimer += Time.fixedDeltaTime;
			if (this.buffTimer < buffInterval) { return; }
			this.buffTimer = 0;


			// Apply stacks of buff based on item stacks
			if (base.body.GetBuffCount(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"]) < 3 + (2 * this.stack))
			{
				base.body.AddBuff(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"]);
			}
		}

		// Consume stacks when attacking (todo replace buff)
		private void OnSkillActivated(GenericSkill skill)
		{
			SkillLocator skillLocator = this.skillLocator;
			if ((skillLocator?.primary) == skill && this.body.GetBuffCount(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"]) > 0)
			{
				base.body.RemoveBuff(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"]);
			}
		}
	}
}
