#region Copyright & License Information
/*
 * Copyright 2007-2018 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Widgets;

namespace OpenRA.Mods.D2.Widgets
{
	public static class D2WidgetUtils
	{
		public static void DrawLine(Point start, Point end, float width, Color color)
		{
			// Offset to the edges of the pixels
			var a = new float2(start.X - 0.5f, start.Y - 0.5f);
			var b = new float2(end.X - 0.5f, end.Y - 0.5f);
			Game.Renderer.RgbaColorRenderer.DrawLine(a, b, width, color);
		}

		public static void DrawDisconnectedLine(IEnumerable<Point> points, float width, Color color)
		{
			using (var e = points.GetEnumerator())
			{
				if (!e.MoveNext())
					return;

				var lastPoint = e.Current;
				while (e.MoveNext())
				{
					var point = e.Current;
					DrawLine(lastPoint, point, width, color);
					lastPoint = point;
				}
			}
		}

		public static void DrawColoredPanelBorder(Rectangle bounds, int size, Color highlightColor, Color shadowColor)
		{
			var rect1 = new Rectangle(bounds.Left, bounds.Top, size, bounds.Height - size);
			var rect2 = new Rectangle(bounds.Left, bounds.Top, bounds.Width - size, size);
			
			WidgetUtils.FillRectWithColor(rect1, highlightColor);
			WidgetUtils.FillRectWithColor(rect2, highlightColor);
			
			var rect3 = new Rectangle(bounds.Left + size, bounds.Bottom - size, bounds.Width - size, size);
			var rect4 = new Rectangle(bounds.Right - size, bounds.Top + size, size, bounds.Height - size);
			
			WidgetUtils.FillRectWithColor(rect3, shadowColor);
			WidgetUtils.FillRectWithColor(rect4, shadowColor);
		}

		public static void DrawPanelBorder(Rectangle bounds, int size)
		{
			var highlightColor = Color.FromArgb(251, 255, 203);
			var shadowColor = Color.FromArgb(103, 103, 79);

			DrawColoredPanelBorder(bounds, size, highlightColor, shadowColor);
		}

		public static void DrawColoredTriangle(Point a, int size, Color normalColor, Color highlightColor, Color shadowColor)
		{
			var rect1 = new Rectangle(a.X - size, a.Y, size, size * 2);
			var rect2 = new Rectangle(a.X - size * 2, a.Y + size, size, size);
			var rect3 = new Rectangle(a.X, a.Y + size, size, size);
			var rect4 = new Rectangle(a.X, a.Y, size, size);
			var rect5 = new Rectangle(a.X + size, a.Y + size, size, size * 2);
			var rect6 = new Rectangle(a.X - size * 2, a.Y + size * 2, size * 3, size);

			WidgetUtils.FillRectWithColor(rect1, highlightColor);
			WidgetUtils.FillRectWithColor(rect2, normalColor);
			WidgetUtils.FillRectWithColor(rect3, normalColor);
			WidgetUtils.FillRectWithColor(rect4, shadowColor);
			WidgetUtils.FillRectWithColor(rect5, shadowColor);
			WidgetUtils.FillRectWithColor(rect6, shadowColor);
		}

		public static void DrawRedTriangle(Point a, int size)
		{
			var normalColor = Color.FromArgb(124, 0, 0);
			var highlightColor = Color.FromArgb(180, 0, 0);
			var shadowColor = Color.Black;

			DrawColoredTriangle(a, size, normalColor, highlightColor, shadowColor);
		}

		public static void DrawYellowTriangle(Point a, int size)
		{
			var normalColor = Color.FromArgb(252, 184, 86);
			var highlightColor = Color.FromArgb(252, 252, 84);
			var shadowColor = Color.Black;

			DrawColoredTriangle(a, size, normalColor, highlightColor, shadowColor);
		}

		public static void DrawGreenTriangle(Point a, int size)
		{
			var normalColor = Color.FromArgb(0, 168, 0);
			var highlightColor = Color.FromArgb(84, 252, 84);
			var shadowColor = Color.Black;

			DrawColoredTriangle(a, size, normalColor, highlightColor, shadowColor);
		}
	}
}