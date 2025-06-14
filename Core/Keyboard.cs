namespace Chip8.Core;

public class Keyboard
{
    public static string curkey = "";
    public static string[] Controls = new string[0x10] { "D1", "D2", "D3", "D4", "Q", "W", "E", "R", "A", "S", "D", "F", "Z", "X", "C", "V" };
    public static void KeyListner()
    {
        while (true)
        {
            ConsoleKeyInfo keyinfo = Console.ReadKey(true);
            curkey = keyinfo.Key.ToString();
        }
    }
}

