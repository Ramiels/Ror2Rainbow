using RoR2;
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
			Log.Info("Disabled!");
			if (this.body)
			{
				this.body.onSkillActivatedServer -= this.OnSkillActivated;
				while (this.body.HasBuff(Rainbow.Buffs.AttackSpeedScarfBuff.BuffDef)) { this.body.RemoveBuff(Rainbow.Buffs.AttackSpeedScarfBuff.BuffDef); }
			}
			this.skillLocator = null;
		}
		private void OnEnable()
		{
			Log.Info("Enabled!");
			if (this.body)
			{
				this.body.onSkillActivatedServer += this.OnSkillActivated;
				this.skillLocator = this.body.GetComponent<SkillLocator>();
			}
		}

		private void FixedUpdate()
		{
			Log.Debug("FixedUpdated!");
			// Only work outside combat
			if (!base.body.outOfCombat) { return; }

			int buffInterval = 10 / (6 + (4 * this.stack));
			// Reset timer outside combat
			if ((wasOutOfCombat != base.body.outOfCombat) && base.body.outOfCombat)
			{
				Log.Debug("Exited Combat");
				this.buffTimer = buffInterval;
			}
			wasOutOfCombat = base.body.outOfCombat;

			// Timer (reset when exiting combat)
			this.buffTimer += Time.fixedDeltaTime;
			if (this.buffTimer < buffInterval) { return; }
			this.buffTimer = 0;
			Log.Debug("Tick!");

			// Apply stacks of buff based on item stacks
			if (base.body.GetBuffCount(Rainbow.Buffs.AttackSpeedScarfBuff.BuffDef) < 6 + (4 * this.stack))
			{
				base.body.AddBuff(Rainbow.Buffs.AttackSpeedScarfBuff.BuffDef);
			}
		}

		// Consume stacks when attacking (todo replace buff)
		private void OnSkillActivated(GenericSkill skill)
		{
			Log.Info("SkillActivated!");
			SkillLocator skillLocator = this.skillLocator;
			if ((skillLocator?.primary) == skill && this.body.GetBuffCount(Rainbow.Buffs.AttackSpeedScarfBuff.BuffDef) > 0)
			{
				base.body.RemoveBuff(Rainbow.Buffs.AttackSpeedScarfBuff.BuffDef);
			}
		}
	}
}
