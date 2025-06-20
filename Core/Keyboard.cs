using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Chip8.Core;

public class Keyboard
{
    public static int curkey;

    private static KeyboardKey[] KeyBindings = new KeyboardKey[0x10]
    {
        KeyboardKey.X,
        KeyboardKey.One,
        KeyboardKey.Two,
        KeyboardKey.Three,
        KeyboardKey.Q,
        KeyboardKey.W,
        KeyboardKey.E,
        KeyboardKey.A,
        KeyboardKey.S,
        KeyboardKey.D,
        KeyboardKey.Z,
        KeyboardKey.C,
        KeyboardKey.Four,
        KeyboardKey.R,
        KeyboardKey.F,
        KeyboardKey.V
    };

    public static int GetKey()
    {
        for (int i = 0; i < KeyBindings.Length; i++)
        {
            if (IsKeyDown(KeyBindings[i]))
            {
                return i;
            }
        }
        return -1;
    }
}
