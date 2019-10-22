using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using fslib;

namespace MineSweeper
{
    class Program
    {
        public static bool Break { get; set; } = false;
        static void Main(string[] args)
        {
            Engine.Initialize("MineSweeper", 960, 720, new EngineOption());
            var font = Engine.Graphics.CreateDynamicFont("NotoSerifCJKjp - Medium.otf", 30, new ColorDefault(ColorSet.White).AsdColor, 3, new ColorDefault(ColorSet.Black).AsdColor);
            var carsol = Engine.Sound.CreateSoundSource("Resources/Carsol.wav", true);
            var window = Engine.Graphics.CreateTexture2D("Resources/Widow.png");
            Central.Initialize(font, carsol, window);
            Base.GetTextures("Resources/Textures.txt");
            Engine.ChangeScene(new GameScene());
            while (Engine.DoEvents() && !Break)
                Engine.Update();
            Engine.Terminate();
        }
    }
}
