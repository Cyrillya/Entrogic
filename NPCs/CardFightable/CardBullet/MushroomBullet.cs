<<<<<<< HEAD
﻿using Entrogic.NPCs.CardFightable.Particles;

using Microsoft.Xna.Framework;
=======
﻿using Microsoft.Xna.Framework;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic.NPCs.CardFightable.CardBullet
{
    public class MushroomBullet : CardFightBullet
    {
        public MushroomBullet()
        {
<<<<<<< HEAD
            _texture = (Texture2D)ModContent.GetTexture("Entrogic/NPCs/CardFightable/CardBullet/MushroomBullet");
=======
            _texture = ModContent.GetTexture("Entrogic/NPCs/CardFightable/CardBullet/MushroomBullet");
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            LifeSpan = 6f;
            IsFriendly = false;
            Damage = 100;
            Size = new Vector2(_texture.Width, _texture.Height);
        }
        public override void Update(GameTime gameTime, Player attackPlayer, NPC attackNPC)
        {
            base.Update(gameTime, attackPlayer, attackNPC);
        }
    }
}
