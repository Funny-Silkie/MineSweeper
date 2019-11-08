using System;
using System.Collections.Generic;
using System.Linq;
using asd;
using fslib;
using static MineSweeper.Base;

namespace MineSweeper
{
    /// <summary>
    /// オブジェクトの抽象クラス
    /// </summary>
    abstract class MObject : TextureObject2D
    {
        /// <summary>
        /// セルとしての座標
        /// </summary>
        public Vector2DI CellPosition { get; set; }
        /// <summary>
        /// 旗が付いているかどうか
        /// </summary>
        public bool IsFragged 
        {
            get => _frag;
            set
            {
                if (value)
                    Texture = Textures["Frag"];
                else
                    Texture = Textures["Wall"];
                _frag = value;
            }
        }   
        private bool _frag = false;
        /// <summary>
        /// マウスが自身と被っているか
        /// </summary>
        public bool MouseCollide => IsCollide();
        /// <summary>
        /// 左クリックされているか
        /// </summary>
        public bool LeftClicked => MouseCollide && Engine.Mouse.LeftButton.ButtonState == ButtonState.Push;
        /// <summary>
        /// 右クリックされているか
        /// </summary>
        public bool RightClicked => MouseCollide && Engine.Mouse.RightButton.ButtonState == ButtonState.Push;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cellPos">セルの座標</param>
        public MObject(Vector2DI cellPos)
        {
            CellPosition = cellPos;
            Position = CellPosition.To2DF() * CellSize;
            Texture = Textures["Wall"];
        }
        protected override void OnUpdate()
        {
            var scene = (GameScene)Layer.Scene;
            //左クリックされたときにメソッドを実行
            if (LeftClicked && !scene.IsGameOver)
                OnLeftClicked();
            //右クリックされたときにメソッドを実行
            if (RightClicked && !scene.IsGameOver)
                OnRightClicked();
        }
        /// <summary>
        /// 左クリックされたときに実行
        /// </summary>
        public virtual void OnLeftClicked() { }
        /// <summary>
        /// 右クリックされたときに実行
        /// </summary>
        protected virtual void OnRightClicked() 
        {
            IsFragged = !IsFragged;
        }
        /// <summary>
        /// マウスと被っているかを返す
        /// </summary>
        private bool IsCollide()
        {
            var pos = Engine.Mouse.Position;
            var x = Position.X <= pos.X && pos.X <= Position.X + Texture.Size.X;
            var y = Position.Y <= pos.Y && pos.Y <= Position.Y + Texture.Size.Y;
            return x && y;
        }
    }
    /// <summary>
    /// 爆弾のクラス
    /// </summary>
    class Mine : MObject
    {
        /// <summary>
        /// クリックされたか
        /// </summary>
        public bool IsShown { get; private set; }
        /// <summary>
        /// 座標を管理
        /// </summary>
        public static List<Vector2DI> Positions { get; }
        static Mine()
        {
            Positions = new List<Vector2DI>();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cellPos">セルの座標</param>
        public Mine(Vector2DI cellPos)
            : base(cellPos)
        {
            Positions.Add(cellPos);
        }
        public override void OnLeftClicked()
        {
            if (!IsFragged)
            {
                //テクスチャ変更
                Texture = Textures["Bomb"];
                IsShown = true;
                var scene = (GameScene)Layer.Scene;
                foreach (var b in scene.MainLayer.Objects)
                    if (b is Mine bomb) bomb.Texture = Textures["Bomb"];
                scene.IsGameOver = true;
            }
        }
        protected override void OnRightClicked()
        {
            //起爆していない時だけ旗の着脱可能に
            if (!IsShown)
                base.OnRightClicked();
        }
    }
    /// <summary>
    /// 壁のオブジェクトクラス
    /// </summary>
    class Wall : MObject
    {
        //周囲の爆弾の個数
        private byte mines;
        //自身を一括破壊の判定から外すようにする
        private bool noDetected = false;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cellPos">セル座標</param>
        public Wall(Vector2DI cellPos)
            : base(cellPos) { }
        protected override void OnAdded()
        {
            //爆弾の個数判定
            mines = DetectMines();
        }
        public override void OnLeftClicked()
        {
            if (!IsFragged)
            {
                noDetected = true;
                if (mines == 0)
                    BundleBreak();
                //文字を追加し自身を削除
                Layer.AddObject(new Character(CellPosition));
                Dispose();
            }
        }
        /// <summary>
        /// 一括破壊するメソッド
        /// </summary>
        private void BundleBreak()
        {
            //壁をすべて検索する。
            foreach (var w in Layer.Objects.OfType<Wall>())
                //一括破壊対象かつ自身の周囲の壁の場合左クリック時の操作を実行
                if (!w.noDetected && Math.Abs(w.CellPosition.X - CellPosition.X) <= 1 && Math.Abs(w.CellPosition.Y - CellPosition.Y) <= 1)
                    w.OnLeftClicked();
        }
        /// <summary>
        /// 周囲の爆弾の個数を判定
        /// </summary>
        private byte DetectMines() => (byte)Layer.Objects.OfType<Mine>().Count(x => Math.Abs(x.CellPosition.X - CellPosition.X) <= 1 && Math.Abs(x.CellPosition.Y - CellPosition.Y) <= 1);
    }
}
