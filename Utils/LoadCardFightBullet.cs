using Entrogic.NPCs.CardFightable.CardBullet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entrogic
{
	[Obsolete]
    public static class LoadCardFightBullet
	{
        public static void LoadAllBullets()
		{
			try
			{
				Entrogic.cfBullets.Clear();

				Entrogic.cfBullets.Add(typeof(MushroomBullet).Name, new MushroomBullet());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
