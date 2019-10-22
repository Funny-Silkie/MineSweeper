using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using fslib;

namespace MineSweeper
{
    public class GameScene : Scene
    {
        private const byte mines = 35;
        private const byte sizeX = 30;
        private const byte sizeY = 21;
        private readonly Random random = new Random(DateTime.Now.Second);
        private readonly Layer2D uiLayer = new Layer2D();
        protected override void OnRegistered()
        {
            AddLayer(uiLayer);
            MakeFloor(uiLayer);
        }
        private void MakeFloor(Layer2D layer)
        {
            uiLayer.Clear();
            Mine.Positions.Clear();
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    layer.AddObject(new Wall(new Vector2DI(i, j + 1)));
            for (int i = 0; i < mines; i++)
            {
                random.Next();
                var b = true;
                while (b)
                {
                    var vec = new Vector2DI(random.Next(sizeX + 1), random.Next(sizeY + 1));
                    b = Mine.Positions.Contains(vec);
                    if (b)
                    {
                        var obj = layer.Objects.OfType<Wall>().Single(x => x.CellPosition == vec);
                        layer.RemoveObject(obj);
                        layer.AddObject(new Mine(vec));
                    }
                }
            }
        }
    }
}
