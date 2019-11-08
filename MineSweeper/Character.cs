using System;
using System.Linq;
using asd;
using fslib;
using static MineSweeper.Base;

namespace MineSweeper
{
    /// <summary>
    /// 壁を壊した後に出てくる数字
    /// </summary>
    class Character : ClickableText
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
        public override void OnLeftPushed()
        {
            //周囲の壁と爆弾を取得
            var objs = Layer.Objects.OfType<MObject>().Where(x => Math.Abs(x.CellPosition.X - CellPosition.X) <= 1 && Math.Abs(x.CellPosition.Y - CellPosition.Y) <= 1);
            if (objs.Count(x => x.IsFragged) >= Mines)
            {
                foreach (var o in objs)
                    if (!o.IsFragged)
                        o.OnLeftClicked();
            }
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
