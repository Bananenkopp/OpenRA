#region Copyright & License Information
/*
 * Copyright 2007-2014 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System.Linq;
using Eluant;
using OpenRA;
using OpenRA.Mods.RA;
using OpenRA.Mods.RA.Activities;
using OpenRA.Scripting;
using OpenRA.Traits;

namespace OpenRA.Mods.RA.Scripting
{
	[ScriptPropertyGroup("Support Powers")]
	public class ChronsphereProperties : ScriptActorProperties, Requires<ChronoshiftPowerInfo>
	{
		public ChronsphereProperties(Actor self)
			: base(self) { }

		[Desc("Chronoshift a group of actors. A duration of 0 will teleport the actors permanently.")]
		public void Chronoshift(LuaTable unitLocationPairs, int duration = 0, bool killCargo = false)
		{
			foreach (var kv in unitLocationPairs)
			{
				Actor actor;
				CPos cell;
				if (!kv.Key.TryGetClrValue<Actor>(out actor) || !kv.Value.TryGetClrValue<CPos>(out cell))
					throw new LuaException("Chronoshift requires a table of Actor,CPos pairs. Received {0},{1}".F(kv.Key.WrappedClrType().Name, kv.Value.WrappedClrType().Name));

				var cs = actor.TraitOrDefault<Chronoshiftable>();
				if (cs != null && cs.CanChronoshiftTo(actor, cell))
					cs.Teleport(actor, cell, duration, killCargo, self);
			}
		}
	}
}