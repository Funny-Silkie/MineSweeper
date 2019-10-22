using System;
using System.Collections.Generic;
using System.Linq;
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
        private int count = 0;
        /// <summary>
        /// 難易度
        /// </summary>
        public Dif Difficulty { get; set; } = Dif.Easy;
        /// <summary>
        /// ゲームオーバーかどうかを返す
        /// </summary>
        public bool IsGameOver
        {
            get => _go;
            set
            {
                if (!IsClear && value && !IsGameOver)
                    uiLayer.AddObject(gameoverText); 
                _go = value;
            }
        }
        private bool _go = false;
        /// <summary>
        /// クリアしたかどうかを返す
        /// </summary>
        private bool IsClear
        {
            get => _clear;
            set
            {
                _clear = value;
                if (value)
                {
                    IsGameOver = true;
                    uiLayer.AddObject(clearText);
                }
            }
        }
        private bool _clear = false;
        //爆弾の数を表示
        private readonly AllmightyText t_mines = new AllmightyText(new Vector2DF(980, 500), "Mines:50") { Font = BigFont };
        //旗の数を表示
        private readonly AllmightyText t_frags = new AllmightyText(new Vector2DF(1000, 580), "Frags:0") { Font = BigFont };
        //難易度を表す文字を表示
        private readonly Difficulties t_easy = new Difficulties(new Vector2DF(1050, 200), "Easy", Dif.Easy);
        private readonly Difficulties t_normal = new Difficulties(new Vector2DF(1000, 270), "Normal", Dif.Normal);
        private readonly Difficulties t_hard = new Difficulties(new Vector2DF(1050, 340), "Hard", Dif.Hard);
        //ゲームオーバーテキスト
        private readonly AllmightyText gameoverText = new AllmightyText(new Vector2DF(1030, 30), "Game\n Over") { Font = BigFont };
        private readonly AllmightyText clearText = new AllmightyText(new Vector2DF(1050, 30), "Clear") { Font = BigFont };
        //地雷の個数
        private byte Mines => (byte)((int)Difficulty * 100 - 50);
        //横のマスの数
        private const byte sizeX = 30;
        //縦のマスの数
        private const byte sizeY = 30;
        //乱数
        private readonly Random random = new Random(DateTime.Now.Second);
        //メインとなるレイヤー
        private readonly Layer2D mainLayer = new Layer2D();
        //背景を追加するレイヤー
        private readonly Layer2D backLayer = new Layer2D();
        //UIを表示するレイヤー
        private readonly Layer2D uiLayer = new Layer2D();
        protected override void OnRegistered()
        {
            backLayer.DrawingPriority -= 1;
            AddLayer(backLayer);
            backLayer.AddObject(new TextureObject2D()
            {
                Texture = Textures["Back"],
                Scale = new Vector2DF(sizeX * CellSize, sizeY * CellSize)
            });
            AddLayer(mainLayer);
            MakeFloor(mainLayer);
            uiLayer.DrawingPriority += 10;
            AddLayer(uiLayer);
            uiLayer.AddObject(t_easy);
            uiLayer.AddObject(t_normal);
            uiLayer.AddObject(t_hard);
            uiLayer.AddObject(t_mines);
            uiLayer.AddObject(t_frags);
        }
        protected override void OnUpdated()
        {
            //Rキーでオブジェクト再配置
            if (Engine.Keyboard.GetKeyState(Keys.R) == ButtonState.Push)
                MakeFloor(mainLayer);
            //壁を全て壊したらクリア
            if (!IsClear && mainLayer.Objects.OfType<Wall>().Count() == 0 && count > 0)
            {
                IsClear = true;
                foreach (var m in mainLayer.Objects.OfType<Mine>())
                    m.IsFragged = true;
            }
            if (gameoverText.Layer != null)
                uiLayer.DrawSpriteAdditionally(new Vector2DF(1010, 20), new Vector2DF(1220, 20), new Vector2DF(1220, 200), new Vector2DF(1010, 200), new Color(255, 255, 0), new Color(255, 255, 0), new Color(255, 255, 0), new Color(255, 255, 0), new Vector2DF(), new Vector2DF(), new Vector2DF(), new Vector2DF(), Textures["Back"], AlphaBlendMode.Add, -1);
            if (clearText.Layer != null)
                uiLayer.DrawSpriteAdditionally(new Vector2DF(1010, 30), new Vector2DF(1250, 30), new Vector2DF(1250, 110), new Vector2DF(1010, 110), new Color(100, 255, 100), new Color(100, 255, 100), new Color(100, 255, 100), new Color(100, 255, 100), new Vector2DF(), new Vector2DF(), new Vector2DF(), new Vector2DF(), Textures["Back"], AlphaBlendMode.Add, -1);
            t_frags.Text = "Frags:" + mainLayer.Objects.OfType<MObject>().Count(x => x.IsFragged);
            count++;
        }
        /// <summary>
        /// 各種オブジェクトの配置を行うメソッド
        /// </summary>
        /// <param name="layer">オブジェクトを配置するレイヤー</param>
        private void MakeFloor(Layer2D layer)
        {
            t_mines.Text = "Mines:" + Mines.ToString();
            if (gameoverText.Layer != null)
                uiLayer.RemoveObject(gameoverText);
            if (clearText.Layer != null)
                uiLayer.RemoveObject(clearText);
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
            for (int i = 0; i < Mines; i++)
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
            IsClear = false;
            count = 0;
        }
    }
}
