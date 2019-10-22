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
    /// 壁を壊した後に出てくる数字
    /// </summary>
    class Character : TextObject2D
    {
        /// <summary>
        /// セルとしての座標
        /// </summary>
        public Vector2DI CellPosition { get; }
        /// <summary>
        /// 周囲の爆弾の数
        /// </summary>
        public byte Mines { get; private set; }
        /// <summary>
        /// マウスが自身と被っているか
        /// </summary>
        public bool MouseCollide => IsCollide();
        /// <summary>
        /// 左クリックされているか
        /// </summary>
        public bool LeftClicked => MouseCollide && Engine.Mouse.LeftButton.ButtonState == ButtonState.Push;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cellPos">セルの座標</param>
        public Character(Vector2DI cellPos)
        {
            CellPosition = cellPos;
            Position = CellPosition.To2DF() * CellSize;
            //微調整
            Position += new Vector2DF(0.3f, -0.7f) * CellSize / 2;
        }
        protected override void OnAdded()
        {
            Font = Base.Font;
            Mines = DetectMines();
            //周囲の爆弾の数が0じゃなかったらテキストを設定
            Text = Mines == 0 ? "" : Mines.ToString();
            Color = ColorDetermination(Mines);
        }
        /// <summary>
        /// マウスと被っているかを返す
        /// </summary>
        private bool IsCollide()
        {
            var pos = Engine.Mouse.Position;
            var x = Position.X <= pos.X && pos.X <= Position.X + CellSize;
            var y = Position.Y <= pos.Y && pos.Y <= Position.Y + CellSize;
            return x && y;
        }
        /// <summary>
        /// 数字に合わせて色を決定する
        /// </summary>
        /// <param name="mines">爆弾の数</param>
        private Color ColorDetermination(byte mines)
        {
            switch (mines)
            {
                case 1:
                    return new Color(0, 0, 255);
                case 2:
                    return new Color(0, 255, 0);
                case 3:
                    return new Color(255, 0, 0);
                case 4:
                    return new Color(0, 0, 180);
                case 5:
                    return new Color(180, 0, 0);
                case 6:
                    return new Color(0, 180, 180);
                case 7:
                    return new Color(180, 180, 0);
                default:
                    return new Color(255, 255, 255);
            }
        }
        /// <summary>
        /// 周囲の爆弾の数を返す。
        /// </summary>
        private byte DetectMines() => (byte)Layer.Objects.OfType<Mine>().Count(x => Math.Abs(x.CellPosition.X - CellPosition.X) <= 1 && Math.Abs(x.CellPosition.Y - CellPosition.Y) <= 1);
    }
}
