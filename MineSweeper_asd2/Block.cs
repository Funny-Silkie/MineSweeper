using Altseed2;

namespace MineSweeper
{
    internal class Block : SpriteNode
    {
        private readonly MainScene scene;
        private readonly TextNode text = new TextNode()
        {
            Font = DataBase.MainFont,
            ZOrder = 1 
        };
        public bool Clicked
        {
            get => _clicked;
            set
            {
                if (_clicked == value) return;
                _clicked = value;
                if (value)
                {
                    if (IsBomb)
                    {
                        Texture = DataBase.BombTexture;
                        text.Text = string.Empty;
                        scene.GameOver();
                    }
                    else
                    {
                        Texture = null;
                        var count = Count;
                        text.Text = count == 0 ? string.Empty : count.ToString();
                        text.Color = ColorDetermination(count);
                        text.CenterPosition = text.ContentSize / 2f;
                        scene.NoMineCount--;
                        if (scene.NoMineCount == 0) scene.Clear();
                        if (count == 0)
                            for (int x = -1; x <= 1; x++)
                                for (int y = -1; y <= 1; y++)
                                {
                                    if (x == 0 && y == 0) continue;
                                    if (scene.Blocks.TryGetValue(new Vector2I(Location.X + x, Location.Y + y), out var block)) block.Clicked = true; 
                                }
                    }
                }
                else
                {
                    Texture = DataBase.BlockTexture;
                    text.Text = string.Empty;
                }
                CenterPosition = ContentSize / 2f;
            }
        }
        private bool _clicked;
        public int Count
        {
            get
            {
                var result = 0;
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        if (scene.Blocks.TryGetValue(new Vector2I(Location.X + x, Location.Y + y), out var block) && block.IsBomb) result++;
                    }
                return result;
            }
        }
        public bool IsBomb { get; set; }
        public Vector2I Location { get; }
        public Block(MainScene scene, Vector2I location)
        {
            this.scene = scene;
            CenterPosition = new Vector2F(16f, 16f);
            Location = location;
            Position = location * 32 + CenterPosition;
            Texture = DataBase.BlockTexture;
            AddChildNode(text);
        }
        private static Color ColorDetermination(int count) => count switch
        {
            1 => new Color(0, 0, 255),
            2 => new Color(0, 255, 0),
            3 => new Color(255, 0, 0),
            4 => new Color(0, 0, 180),
            5 => new Color(180, 0, 0),
            6 => new Color(0, 180, 180),
            7 => new Color(180, 180, 0),
            _ => new Color(255, 255, 255),
        };
        protected override void OnUpdate()
        {
            if (scene.Locked) return;
            var entered = IsMouseEntered();
            if (DataBase.LeftPushed && entered) Clicked = true;
            if (!Clicked && DataBase.RigntPushed && entered) Texture = DataBase.BlockTexture == Texture ? DataBase.FragTexture : DataBase.BlockTexture;
        }
        private bool IsMouseEntered()
        {
            var p = Engine.Mouse.Position;
            return Position.X - ContentSize.X / 2 <= p.X && p.X <= Position.X + ContentSize.X / 2 && Position.Y - ContentSize.Y / 2 <= p.Y && p.Y <= Position.Y + ContentSize.Y / 2;
        }
    }
}
