#region Copyright & License Information
/*
 * Copyright 2007-2022 The d2 mod Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Linq;
using OpenRA.Activities;
using OpenRA.Mods.Common;
using OpenRA.Mods.Common.Activities;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.D2.Traits
{
	[Desc("Deliver the unit in production via frigate.")]
	public class D2ProductionAirdropInfo : ProductionInfo
	{
		[NotificationReference("Speech")]
		public readonly string ReadyAudio = "Reinforce";

		[FieldLoader.Require]
		[ActorReference(typeof(AircraftInfo))]
		[Desc("Cargo aircraft used for delivery. Must have the `Aircraft` trait.")]
		public readonly string ActorType = "frigate";

		[Desc("The cargo aircraft will spawn at the player baseline (map edge closest to the player spawn)")]
		public readonly bool BaselineSpawn = false;

		[Desc("Direction the aircraft should face to land.")]
		public readonly WAngle Facing = new WAngle(256);

		public override object Create(ActorInitializer init) { return new D2ProductionAirdrop(init, this); }
	}

	class D2ProductionAirdrop : Production
	{
		public D2ProductionAirdrop(ActorInitializer init, D2ProductionAirdropInfo info)
			: base(init, info) { }

		public override bool Produce(Actor self, ActorInfo producee, string productionType, TypeDictionary inits, int refundableValue)
		{
			if (IsTraitDisabled || IsTraitPaused)
				return false;

			var info = (D2ProductionAirdropInfo)Info;
			var owner = self.Owner;
			var map = owner.World.Map;
			var aircraftInfo = self.World.Map.Rules.Actors[info.ActorType].TraitInfo<AircraftInfo>();

			CPos startPos;
			CPos endPos;
			WAngle spawnFacing;

			if (info.BaselineSpawn)
			{
				var bounds = map.Bounds;
				var center = new MPos(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2).ToCPos(map);
				var spawnVec = owner.HomeLocation - center;
				startPos = owner.HomeLocation + spawnVec * (Exts.ISqrt((bounds.Height * bounds.Height + bounds.Width * bounds.Width) / (4 * spawnVec.LengthSquared)));
				endPos = startPos;
				var spawnDirection = new WVec((self.Location - startPos).X, (self.Location - startPos).Y, 0);
				spawnFacing = spawnDirection.Yaw;
			}
			else
			{
				// Start a fixed distance away: the width of the map.
				// This makes the production timing independent of spawnpoint
				var loc = self.Location.ToMPos(map);
				startPos = new MPos(loc.U + map.Bounds.Width, loc.V).ToCPos(map);
				endPos = new MPos(map.Bounds.Left, loc.V).ToCPos(map);
				spawnFacing = info.Facing;
			}

			// TODO: Do not assume a single exit point for simplicity
			var exitInfo = self.Info.TraitInfos<ExitInfo>();

			foreach (var tower in self.TraitsImplementing<INotifyDelivery>())
				tower.IncomingDelivery(self);

			owner.World.AddFrameEndTask(w =>
			{
				if (!self.IsInWorld || self.IsDead)
				{
					owner.PlayerActor.Trait<PlayerResources>().GiveCash(refundableValue);
					return;
				}

				var actor = w.CreateActor(info.ActorType, new TypeDictionary
				{
					new CenterPositionInit(w.Map.CenterOfCell(startPos) + new WVec(WDist.Zero, WDist.Zero, aircraftInfo.CruiseAltitude)),
					new OwnerInit(owner),
					new FacingInit(spawnFacing)
				});

				var exitCellsCount = exitInfo.Count();
				var exitCells = new CPos[exitCellsCount];
				var i = 0;
				foreach (var exitInfoItem in exitInfo)
				{
					exitCells[i++] = self.Location + exitInfoItem.ExitCell;
				}

				actor.QueueActivity(new Land(actor, Target.FromActor(self), WDist.Zero, WVec.Zero, info.Facing, clearCells: exitCells));
				actor.QueueActivity(new CallFunc(() =>
				{
					if (!self.IsInWorld || self.IsDead)
					{
						owner.PlayerActor.Trait<PlayerResources>().GiveCash(refundableValue);
						return;
					}

					foreach (var cargo in self.TraitsImplementing<INotifyDelivery>())
						cargo.Delivered(self);

					self.World.AddFrameEndTask(ww => DoProduction(self, producee, exitInfo.First(), productionType, inits));
					Game.Sound.PlayNotification(self.World.Map.Rules, self.Owner, "Speech", info.ReadyAudio, self.Owner.Faction.InternalName);
				}));

				actor.QueueActivity(new FlyOffMap(actor, Target.FromCell(w, endPos)));
				actor.QueueActivity(new RemoveSelf());
			});

			return true;
		}
	}
}
