using asd;

namespace MineSweeper
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Altseedの初期化
            Engine.Initialize("MineSweeper", 1280, 960, new EngineOption());
            //テクスチャの読み込み
            Base.GetTextures("Resources/Textures.txt");
            //シーン推移
            Engine.ChangeScene(new GameScene());
            //Altseedの更新
            while (Engine.DoEvents())
                Engine.Update();
            //Altseedの終了
            Engine.Terminate();
        }
    }
}
