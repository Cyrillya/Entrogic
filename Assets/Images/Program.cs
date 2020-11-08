using System;
using System.IO;

namespace Load
{
    class Program
    {
        static void Main(string[] args)
        {
            String path = @"C:\Users\Administrator\Documents\My Games\Terraria\ModLoader\Beta\Mod Sources\Entrogic\Images";

            //第一种方法
            var files = Directory.GetFiles(path, "*.png");

            foreach (var file in files)
            {
                string[] text = file.ToString().Split('.')[0].Split('\\');
                Console.WriteLine($"AddIntoTextureTable(\"{text[text.Length - 1]}\");");
            }
            Console.ReadLine();
        }
    }
}
