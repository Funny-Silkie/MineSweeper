using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using fslib;

namespace MineSweeper
{
    /// <summary>
    /// データの保管等
    /// </summary>
    static class Base
    {
        public static Font Font { get; } = Engine.Graphics.CreateDynamicFont("Resources/NotoSerifCJKjp-Medium.otf", 30, new ColorDefault(ColorSet.White).AsdColor, 1, new ColorDefault(ColorSet.Black).AsdColor);
        /// <summary>
        /// テクスチャを保管しておくディクショナリ
        /// </summary>
        public static Dictionary<string, Texture2D> Textures { get; } = new Dictionary<string, Texture2D>();
        /// <summary>
        /// マスの大きさ
        /// </summary>
        public static int CellSize { get; } = 32;
        /// <summary>
        /// ファイルからテクスチャを読み込む。
        /// </summary>
        /// <param name="filename">読み込むファイル名</param>
        public static void GetTextures(string filename)
        {
            //指定したファイルを読み込み行ごとで区切ったのちに更に分割
            var str = FileReader.CsvToCollectionS(filename, ',');
            foreach (var s in str)
            {
                foreach (var t in s)
                    t.Trim();
                Textures.Add(s[0], Engine.Graphics.CreateTexture2D("Resources/" + s[1]));
            }
        }
    }
}
