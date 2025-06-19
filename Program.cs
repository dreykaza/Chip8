using Raylib_cs;
using Chip8.Core;

namespace Chip8;

class Program
{
    static void Main(string[] args)
    {
        // Console.WriteLine("Name");
        // string name = Console.ReadLine();
        // string filePath = Path.Combine("ROMs", name);
        // byte[] game = File.ReadAllBytes(filePath);
        // Emulator.Start(game);
        // Task.Delay(-1).Wait();

        Raylib.InitWindow(980, 480, "Chip 8");
        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose())
        {
            Keyboard.KeyListner();
            if (!(Keyboard.curkey == -1))
            {
                Console.WriteLine(Keyboard.curkey);
            }
            Raylib.BeginDrawing();
            Display.ShowDisplay();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }


}
