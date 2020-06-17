#region Copyright & License Information
/*
 * Copyright 2007-2019 The d2 mod Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Primitives;
using OpenRA.Widgets;

namespace OpenRA.Mods.D2.Widgets
{
	public class D2PanelWidget : Widget
	{
		public int BarMargin = 3;

		public Func<Color> GetColor;

		public Action<MouseInput> OnMouseDown = _ => { };
		public Action<MouseInput> OnMouseUp = _ => { };

		public D2PanelWidget()
		{
			GetColor = () => Color.FromArgb(186, 190, 150);
		}

		protected D2PanelWidget(D2PanelWidget other)
			: base(other)
		{
			GetColor = other.GetColor;
		}

		public override Widget Clone()
		{
			return new D2PanelWidget(this);
		}

		public override void Draw()
		{
			var rb = RenderBounds;
			WidgetUtils.FillRectWithColor(rb, GetColor());
			D2WidgetUtils.DrawPanelBorder(rb, BarMargin);
		}
	}
}
