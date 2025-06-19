using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Chip8.Core;

public class Keyboard
{
    public static int curkey;

    private static KeyboardKey[] KeyBindings = new KeyboardKey[0x10]
    {
        KeyboardKey.One,
        KeyboardKey.Two,
        KeyboardKey.Three,
        KeyboardKey.Four,
        KeyboardKey.Q,
        KeyboardKey.W,
        KeyboardKey.E,
        KeyboardKey.R,
        KeyboardKey.A,
        KeyboardKey.S,
        KeyboardKey.D,
        KeyboardKey.F,
        KeyboardKey.Z,
        KeyboardKey.X,
        KeyboardKey.C,
        KeyboardKey.V
    };

    public static void KeyListner()
    {
        for (int i = 0; i < KeyBindings.Length; i++)
        {
            if (IsKeyDown(KeyBindings[i]))
            {
                curkey = i;
                return;
            }
        }
        curkey = -1;
    }
}
