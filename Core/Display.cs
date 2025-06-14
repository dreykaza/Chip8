using System.Text;

namespace Chip8.Core;

public class Display
{
    public static bool[,] Pixels = new bool[32, 64];

    public static void ShowDisplay()
    {
        Console.Clear();
        var sb = new StringBuilder();

        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                sb.Append(Pixels[i, j] ? "██" : "  ");
            }
            sb.AppendLine();
        }

        Console.Write(sb.ToString());
    }

}
