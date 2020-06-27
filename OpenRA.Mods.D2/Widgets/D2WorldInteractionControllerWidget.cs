#region Copyright & License Information
/*
 * Copyright 2007-2020 The d2 mod Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
/* Based on on WorldInteractionControllerWidget.cs
 * Original Copyright: */
/*
 * Copyright 2007-2020 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Graphics;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Mods.D2.Orders;

namespace OpenRA.Mods.D2.Widgets
{
	public class D2WorldInteractionControllerWidget : WorldInteractionControllerWidget
	{
		[ObjectCreator.UseCtor]
		public D2WorldInteractionControllerWidget(World world, WorldRenderer worldRenderer)
			: base(world, worldRenderer)
		{
		}

		public override void Draw()
		{
			base.Draw();
		}

		public override bool HandleMouseInput(MouseInput mi)
		{
			if (World.OrderGenerator is D2ForceModifiersOrderGenerator)
			{
				// Any button can be used if button already pressed
				if (mi.Button == Game.Settings.Game.MouseButtonPreference.Cancel)
					mi.Button = Game.Settings.Game.MouseButtonPreference.Action;
			}

			return base.HandleMouseInput(mi);
		}

		public override string GetCursor(int2 screenPos)
		{
			return base.GetCursor(screenPos);
		}

		public override bool HandleKeyPress(KeyInput e)
		{
			return base.HandleKeyPress(e);
		}
	}
}
