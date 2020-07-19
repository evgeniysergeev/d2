#region Copyright & License Information
/*
 * Copyright 2007-2020 The d2 mod Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Mods.D2.Orders;
using OpenRA.Traits;

namespace OpenRA.Mods.D2.Traits
{
	public class D2DefaultOrderGeneratorInfo : TraitInfo
	{
		public override object Create(ActorInitializer init) { return new D2DefaultOrderGenerator(); }
	}

	public class D2DefaultOrderGenerator : IDefaultOrderGenerator
	{
		public D2DefaultOrderGenerator() { }

		IOrderGenerator IDefaultOrderGenerator.CreateDefaultOrderGenerator()
		{
			return new D2UnitOrderGenerator();
		}
	}
}
