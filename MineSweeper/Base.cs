using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asd;
using fslib;

namespace MineSweeper
{
    public static class Base
    {
        public static Dictionary<string, Texture2D> Textures { get; } = new Dictionary<string, Texture2D>();
        public const int CellSize = 32;
        public static void GetTextures(string filename)
        {
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
