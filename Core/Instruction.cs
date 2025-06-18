namespace Chip8.Core;

public class Instruction
{
    public delegate void OpHandler(ushort opcode);
    private static OpHandler[] instruction = new OpHandler[16];

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
        bool overflow;
        switch (code)
        {
            case 0x0:
                CPU.Registres[Vx] = CPU.Registres[Vy];
                break;

            case 0x1:
                CPU.Registres[Vx] |= CPU.Registres[Vy];
                break;

            case 0x2:
                CPU.Registres[Vx] &= CPU.Registres[Vy];
                break;

            case 0x3:
                CPU.Registres[Vx] ^= CPU.Registres[Vy];
                break;

            case 0x4:
                int sum = CPU.Registres[Vx] + CPU.Registres[Vy];
                CPU.Registres[Vx] = (byte)sum;
                CPU.Registres[15] = (byte)(sum > 255 ? 1 : 0);
                break;

            case 0x5:
                overflow = CPU.Registres[Vx] >= CPU.Registres[Vy] ? true : false;
                CPU.Registres[Vx] -= CPU.Registres[Vy];
                CPU.Registres[15] = overflow ? (byte)1 : (byte)0; ;
                break;

            case 0x6:
                overflow = Convert.ToBoolean(CPU.Registres[Vx] & 0x01);
                CPU.Registres[Vx] >>= 1;
                CPU.Registres[15] = (byte)(overflow ? 1 : 0);
                break;

            case 0x7:
                overflow = CPU.Registres[Vy] >= CPU.Registres[Vx] ? true : false;
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vy] - CPU.Registres[Vx]);
                CPU.Registres[15] = overflow ? (byte)1 : (byte)0; ;
                break;

            case 0xE:
                overflow = (CPU.Registres[Vx] & 0x80) != 0;
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] << 1);
                CPU.Registres[15] = (byte)(overflow ? 1 : 0);
                break;
        }
    }

    public static void Jump(ushort position) => CPU.PC = (ushort)(position - 2);

    public static void Call(ushort position)
    {
        CPU.SP++;
        CPU.Stack[CPU.SP] = CPU.PC;
        CPU.PC = position;
        CPU.PC -= 2;
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
        for (int row = 0; row < n; row++)
        {
            byte spritelayer = CPU.Memory[CPU.I + row];
            int y = (CPU.Registres[Vy] + row + 1) % 32;
            for (int col = 0; col < 8; col++)
            {
                bool pixel = ((spritelayer >> (7 - col)) & 1) == 1;
                int x = (CPU.Registres[Vx] + col) % 64;
                if (pixel)
                {
                    if (Display.Pixels[y, x])
                        CPU.Registres[15] = 1;
                    Display.Pixels[y, x] ^= true;

                }
            }
        }
    }

    public static void GroupE(ushort Vx, ushort code)
    {
        switch (code)
        {
            case 0x9E:
                CPU.PC += (ushort)(Keyboard.curkey == Array.IndexOf(Keyboard.Controls, Keyboard.Controls[Vx]) ? 2 : 0);
                break;

            case 0xA1:
                CPU.PC += (ushort)(Keyboard.curkey == Array.IndexOf(Keyboard.Controls, Keyboard.Controls[Vx]) ? 0 : 2);
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
                // while (true)
                // {
                //     if (!(Keyboard.curkey == -1))
                //     {
                //         CPU.Registres[Vx] = (byte)(Keyboard.curkey);
                //         break;
                //     }
                // }
                break;

            case 0x15:
                CPU.DT = CPU.Registres[Vx];
                break;

            case 0x18:
                CPU.ST = CPU.Registres[Vx];
                break;

            case 0x1E:
                ushort sum = (ushort)(CPU.I + CPU.Registres[Vx]);
                CPU.Registres[0xF] = (byte)(sum > 0xFFF ? 1 : 0); // Установка VF
                CPU.I = sum;
                break;

            case 0x29:
                CPU.I = (ushort)(CPU.Registres[Vx] * 5);
                break;

            case 0x33:
                CPU.Memory[CPU.I] = (byte)(CPU.Registres[Vx] / 100);
                CPU.Memory[CPU.I + 1] = (byte)((CPU.Registres[Vx] / 10) % 10);
                CPU.Memory[CPU.I + 2] = (byte)(CPU.Registres[Vx] % 10);
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

