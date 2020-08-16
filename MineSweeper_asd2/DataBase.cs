using Altseed2;

namespace MineSweeper
{
    internal static class DataBase
    {
        public static Texture2D BlockTexture { get; } = Texture2D.LoadStrict("Resources/Wall.png");
        public static Texture2D BombTexture { get; } = Texture2D.LoadStrict("Resources/Bomb.png");
        public static Texture2D FragTexture { get; } = Texture2D.LoadStrict("Resources/Frag.png");
        public static Font MainFont { get; } = Font.LoadDynamicFontStrict("Resources/GenYoMinJP-Bold.ttf", 30);
        public static bool LeftPushed => _leftPushed ??= Engine.Mouse.GetMouseButtonState(MouseButton.ButtonLeft) == ButtonState.Push;
        private static bool? _leftPushed;
        public static bool RigntPushed => _rigntPushed ??= Engine.Mouse.GetMouseButtonState(MouseButton.ButtonRight) == ButtonState.Push;
        private static bool? _rigntPushed;
        internal static void Initialize()
        {

        }
        internal static void Update()
        {
            _leftPushed = null;
            _rigntPushed = null;
        }
    }
}
