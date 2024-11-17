using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;

namespace Rainbow.Buffs
{
	public abstract class BuffBase
	{
		public virtual bool IsDebuff { get; } = false;
		public virtual Color32 BuffColor { get; } = UnityEngine.Color.green;
		public virtual Sprite IconSprite { get; }
		public virtual bool IsCooldown { get; } = false;
		public virtual string Name { get; }
		public virtual bool CanStack { get; } = false;
		public virtual bool IgnoreGrowthNectar { get; } = false;
		public virtual bool IsHidden { get; } = false;

		public static BuffDef BuffDef;
		public static Dictionary<string, BuffDef> BuffDefs = [];
		// Init func
		public abstract void Init();

		// Create item
		protected void CreateBuff()
		{
			// Create the buffdef and populate it with data
			BuffDef = ScriptableObject.CreateInstance<BuffDef>();
			BuffDef.name = Name;
			BuffDef.isDebuff = IsDebuff;
			BuffDef.buffColor = BuffColor;
			BuffDef.iconSprite = IconSprite;
			BuffDef.isCooldown = IsCooldown;
			BuffDef.canStack = CanStack;
			BuffDef.eliteDef = null;
			BuffDef.ignoreGrowthNectar = IgnoreGrowthNectar;
			BuffDef.isHidden = IsHidden;
			// Add the buff
			R2API.ContentAddition.AddBuffDef(BuffDef);
			BuffDefs.Add(Name, BuffDef);
		}
		// Hooks
		public abstract void Hooks();
		// Buff Count funcs (idfk what I'm doing here)
		public int GetCount(CharacterBody body)
		{
			if (!body) { return 0; }
			return body.GetBuffCount(BuffDef);
		}
	}
}