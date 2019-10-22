using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using fslib;
using static MineSweeper.Base;

namespace MineSweeper
{
    /// <summary>
    /// ゲームのシーン
    /// </summary>
    class GameScene : Scene
    {
        public bool IsGameOver { get; set; } = false;
        private const byte mines = 150;
        private const byte sizeX = 40;
        private const byte sizeY = 30;
        //乱数
        private readonly Random random = new Random(DateTime.Now.Second);
        //メインとなるレイヤー
        private readonly Layer2D mainLayer = new Layer2D();
        //背景を追加するレイヤー
        private readonly Layer2D backLayer = new Layer2D();
        protected override void OnRegistered()
        {
            backLayer.DrawingPriority -= 1;
            AddLayer(backLayer);
            backLayer.AddObject(new TextureObject2D()
            {
                Texture = Textures["Back"],
                Scale = Engine.WindowSize.To2DF()
            });
            AddLayer(mainLayer);
            MakeFloor(mainLayer);
        }
        protected override void OnUpdated()
        {
            if (Engine.Keyboard.GetKeyState(Keys.R) == ButtonState.Push)
                MakeFloor(mainLayer);
        }
        /// <summary>
        /// 各種オブジェクトの配置を行うメソッド
        /// </summary>
        /// <param name="layer">オブジェクトを配置するレイヤー</param>
        private void MakeFloor(Layer2D layer)
        {
            IsGameOver = false;
            mainLayer.Clear();
            Mine.Positions.Clear();
            var list = new List<Wall>();
            //壁の仮登録
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                {
                    var w = new Wall(new Vector2DI(i, j));
                    list.Add(w);
                }
            //ランダムな座標の壁を爆弾に置換する
            for (int i = 0; i < mines; i++)
            {
            CheckPoint:;
                var vec = new Vector2DI(random.Next(sizeX), random.Next(sizeY));
                if (!Mine.Positions.Contains(vec))
                {
                    var obj = list.Single(x => x.CellPosition == vec);
                    list.Remove(obj);
                    layer.AddObject(new Mine(vec));
                }
                else
                    goto CheckPoint;
            }
            //残った壁をレイヤーに登録
            foreach (var w in list)
                layer.AddObject(w);
        }
    }
}
