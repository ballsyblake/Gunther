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
            Console.WriteLine(position.M42);
            if (position.M41 > -479.4)
                position.M41 = -479.4f;
            if (position.M41 < -3509.4)
                position.M41 = -3509.4f;
            if (position.M42 > -379.2)
                position.M42 = -379.2f;
            if (position.M42 < -2619.2)
                position.M42 = -2619.2f;

            Transform = position * offset;
        }
    }
}
