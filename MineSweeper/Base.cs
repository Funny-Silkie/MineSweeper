﻿using System.Collections.Generic;
using asd;
using fslib;

namespace MineSweeper
{
    /// <summary>
    /// データの保管等
    /// </summary>
    static class Base
    {
        /// <summary>
        /// 文字用のフォント
        /// </summary>
        public static Font Font { get; } = Engine.Graphics.CreateDynamicFont("NotoSerifCJKjp-Medium.otf", 30, new ColorDefault(ColorSet.White).AsdColor, 1, new ColorDefault(ColorSet.Black).AsdColor);
        /// <summary>
        /// 大きいフォント
        /// </summary>
        public static Font BigFont { get; } = Engine.Graphics.CreateDynamicFont("NotoSerifCJKjp-Medium.otf", 50, new ColorDefault(ColorSet.White).AsdColor, 2, new ColorDefault(ColorSet.Black).AsdColor);
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
            var str = IOHandler.CsvToCollectionS(filename, new EncodeOption(EncodingType.UTF8), ',');
            foreach (var s in str)
            {
                foreach (var t in s)
                    t.Trim();
                Textures.Add(s[0], Engine.Graphics.CreateTexture2D("" + s[1]));
            }
        }
    }
}
