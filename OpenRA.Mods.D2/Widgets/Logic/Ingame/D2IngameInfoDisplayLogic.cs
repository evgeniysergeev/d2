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

using System.Drawing;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Mods.Common.Widgets.Logic;
using OpenRA.Primitives;
using OpenRA.Widgets;

namespace OpenRA.Mods.D2.Widgets.Logic
{
	public class D2IngameInfoDisplayLogic : ChromeLogic
	{
		readonly World world;
		readonly WorldRenderer worldRenderer;

		readonly LabelWidget label;
		readonly D2SpriteWidget preview;
		readonly D2ProgressBarWidget health;
		readonly LabelWidget dmgLabel;

		int selectionHash;
		Actor[] selectedActors = { };

		[ObjectCreator.UseCtor]
		public D2IngameInfoDisplayLogic(Widget widget, World world, WorldRenderer worldRenderer)
		{
			this.world = world;
			this.worldRenderer = worldRenderer;

			var panelColor = Color.FromArgb(186, 190, 150);
			var textColor = Color.FromArgb(71, 71, 55);

			var panel = widget.GetOrNull<ColorBlockWidget>("PANEL");
			if (panel != null)
				panel.GetColor = () => panelColor;

			label = widget.GetOrNull<LabelWidget>("NAME");
			if (label != null)
			{
				label.Align = TextAlign.Center;
				label.TextColor = textColor;
			}

			preview = widget.GetOrNull<D2SpriteWidget>("ICON");
			health = widget.GetOrNull<D2ProgressBarWidget>("HEALTH");
			if (health != null)
			{
				health.Visible = false;
			}

			dmgLabel = widget.GetOrNull<LabelWidget>("DMG");
			if (dmgLabel != null)
			{
				dmgLabel.Text = "DMG";
				dmgLabel.Align = TextAlign.Center;
				dmgLabel.TextColor = textColor;
				dmgLabel.Visible = false;
			}
		}
		public override void Tick()
		{
			base.Tick();

			UpdateStateIfNecessary();
		}

		void UpdateStateIfNecessary()
		{
			if (selectionHash == world.Selection.Hash)
				return;

			selectedActors = world.Selection.Actors
				.Where(a => a.IsInWorld && !a.IsDead)
				.ToArray();

			if (selectedActors.Length == 1)
			{
				var actor = selectedActors[0];
				if (label != null)
				{
					var tooltip = actor.TraitsImplementing<Tooltip>();
					if (tooltip.Any())
						label.Text = tooltip.FirstOrDefault(t => !t.IsTraitDisabled).Info.Name;
					else
					{
						label.Text = actor.Info.Name;
					}
				}

				var faction = world.LocalPlayer.Faction.Name;
				var rsi = actor.Info.TraitInfo<RenderSpritesInfo>();
				var icon = new Animation(world, rsi.GetImage(actor.Info, world.Map.Rules.Sequences, faction));
				var bi = actor.Info.TraitInfo<BuildableInfo>();
				icon.Play(bi.Icon);

				if (preview != null)
				{
					preview.Sprite = icon.Image;
					preview.Palette = worldRenderer.Palette(bi.IconPalette);
					preview.Scale = 2.0f;
					preview.Offset = float2.Zero;
				}

				if (health != null)
				{
					var healthTrait = actor.TraitsImplementing<Health>();
					if (healthTrait.Any())
					{
						health.GetPercentage = () =>
						{
							var h = healthTrait.FirstOrDefault();
							return (h != null) ? h.HP * 100 / h.MaxHP : 0;
						};
						health.Visible = true;
					}
					else
					{
						health.GetPercentage = () => 0;
						health.Visible = false;
					}
				}

				if (dmgLabel != null)
				{
					dmgLabel.Visible = true;
				}
			}
			else
			{
				if (preview != null)
					preview.Sprite = null;
				if (label != null)
					label.Text = "";
				if (health != null)
					health.Visible = false;
				if (dmgLabel != null)
				{
					dmgLabel.Visible = false;
				}
			}

			selectionHash = world.Selection.Hash;
		}
	}
}
