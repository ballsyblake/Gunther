using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RC_Framework;

namespace MiniGame
{
    class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Sprite3 target)
        {
            var position = Matrix.CreateTranslation(
              -target.getPosX() - (target.getWidth() / 2),
              -target.getPosY() - (target.getHeight() / 2),
              0);

            var offset = Matrix.CreateTranslation(
                Game1.screenWidth / 2,
                Game1.screenHeight / 2,
                0);

            Transform = position * offset;
        }
    }
}
