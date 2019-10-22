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
        /// マウスと被っているかどうか
        /// </summary>
        public override bool MouseCollide => IsCollide(320, 50);
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
        protected override void OnLeftClicked()
        {
            var scene = (GameScene)Layer.Scene;
            scene.Difficulty = ReturnDif;
        }
        protected override void OnRightClicked() => OnLeftClicked();
        protected override void OnUpdate()
        {
            var scene = (GameScene)Layer.Scene;
            base.OnUpdate();
            if (ReturnDif == scene.Difficulty)
                Color = new ColorDefault(ColorSet.Green).AsdColor;
            else
                Color = new ColorDefault(ColorSet.White).AsdColor;
        }
    }
}
