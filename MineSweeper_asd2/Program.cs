using Altseed2;

namespace MineSweeper
{
    internal class Program
    {
        static void Main()
        {
            if (!Engine.Initialize("MineSweeper", 1280, 960)) return;
            DataBase.Initialize();
            Engine.AddNode(new MainScene());
            while (Engine.DoEvents())
            {
                DataBase.Update();
                Engine.Update();
            }
            Engine.Terminate();
        }
    }
}
