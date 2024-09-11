using RoR2;
using RoR2.Items;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Rainbow.ItemBodyBehaviors
{
	internal class GambogeScarfBehavior : BaseItemBodyBehavior
	{
		[BaseItemBodyBehavior.ItemDefAssociationAttribute(useOnServer = true, useOnClient = false)]
		private static ItemDef GetItemDef()
		{
			return Rainbow.Items.GambogeScarf;
		}

		private bool providingBuff;

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
			if (shouldProvideBuff == this.providingBuff)
			{
				return;
			}
			this.providingBuff = shouldProvideBuff;
			if (this.providingBuff)
			{
				base.body.AddBuff(RoR2Content.Buffs.WhipBoost);
				EffectData effectData = new EffectData();
				effectData.origin = base.body.corePosition;
				CharacterDirection characterDirection = base.body.characterDirection;
				bool flag = false;
				if (characterDirection && characterDirection.moveVector != Vector3.zero)
				{
					effectData.rotation = Util.QuaternionSafeLookRotation(characterDirection.moveVector);
					flag = true;
				}
				if (!flag)
				{
					effectData.rotation = base.body.transform.rotation;
				}
				EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/SprintActivate"), effectData, true);
				return;
			}
			base.body.RemoveBuff(RoR2Content.Buffs.WhipBoost);
		}

	}
}
