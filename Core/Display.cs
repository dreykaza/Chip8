using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Chip8.Core;

public class Display
{
    public static byte CellSize = 15;
    public static bool[,] Pixels = new bool[32, 64];

    public static void ShowDisplay()
    {
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                DrawRectangle(j * CellSize, i * CellSize, CellSize, CellSize, Pixels[i, j] ? Color.White : Color.Black);
            }
        }
    }

}
