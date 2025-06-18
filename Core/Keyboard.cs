namespace Chip8.Core;

public class Keyboard
{
    public static int curkey = -1;
    public static string[] Controls = new string[0x10] { "D1", "D2", "D3", "D4", "Q", "W", "E", "R", "A", "S", "D", "F", "Z", "X", "C", "V" };
    public static void KeyListner()
    {
        _ = Task.Run(() => KeySwitch());
        while (true)
        {
            ConsoleKeyInfo keyinfo = Console.ReadKey(true);
            curkey = Array.IndexOf(Controls, keyinfo.Key.ToString());
        }
    }

    public static void KeySwitch()
    {
        while (true)
        {
            curkey = -1;
            Thread.Sleep(70);
        }
    }
}

