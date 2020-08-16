using Altseed2;
using System;
using System.Collections.Generic;

namespace MineSweeper
{
    internal class MainScene : Node
    {
        private static readonly Random random = new Random(DateTime.Now.Second);
        private readonly TextNode text = new TextNode()
        {
            Font = DataBase.MainFont,
            Position = new Vector2F(1000f, 100f)
        };
        public int NoMineCount { get; set; }
        public Dictionary<Vector2I, Block> Blocks { get; } = new Dictionary<Vector2I, Block>(900);
        public bool Locked { get; private set; }
        protected override void OnAdded()
        {
            AddChildNode(text);
            for (int y = 0; y < 30; y++)
                for (int x = 0; x < 30; x++)
                {
                    var location = new Vector2I(x, y);
                    var block = new Block(this, location);
                    Blocks.Add(location, block);
                    AddChildNode(block);
                }
            SetMines();
        }
        protected override void OnUpdate()
        {
            if (Engine.Keyboard.GetKeyState(Key.R) == ButtonState.Push) Reset();   
        }
        internal void Clear()
        {
            Locked = true;
            text.Text = "Clear";
        }
        internal void GameOver()
        {
            Locked = true;
            text.Text = "GameOver";
        }
        private void Reset()
        {
            Locked = false;
            text.Text = string.Empty;
            foreach (var (_, block) in Blocks)
            {
                block.Clicked = false;
                block.Texture = DataBase.BlockTexture;
                block.IsBomb = false;
            }
            SetMines();
        }
        private void SetMines()
        {
            const int mineCount = 100;
            var mines = 0;
            while (mines < mineCount)
            {
                var location = new Vector2I(random.Next(30), random.Next(30));
                if (Blocks[location].IsBomb) continue;
                Blocks[location].IsBomb = true;
                mines++;
            }
            NoMineCount = 900 - mineCount;
        }
    }
}
