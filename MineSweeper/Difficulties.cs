using asd;
using fslib;
using static MineSweeper.Base;

namespace MineSweeper
{
    /// <summary>
    /// 難易度
    /// </summary>
    enum Dif : byte
    {
        Easy = 1,
        Normal,
        Hard
    }
    /// <summary>
    /// 難易度選択用のテキスト
    /// </summary>
    class Difficulties : ClickableText
    {
        /// <summary>
        /// クリック時に返す難易度
        /// </summary>
        public Dif ReturnDif { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">座標</param>
        /// <param name="text">表示文字列</param>
        /// <param name="difficulty">難易度</param>
        public Difficulties(Vector2DF position, string text, Dif difficulty)
        {
            ReturnDif = difficulty;
            Font = BigFont;
            Position = position;
            Text = text;
        }
        public override void OnLeftPushed()
        {
            var scene = (GameScene)Layer.Scene;
            scene.Difficulty = ReturnDif;
        }
        protected override void OnUpdate()
        {
            var scene = (GameScene)Layer.Scene;
            base.OnUpdate();
            Color = ReturnDif == scene.Difficulty ? new ColorDefault(ColorSet.Green) : new ColorDefault(ColorSet.White);
        }
    }
    class ResetText : ClickableText
    {
        public ResetText(Vector2DF position) : base(BigFont, "Reset", position) { }
        public override void OnLeftPushed()
        {
            Color = new ColorDefault(ColorSet.Red);
            ((GameScene)Layer.Scene).MakeFloor();
        }
        public override void OnLeftReleased()
        {
            Color = new ColorDefault(ColorSet.White);
        }
    }
}
