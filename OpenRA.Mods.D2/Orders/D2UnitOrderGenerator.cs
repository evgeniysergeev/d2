#region Copyright & License Information
/*
 * Copyright 2007-2020 The d2 mod Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
/* Based on on UnitOrderGenerator. Original Copyright: */
/*
 * Copyright 2007-2020 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Collections.Generic;
using OpenRA.Orders;

namespace OpenRA.Mods.D2.Orders
{
	public class D2UnitOrderGenerator : UnitOrderGenerator
	{
		public override IEnumerable<Order> Order(World world, CPos cell, int2 worldPixel, MouseInput mi)
		{
			return base.Order(world, cell, worldPixel, mi);
		}

		public override string GetCursor(World world, CPos cell, int2 worldPixel, MouseInput mi)
		{
			return base.GetCursor(world, cell, worldPixel, mi);
		}

		// Used for classic mouse orders, determines whether or not action at xy is move or select
		public override bool InputOverridesSelection(World world, int2 xy, MouseInput mi)
		{
			return base.InputOverridesSelection(world, xy, mi);
		}

		public override bool ClearSelectionOnLeftClick { get { return true; } }
	}
}
