using Raylib_cs;
using Chip8.Core;

namespace Chip8;

class Program
{
    static void Main(string[] args)
    {
        string name = args[0];
        byte[] game = File.ReadAllBytes(name);
        Emulator.Start(game);
        Raylib.InitWindow(980, 480, "Chip 8");
        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Display.ShowDisplay();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

