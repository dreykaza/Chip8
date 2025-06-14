namespace Chip8.Core;

public class Instruction
{
    public delegate void OpHandler(ushort opcode);
    public static OpHandler[] instruction = new OpHandler[16];

    public static OpHandler[] CreateInstructionTable()
    {
        instruction[0x0] = Group0;
        instruction[0x1] = opcode => Jump((ushort)(opcode & 0x0FFF));
        instruction[0x2] = opcode => Call((ushort)(opcode & 0x0FFF));
        instruction[0x3] = opcode => SkipIfEqual((ushort)((opcode & 0x0F00) >> 8), (ushort)(opcode & 0x00FF));
        instruction[0x4] = opcode => SkipIfNotEqual((ushort)((opcode & 0x0F00) >> 8), (ushort)(opcode & 0x00FF));
        instruction[0x5] = opcode => SkipIfEqualReg((ushort)((opcode & 0x0F00) >> 8), (ushort)((opcode & 0x00F0) >> 4));
        instruction[0x6] = opcode => SetReg((ushort)((opcode & 0x0F00) >> 8), (byte)(opcode & 0x00FF));
        instruction[0x7] = opcode => Add((ushort)((opcode & 0x0F00) >> 8), (byte)(opcode & 0x00FF));
        instruction[0x8] = opcode => Group8((ushort)((opcode & 0x0F00) >> 8), (ushort)((opcode & 0x00F0) >> 4), (ushort)(opcode & 0x000F));
        instruction[0x9] = opcode => SkipIfNotEqualReg((ushort)((opcode & 0x0F00) >> 8), (ushort)((opcode & 0x00F0) >> 4));
        instruction[0xA] = opcode => SetI((ushort)(opcode & 0x0FFF));
        instruction[0xB] = opcode => JumpV0((ushort)(opcode & 0x0FFF));
        instruction[0xC] = opcode => RandomWithAnd((ushort)((opcode & 0x0F00) >> 8), (ushort)(opcode & 0x00FF));
        instruction[0xD] = opcode => Draw((ushort)((opcode & 0x0F00) >> 8), (ushort)((opcode & 0x00F0) >> 4), (ushort)(opcode & 0x000F));
        instruction[0xE] = opcode => GroupE((ushort)((opcode & 0x0F00) >> 8), (ushort)(opcode & 0x00FF));
        instruction[0xF] = opcode => GroupF((ushort)((opcode & 0x0F00) >> 8), (ushort)(opcode & 0x00FF));
        return instruction;
    }

    public static void Group0(ushort opcode)
    {
        switch (opcode)
        {
            case 0x000:
                break;

            case 0x0E0:
                for (int i = 0; i < 32; i++)
                    for (int j = 0; j < 64; j++)
                        Display.Pixels[i, j] = false;
                break;


            case 0x0EE:
                CPU.PC = CPU.Stack[CPU.SP];
                CPU.SP--;
                break;
        }
    }

    public static void Group8(ushort Vx, ushort Vy, ushort code)
    {
        switch (code)
        {
            case 0x0:
                CPU.Registres[Vx] = CPU.Registres[Vy];
                break;

            case 0x1:
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] | CPU.Registres[Vy]);
                break;

