using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Entrogic
{
	public class PiggyBankAmmo
	{
		public PiggyBankAmmo()
		{
            On.Terraria.Player.PickAmmo += Player_PickAmmo;
			On.Terraria.Player.HasAmmo += NewHasAmmo;
			On.Terraria.UI.ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += NewDraw;
			On.Terraria.Player.CountItem += NewCountItem;
			On.Terraria.Player.ConsumeItem += NewConsumeItem;
		}

        private void Player_PickAmmo(On.Terraria.Player.orig_PickAmmo orig, Player player, Item sItem, ref int shoot, ref float speed, ref bool canShoot, ref int Damage, ref float KnockBack, bool dontConsume)
		{
			Item item = new Item();
			bool flag = false;
			for (int i = 54; i < 58; i++)
			{
				if (player.inventory[i].ammo == sItem.useAmmo && player.inventory[i].stack > 0)
				{
					item = player.inventory[i];
					canShoot = true;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Item[] bank3 = player.bank3.item;
				for (int a = 0; a < bank3.Length; a++)
				{
					if (bank3[a].ammo == sItem.useAmmo && bank3[a].stack > 0)
					{
						item = bank3[a];
						canShoot = true;
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				Item[] bank4 = player.bank2.item;
				for (int b = 0; b < bank4.Length; b++)
				{
					if (bank4[b].ammo == sItem.useAmmo && bank4[b].stack > 0)
					{
						item = bank4[b];
						canShoot = true;
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				Item[] bank5 = player.bank.item;
				for (int c = 0; c < bank5.Length; c++)
				{
					if (bank5[c].ammo == sItem.useAmmo && bank5[c].stack > 0)
					{
						item = bank5[c];
						canShoot = true;
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int j = 0; j < 54; j++)
				{
					if (player.inventory[j].ammo == sItem.useAmmo && player.inventory[j].stack > 0)
					{
						item = player.inventory[j];
						canShoot = true;
						break;
					}
				}
			}
			if (!canShoot)
			{
				return;
			}
			if (sItem.type == 1946)
			{
				shoot = 338 + item.type - 771;
				if (shoot > 341)
				{
					shoot = 341;
				}
			}
			else if (sItem.useAmmo == AmmoID.Rocket)
			{
				shoot += item.shoot;
			}
			else if (sItem.useAmmo == 780)
			{
				shoot += item.shoot;
			}
			else if (item.shoot > 0)
			{
				shoot = item.shoot;
			}
			if (sItem.type == 3019 && shoot == 1)
			{
				shoot = 485;
			}
			if (sItem.type == 3052)
			{
				shoot = 495;
			}
			if (sItem.type == 3245 && shoot == 21)
			{
				shoot = 532;
			}
			if (shoot == 42)
			{
				if (item.type == 370)
				{
					shoot = 65;
					Damage += 5;
				}
				else if (item.type == 408)
				{
					shoot = 68;
					Damage += 5;
				}
				else if (item.type == 1246)
				{
					shoot = 354;
					Damage += 5;
				}
			}
			if (player.inventory[player.selectedItem].type == 2888 && shoot == 1)
			{
				shoot = 469;
			}
			if (player.magicQuiver && (sItem.useAmmo == AmmoID.Arrow || sItem.useAmmo == AmmoID.Stake))
			{
				KnockBack = (float)((int)((double)KnockBack * 1.1));
				speed *= 1.1f;
			}
			speed += item.shootSpeed;
			if (item.ranged)
			{
				if (item.damage > 0)
				{
					if (sItem.damage > 0)
					{
						Damage += (int)((float)(item.damage * Damage) / (float)sItem.damage);
					}
					else
					{
						Damage += item.damage;
					}
				}
			}
			else
			{
				Damage += item.damage;
			}
			if (sItem.useAmmo == AmmoID.Arrow && player.archery && speed < 20f)
			{
				speed *= 1.2f;
				if (speed > 20f)
				{
					speed = 20f;
				}
			}
			KnockBack += item.knockBack;
			ItemLoader.PickAmmo(sItem, item, player, ref shoot, ref speed, ref Damage, ref KnockBack);
			bool flag2 = dontConsume;
			if (sItem.type == 3245)
			{
				if (Main.rand.Next(3) == 0)
				{
					flag2 = true;
				}
				else if (player.thrownCost33 && Main.rand.Next(100) < 33)
				{
					flag2 = true;
				}
				else if (player.thrownCost50 && Main.rand.Next(100) < 50)
				{
					flag2 = true;
				}
			}
			if (sItem.type == 3475 && Main.rand.Next(3) != 0)
			{
				flag2 = true;
			}
			if (sItem.type == 3540 && Main.rand.Next(3) != 0)
			{
				flag2 = true;
			}
			if (player.magicQuiver && sItem.useAmmo == AmmoID.Arrow && Main.rand.Next(5) == 0)
			{
				flag2 = true;
			}
			if (player.ammoBox && Main.rand.Next(5) == 0)
			{
				flag2 = true;
			}
			if (player.ammoPotion && Main.rand.Next(5) == 0)
			{
				flag2 = true;
			}
			if (sItem.type == 1782 && Main.rand.Next(3) == 0)
			{
				flag2 = true;
			}
			if (sItem.type == 98 && Main.rand.Next(3) == 0)
			{
				flag2 = true;
			}
			if (sItem.type == 2270 && Main.rand.Next(2) == 0)
			{
				flag2 = true;
			}
			if (sItem.type == 533 && Main.rand.Next(2) == 0)
			{
				flag2 = true;
			}
			if (sItem.type == 1929 && Main.rand.Next(2) == 0)
			{
				flag2 = true;
			}
			if (sItem.type == 1553 && Main.rand.Next(2) == 0)
			{
				flag2 = true;
			}
			if (sItem.type == 434 && player.itemAnimation < PlayerHooks.TotalMeleeTime((float)sItem.useAnimation, player, sItem) - 2)
			{
				flag2 = true;
			}
			if (player.ammoCost80 && Main.rand.Next(5) == 0)
			{
				flag2 = true;
			}
			if (player.ammoCost75 && Main.rand.Next(4) == 0)
			{
				flag2 = true;
			}
			if (shoot == 85 && player.itemAnimation < player.itemAnimationMax - 6)
			{
				flag2 = true;
			}
			if ((shoot == 145 || shoot == 146 || shoot == 147 || shoot == 148 || shoot == 149) && player.itemAnimation < player.itemAnimationMax - 5)
			{
				flag2 = true;
			}
			if (!(flag2 | (!PlayerHooks.ConsumeAmmo(player, sItem, item) | !ItemLoader.ConsumeAmmo(sItem, item, player))) && item.consumable)
			{
				PlayerHooks.OnConsumeAmmo(player, sItem, item);
				ItemLoader.OnConsumeAmmo(sItem, item, player);
				item.stack--;
				if (item.stack <= 0)
				{
					item.active = false;
					item.TurnToAir();
				}
			}
		}

        public bool NewConsumeItem(On.Terraria.Player.orig_ConsumeItem orig, Player player, int type, bool reverseOrder)
		{
			Item[] bank = player.bank.item;
			Item[] bank2 = player.bank2.item;
			Item[] bank3 = player.bank3.item;
			int num = 0;
			int num2 = 58;
			int num3 = 1;
			if (reverseOrder)
			{
				num = 57;
				num2 = -1;
				num3 = -1;
			}
			for (int a = reverseOrder ? (bank3.Length - 1) : 0; a != -1; a += (reverseOrder ? -1 : 1))
			{
				if (bank3[a].stack > 0 && bank3[a].type == type)
				{
					num += bank3[a].stack;
				}
				if (bank3[a].stack > 0 && bank3[a].type == type)
				{
					if (ItemLoader.ConsumeItem(bank3[a], player))
					{
						bank3[a].stack--;
					}
					if (bank3[a].stack <= 0)
					{
						bank3[a].SetDefaults(0, false);
					}
					return true;
				}
			}
			for (int b = reverseOrder ? (bank2.Length - 1) : 0; b != -1; b += (reverseOrder ? -1 : 1))
			{
				if (bank2[b].stack > 0 && bank2[b].type == type)
				{
					num += bank2[b].stack;
				}
				if (bank2[b].stack > 0 && bank2[b].type == type)
				{
					if (ItemLoader.ConsumeItem(bank2[b], player))
					{
						bank2[b].stack--;
					}
					if (bank2[b].stack <= 0)
					{
						bank2[b].SetDefaults(0, false);
					}
					return true;
				}
			}
			for (int c = reverseOrder ? (bank.Length - 1) : 0; c != -1; c += (reverseOrder ? -1 : 1))
			{
				if (bank[c].stack > 0 && bank[c].type == type)
				{
					num += bank[c].stack;
				}
				if (bank[c].stack > 0 && bank[c].type == type)
				{
					if (ItemLoader.ConsumeItem(bank[c], player))
					{
						bank[c].stack--;
					}
					if (bank[c].stack <= 0)
					{
						bank[c].SetDefaults(0, false);
					}
					return true;
				}
			}
			for (int num4 = num; num4 != num2; num4 += num3)
			{
				if (player.inventory[num4].stack > 0 && player.inventory[num4].type == type)
				{
					if (ItemLoader.ConsumeItem(player.inventory[num4], player))
					{
						player.inventory[num4].stack--;
					}
					if (player.inventory[num4].stack <= 0)
					{
						player.inventory[num4].SetDefaults(0, false);
					}
					return true;
				}
			}
			return false;
		}

		private int NewCountItem(On.Terraria.Player.orig_CountItem orig, Player player, int type, int stopCountingAt)
		{
			int num = 0;
			Item[] bank = player.bank.item;
			Item[] bank2 = player.bank2.item;
			Item[] bank3 = player.bank3.item;
			for (int a = 0; a < bank3.Length; a++)
			{
				if (bank3[a].stack > 0 && bank3[a].type == type)
				{
					num += bank3[a].stack;
				}
			}
			for (int b = 0; b < bank2.Length; b++)
			{
				if (bank2[b].stack > 0 && bank2[b].type == type)
				{
					num += bank2[b].stack;
				}
			}
			for (int c = 0; c < bank.Length; c++)
			{
				if (bank[c].stack > 0 && bank[c].type == type)
				{
					num += bank[c].stack;
				}
			}
			for (int num2 = 0; num2 != 58; num2++)
			{
				if (player.inventory[num2].stack > 0 && player.inventory[num2].type == type)
				{
					num += player.inventory[num2].stack;
				}
			}
			return num;
		}

		private void NewDraw(On.Terraria.UI.ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
		{
			Player player = Main.player[Main.myPlayer];
			Item item = inv[slot];
			float inventoryScale = Main.inventoryScale;
			Color color = Color.White;
			if (lightColor != Color.Transparent)
			{
				color = lightColor;
			}
			int num = -1;
			bool flag = false;
			int num2 = 0;
			if (PlayerInput.UsingGamepadUI)
			{
				switch (context)
				{
					case 0:
					case 1:
					case 2:
						num = slot;
						break;
					case 3:
					case 4:
						num = 400 + slot;
						break;
					case 5:
						num = 303;
						break;
					case 6:
						num = 300;
						break;
					case 7:
						num = 1500;
						break;
					case 8:
					case 9:
					case 10:
					case 11:
						num = 100 + slot;
						break;
					case 12:
						if (inv == player.dye)
						{
							num = 120 + slot;
						}
						if (inv == player.miscDyes)
						{
							num = 185 + slot;
						}
						break;
					case 15:
						num = 2700 + slot;
						break;
					case 16:
						num = 184;
						break;
					case 17:
						num = 183;
						break;
					case 18:
						num = 182;
						break;
					case 19:
						num = 180;
						break;
					case 20:
						num = 181;
						break;
					case 22:
						if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig != -1)
						{
							num = 700 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig;
						}
						if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall != -1)
						{
							num = 1500 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall + 1;
						}
						break;
				}
				flag = (UILinkPointNavigator.CurrentPoint == num);
				if (context == 0)
				{
					num2 = player.DpadRadial.GetDrawMode(slot);
					if (num2 > 0 && !PlayerInput.CurrentProfile.UsingDpadHotbar())
					{
						num2 = 0;
					}
				}
			}
			Texture2D texture2D = Main.inventoryBackTexture;
			Color color2 = Main.inventoryBack;
			bool flag2 = false;
			if (item.type > 0 && item.stack > 0 && item.favorited && context != 13 && context != 21 && context != 22 && context != 14)
			{
				texture2D = Main.inventoryBack10Texture;
			}
			else if (item.type > 0 && item.stack > 0 && ItemSlot.Options.HighlightNewItems && item.newAndShiny && context != 13 && context != 21 && context != 14 && context != 22)
			{
				texture2D = Main.inventoryBack15Texture;
				float num3 = (float)Main.mouseTextColor / 255f;
				num3 = num3 * 0.2f + 0.8f;
				color2 = Utils.MultiplyRGBA(color2, new Color(num3, num3, num3));
			}
			else if (PlayerInput.UsingGamepadUI && item.type > 0 && item.stack > 0 && num2 != 0 && context != 13 && context != 21 && context != 22)
			{
				texture2D = Main.inventoryBack15Texture;
				float num4 = (float)Main.mouseTextColor / 255f;
				num4 = num4 * 0.2f + 0.8f;
				color2 = ((num2 != 1) ? Utils.MultiplyRGBA(color2, new Color(num4 / 2f, num4, num4 / 2f)) : Utils.MultiplyRGBA(color2, new Color(num4, num4 / 2f, num4 / 2f)));
			}
			else if (context == 0 && slot < 10)
			{
				texture2D = Main.inventoryBack9Texture;
			}
			else
			{
				switch (context)
				{
					case 3:
						texture2D = Main.inventoryBack5Texture;
						break;
					case 4:
						texture2D = Main.inventoryBack2Texture;
						break;
					case 5:
					case 7:
						texture2D = Main.inventoryBack4Texture;
						break;
					case 6:
						texture2D = Main.inventoryBack7Texture;
						break;
					case 8:
					case 10:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
						texture2D = Main.inventoryBack3Texture;
						break;
					case 9:
					case 11:
						texture2D = Main.inventoryBack8Texture;
						break;
					case 12:
						texture2D = Main.inventoryBack12Texture;
						break;
					case 13:
						{
							byte b = 200;
							if (slot == Main.player[Main.myPlayer].selectedItem)
							{
								texture2D = Main.inventoryBack14Texture;
								b = byte.MaxValue;
							}
							color2 = new Color((int)b, (int)b, (int)b, (int)b);
							break;
						}
					case 14:
					case 21:
						flag2 = true;
						break;
					case 15:
						texture2D = Main.inventoryBack6Texture;
						break;
					case 22:
						texture2D = Main.inventoryBack4Texture;
						break;
				}
			}
			if (context == 0 && inventoryGlowTime[slot] > 0 && !inv[slot].favorited)
			{
				float scale = Main.invAlpha / 255f;
				Color color4 = new Color(63, 65, 151, 255) * scale;
				Color value2 = Main.hslToRgb(inventoryGlowHue[slot], 1f, 0.5f) * scale;
				float num5 = (float)inventoryGlowTime[slot] / 300f;
				num5 *= num5;
				color2 = Color.Lerp(color4, value2, num5 / 2f);
				texture2D = Main.inventoryBack13Texture;
			}
			if ((context == 4 || context == 3) && inventoryGlowTimeChest[slot] > 0 && !inv[slot].favorited)
			{
				float scale2 = Main.invAlpha / 255f;
				Color value3 = new Color(130, 62, 102, 255) * scale2;
				if (context == 3)
				{
					value3 = new Color(104, 52, 52, 255) * scale2;
				}
				Color value4 = Main.hslToRgb(inventoryGlowHueChest[slot], 1f, 0.5f) * scale2;
				float num6 = (float)inventoryGlowTimeChest[slot] / 300f;
				num6 *= num6;
				color2 = Color.Lerp(value3, value4, num6 / 2f);
				texture2D = Main.inventoryBack13Texture;
			}
			if (flag)
			{
				texture2D = Main.inventoryBack14Texture;
				color2 = Color.White;
			}
			if (!flag2)
			{
				spriteBatch.Draw(texture2D, position, null, color2, 0f, default(Vector2), inventoryScale, 0, 0f);
			}
			int num7 = -1;
			switch (context)
			{
				case 8:
					if (slot == 0)
					{
						num7 = 0;
					}
					if (slot == 1)
					{
						num7 = 6;
					}
					if (slot == 2)
					{
						num7 = 12;
					}
					break;
				case 9:
					if (slot == 10)
					{
						num7 = 3;
					}
					if (slot == 11)
					{
						num7 = 9;
					}
					if (slot == 12)
					{
						num7 = 15;
					}
					break;
				case 10:
					num7 = 11;
					break;
				case 11:
					num7 = 2;
					break;
				case 12:
					num7 = 1;
					break;
				case 16:
					num7 = 4;
					break;
				case 17:
					num7 = 13;
					break;
				case 18:
					num7 = 7;
					break;
				case 19:
					num7 = 10;
					break;
				case 20:
					num7 = 17;
					break;
			}
			if ((item.type <= 0 || item.stack <= 0) && num7 != -1)
			{
				Texture2D texture2D2 = Main.extraTexture[54];
				Rectangle rectangle = Utils.Frame(texture2D2, 3, 6, num7 % 3, num7 / 3);
				rectangle.Width -= 2;
				rectangle.Height -= 2;
				spriteBatch.Draw(texture2D2, position + Utils.Size(texture2D) / 2f * inventoryScale, new Rectangle?(rectangle), Color.White * 0.35f, 0f, Utils.Size(rectangle) / 2f, inventoryScale, 0, 0f);
			}
			Vector2 vector = Utils.Size(texture2D) * inventoryScale;
			if (item.type > 0 && item.stack > 0)
			{
				Texture2D texture2D3 = Main.itemTexture[item.type];
				Rectangle rectangle2 = (Main.itemAnimations[item.type] == null) ? Utils.Frame(texture2D3, 1, 1, 0, 0) : Main.itemAnimations[item.type].GetFrame(texture2D3);
				Color newColor = color;
				float num8 = 1f;
				ItemSlot.GetItemLight(ref newColor, ref num8, item, false);
				float num9 = 1f;
				if (rectangle2.Width > 32 || rectangle2.Height > 32)
				{
					num9 = ((rectangle2.Width <= rectangle2.Height) ? (32f / (float)rectangle2.Height) : (32f / (float)rectangle2.Width));
				}
				num9 *= inventoryScale;
				Vector2 position2 = position + vector / 2f - Utils.Size(rectangle2) * num9 / 2f;
				Vector2 origin = Utils.Size(rectangle2) * (num8 / 2f - 0.5f);
				if (ItemLoader.PreDrawInInventory(item, spriteBatch, position2, rectangle2, item.GetAlpha(newColor), item.GetColor(color), origin, num9 * num8))
				{
					spriteBatch.Draw(texture2D3, position2, new Rectangle?(rectangle2), item.GetAlpha(newColor), 0f, origin, num9 * num8, 0, 0f);
					if (item.color != Color.Transparent)
					{
						spriteBatch.Draw(texture2D3, position2, new Rectangle?(rectangle2), item.GetColor(color), 0f, origin, num9 * num8, 0, 0f);
					}
				}
				ItemLoader.PostDrawInInventory(item, spriteBatch, position2, rectangle2, item.GetAlpha(newColor), item.GetColor(color), origin, num9 * num8);
				if (ItemID.Sets.TrapSigned[item.type])
				{
					spriteBatch.Draw(Main.wireTexture, position + new Vector2(40f, 40f) * inventoryScale, new Rectangle?(new Rectangle(4, 58, 8, 8)), color, 0f, new Vector2(4f), 1f, 0, 0f);
				}
				if (item.stack > 1)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, item.stack.ToString(), position + new Vector2(10f, 26f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
				}
				int num10 = -1;
				if (context == 13)
				{
					if (item.DD2Summon)
					{
						Item[] bank3 = player.bank3.item;
						for (int a = 0; a < bank3.Length; a++)
						{
							if (bank3[a].type == 3822)
							{
								num10 += bank3[a].stack;
							}
						}
						Item[] bank4 = player.bank2.item;
						for (int b2 = 0; b2 < bank4.Length; b2++)
						{
							if (bank4[b2].type == 3822)
							{
								num10 += bank4[b2].stack;
							}
						}
						Item[] bank5 = player.bank.item;
						for (int c = 0; c < bank5.Length; c++)
						{
							if (bank5[c].type == 3822)
							{
								num10 += bank5[c].stack;
							}
						}
						for (int i = 0; i < 58; i++)
						{
							if (inv[i].type == 3822)
							{
								num10 += inv[i].stack;
							}
						}
						if (num10 >= 0)
						{
							num10++;
						}
					}
					if (item.useAmmo > 0)
					{
						int useAmmo = item.useAmmo;
						num10 = 0;
						for (int j = 0; j < 58; j++)
						{
							if (inv[j].ammo == useAmmo)
							{
								num10 += inv[j].stack;
							}
						}
						Item[] bank6 = player.bank3.item;
						for (int a2 = 0; a2 < bank6.Length; a2++)
						{
							if (bank6[a2].ammo == useAmmo)
							{
								num10 += bank6[a2].stack;
							}
						}
						Item[] bank7 = player.bank2.item;
						for (int b3 = 0; b3 < bank7.Length; b3++)
						{
							if (bank7[b3].ammo == useAmmo)
							{
								num10 += bank7[b3].stack;
							}
						}
						Item[] bank8 = player.bank.item;
						for (int c2 = 0; c2 < bank8.Length; c2++)
						{
							if (bank8[c2].ammo == useAmmo)
							{
								num10 += bank8[c2].stack;
							}
						}
					}
					if (item.fishingPole > 0)
					{
						num10 = 0;
						for (int k = 0; k < 58; k++)
						{
							if (inv[k].bait > 0)
							{
								num10 += inv[k].stack;
							}
						}
					}
					if (item.tileWand > 0)
					{
						int tileWand = item.tileWand;
						num10 = 0;
						for (int l = 0; l < 58; l++)
						{
							if (inv[l].type == tileWand)
							{
								num10 += inv[l].stack;
							}
						}
					}
					if (item.type == 509 || item.type == 851 || item.type == 850 || item.type == 3612 || item.type == 3625 || item.type == 3611)
					{
						num10 = 0;
						for (int m = 0; m < 58; m++)
						{
							if (inv[m].type == 530)
							{
								num10 += inv[m].stack;
							}
						}
					}
				}
				if (num10 != -1)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, num10.ToString(), position + new Vector2(8f, 30f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale * 0.8f), -1f, inventoryScale);
				}
				if (context == 13)
				{
					string text = string.Concat(slot + 1);
					if (text == "10")
					{
						text = "0";
					}
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text, position + new Vector2(8f, 4f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
				}
				if (context == 13 && item.potion)
				{
					Vector2 position3 = position + Utils.Size(texture2D) * inventoryScale / 2f - Utils.Size(Main.cdTexture) * inventoryScale / 2f;
					Color color3 = item.GetAlpha(color) * ((float)player.potionDelay / (float)player.potionDelayTime);
					spriteBatch.Draw(Main.cdTexture, position3, null, color3, 0f, default(Vector2), num9, 0, 0f);
				}
				if ((context == 10 || context == 18) && item.expertOnly && !Main.expertMode)
				{
					Vector2 position4 = position + Utils.Size(texture2D) * inventoryScale / 2f - Utils.Size(Main.cdTexture) * inventoryScale / 2f;
					Color white = Color.White;
					spriteBatch.Draw(Main.cdTexture, position4, null, white, 0f, default(Vector2), num9, 0, 0f);
				}
			}
			else if (context == 6)
			{
				Texture2D trashTexture = Main.trashTexture;
				Vector2 position5 = position + Utils.Size(texture2D) * inventoryScale / 2f - Utils.Size(trashTexture) * inventoryScale / 2f;
				spriteBatch.Draw(trashTexture, position5, null, new Color(100, 100, 100, 100), 0f, default(Vector2), inventoryScale, 0, 0f);
			}
			if (context == 0 && slot < 10)
			{
				float num11 = inventoryScale;
				string text2 = string.Concat(slot + 1);
				if (text2 == "10")
				{
					text2 = "0";
				}
				Color inventoryBack = Main.inventoryBack;
				int num12 = 0;
				if (Main.player[Main.myPlayer].selectedItem == slot)
				{
					num12 -= 3;
					inventoryBack.R = byte.MaxValue;
					inventoryBack.B = 0;
					inventoryBack.G = 210;
					inventoryBack.A = 100;
					num11 *= 1.4f;
				}
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text2, position + new Vector2(6f, (float)(4 + num12)) * inventoryScale, inventoryBack, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
			}
			if (num != -1)
			{
				UILinkPointNavigator.SetPosition(num, position + vector * 0.75f);
			}
		}

		private bool NewHasAmmo(On.Terraria.Player.orig_HasAmmo orig, Player player, Item sItem, bool canUse)
		{
			if (sItem.useAmmo > 0)
			{
				canUse = false;
				Item[] bank3 = player.bank3.item;
				for (int b = 0; b < bank3.Length; b++)
				{
					if (bank3[b].ammo == sItem.useAmmo && bank3[b].stack > 0)
					{
						canUse = true;
						break;
					}
				}
				if (!canUse)
				{
					Item[] bank4 = player.bank2.item;
					for (int a = 0; a < bank4.Length; a++)
					{
						if (bank4[a].ammo == sItem.useAmmo && bank4[a].stack > 0)
						{
							canUse = true;
							break;
						}
					}
				}
				if (!canUse)
				{
					Item[] bank5 = player.bank.item;
					for (int c = 0; c < bank5.Length; c++)
					{
						if (bank5[c].ammo == sItem.useAmmo && bank5[c].stack > 0)
						{
							canUse = true;
							break;
						}
					}
				}
				if (!canUse)
				{
					for (int i = 0; i < 58; i++)
					{
						if (player.inventory[i].ammo == sItem.useAmmo && player.inventory[i].stack > 0)
						{
							canUse = true;
							break;
						}
					}
				}
			}
			return canUse;
		}

		private static float[] inventoryGlowHue = new float[58];
		private static int[] inventoryGlowTime = new int[58];
		private static float[] inventoryGlowHueChest = new float[58];
		private static int[] inventoryGlowTimeChest = new int[58];
	}
}
