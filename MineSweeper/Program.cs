using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using fslib;

namespace MineSweeper
{
    public class Program
    {
        public static bool Break { get; set; } = false;
        static void Main(string[] args)
        {
            Engine.Initialize("MineSweeper", 1280, 960, new EngineOption());
            Base.GetTextures("Resources/Textures.txt");
            Engine.ChangeScene(new GameScene());
            while (Engine.DoEvents() && !Break)
                Engine.Update();
            Engine.Terminate();
        }
    }
}
