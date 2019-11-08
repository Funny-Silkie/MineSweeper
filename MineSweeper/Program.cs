using asd;

namespace MineSweeper
{
    public class Program
    {
        public static void Main()
        {
            //Altseedの初期化
            Engine.Initialize("MineSweeper", 1280, 960, new EngineOption());
            Engine.File.AddRootPackageWithPassword("Resources.pack", "2236");
            //テクスチャの読み込み
            Base.GetTextures("Textures.fstxt");
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
