using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using RC_Framework;

namespace MiniGame
{
    class Pause : RC_GameStateParent
    {
        Sprite3 book = null;
        public override void LoadContent()
        {
            book = new Sprite3(true, Game1.texBook, 50, 50);
            book.setWidthHeight(700, 500);
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(GameTime gameTime)
        {
            book.Draw(spriteBatch);
        }

        
    }
}