            case 0x2:
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] & CPU.Registres[Vy]);
                break;

            case 0x3:
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] ^ CPU.Registres[Vy]);
                break;

            case 0x4:
                CPU.Registres[15] = 0;
                try
                {
                    CPU.Registres[Vx] = checked((byte)(CPU.Registres[Vx] + CPU.Registres[Vy]));
                }
                catch (OverflowException)
                {
                    CPU.Registres[15] = 1;
                    CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] + CPU.Registres[Vy]);
                }
                break;

            case 0x5:
                CPU.Registres[15] = 1;
                try
                {
                    CPU.Registres[Vx] = checked((byte)(CPU.Registres[Vx] - CPU.Registres[Vy]));
                }
                catch (OverflowException)
                {
                    CPU.Registres[15] = 0;
                    CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] - CPU.Registres[Vy]);
                }
                break;

            case 0x6:
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] / 2);
                CPU.Registres[15] = (byte)(CPU.Registres[Vx] % 2);
                break;

            case 0x7:
                CPU.Registres[15] = 0;
                try
                {
                    CPU.Registres[Vx] = checked((byte)(CPU.Registres[Vy] - CPU.Registres[Vx]));
                }
                catch (OverflowException)
                {
                    CPU.Registres[15] = 1;
                    CPU.Registres[Vx] = (byte)(CPU.Registres[Vy] - CPU.Registres[Vx]);
                }
                break;

            case 0xE:
                bool overflow = (CPU.Registres[Vx] & 0x80) != 0;
                CPU.Registres[15] = (byte)(overflow ? 1 : 0);
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] << 1);
                break;
        }
    }

    public static void Jump(ushort position) => CPU.PC = position;

    public static void Call(ushort position)
    {
        CPU.SP++;
        CPU.Stack[CPU.SP] = CPU.PC;
        CPU.PC = position;
    }

    public static void SkipIfEqual(ushort Vx, ushort integer) =>
        CPU.PC += (ushort)(CPU.Registres[Vx] == integer ? 2 : 0);

    public static void SkipIfNotEqual(ushort Vx, ushort integer) =>
        CPU.PC += (ushort)(CPU.Registres[Vx] == integer ? 0 : 2);

    public static void SkipIfEqualReg(ushort Vx, ushort Vy) =>
        CPU.PC += (ushort)(CPU.Registres[Vx] == CPU.Registres[Vy] ? 2 : 0);

    public static void SetReg(ushort Vx, byte integer) => CPU.Registres[Vx] = integer;

    public static void Add(ushort Vx, byte integer) => CPU.Registres[Vx] += integer;

    public static void SkipIfNotEqualReg(ushort Vx, ushort Vy) =>
        CPU.PC += (ushort)(CPU.Registres[Vx] == CPU.Registres[Vy] ? 0 : 2);

    public static void SetI(ushort integer) => CPU.I = integer;

    public static void JumpV0(ushort position) => CPU.PC = (ushort)(position + CPU.Registres[0]);

    public static void RandomWithAnd(ushort Vx, ushort kk) =>
        CPU.Registres[Vx] = (byte)((new Random()).Next(255) & kk);

    public static void Draw(ushort Vx, ushort Vy, ushort n)
    {
        CPU.Registres[15] = 0;
        int x, y;
        byte[] sprite = new byte[n];
        for (int i = 0; i < n; i++)
            sprite[i] = CPU.Memory[CPU.I + i];
        bool[] bits = new bool[8];
        int layer = 0;
        foreach (var item in sprite)
        {
            if (CPU.Registres[Vy] + layer >= 32) { y = CPU.Registres[Vy] - 32 + layer; } else { y = CPU.Registres[Vy] + layer; }
            for (int i = 0; i < 8; i++)
                bits[i] = (item & (1 << (7 - i))) != 0;

            for (int j = 0; j < bits.Length; j++)
            {
                if (CPU.Registres[Vx] + j >= 64) { x = CPU.Registres[Vx] - 64 + j; } else { x = CPU.Registres[Vx] + j; }
                if (Display.Pixels[y, x]
                        && bits[j])
                {
                    CPU.Registres[15] = 1;
                    Display.Pixels[y, x] ^= bits[j];
                }
                else
                {
                    Display.Pixels[y, x] ^= bits[j];
                }
            }
            layer++;
        }
    }

    public static void GroupE(ushort Vx, ushort code)
    {
        switch (code)
        {
            case 0x9E:
                CPU.PC += (ushort)(Keyboard.curkey == Keyboard.Controls[Vx] ? 2 : 0);
                break;

            case 0xA1:
                CPU.PC += (ushort)(Keyboard.curkey == Keyboard.Controls[Vx] ? 0 : 2);
                break;
        }
    }
    public static void GroupF(ushort Vx, ushort code)
    {
        switch (code)
        {
            case 0x07:
                CPU.Registres[Vx] = (byte)(CPU.DT);
                break;

            case 0x0A:
                while (true)
                {
                    if (Array.Find(Keyboard.Controls, cur => cur == Keyboard.curkey) != "0")
                    {
                        break;
                    }
                }
                break;

            case 0x15:
                CPU.DT = CPU.Registres[Vx];
                break;

            case 0x18:
                CPU.ST = CPU.Registres[Vx];
                break;

            case 0x1E:
                CPU.I += CPU.Registres[Vx];
                break;

            case 0x29:
                CPU.I = (ushort)(CPU.Registres[Vx] * 5);
                break;

            case 0x33:
                for (int i = 0; i < Vx.ToString().Length; i++)
                    CPU.Memory[CPU.I + i] = (byte)(Vx % (10 ^ (Vx.ToString().Length - i)));
                break;

            case 0x55:
                for (int i = 0; i <= Vx; i++)
                    CPU.Memory[CPU.I + i] = (byte)(CPU.Registres[i]);
                break;

            case 0x65:
                for (int i = 0; i <= Vx; i++)
                    CPU.Registres[i] = CPU.Memory[CPU.I + i];
                break;
        }
    }
}

