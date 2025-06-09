namespace Chip8.core;

public class Instruction
{
    delegate void OpHandler(ushort opcode);

    OpHandler[] instruction = new OpHandler[16];

    public Instruction()
    {
        instruction[0x0] = Group0;
        instruction[0x1] = opcode => Jump((ushort)(opcode & 0x0FFF));
        instruction[0x2] = opcode => Call((ushort)(opcode & 0x0FFF));
        instruction[0x3] = opcode => SkipIfEqual((ushort)(opcode & 0x0F00 >> 8), (ushort)(opcode & 0x00FF));
        instruction[0x4] = opcode => SkipIfNotEqual((ushort)(opcode & 0x0F00 >> 8), (ushort)(opcode & 0x00FF));
        instruction[0x5] = opcode => SkipIfEqualReg((ushort)(opcode & 0x0F00 >> 8), (ushort)(opcode & 0x00F0 >> 4));
        instruction[0x6] = opcode => SetReg((ushort)(opcode & 0x0F00 >> 8), (byte)(opcode & 0x00FF));
        instruction[0x7] = opcode => Add((ushort)(opcode & 0x0F00 >> 8), (byte)(opcode & 0x00FF));
        instruction[0x8] = opcode => Group8((ushort)(opcode & 0x0F00 >> 8), (ushort)(opcode & 0x00F0 >> 4), (ushort)(opcode & 0x000F));
    }

    public void Group0(ushort opcode)
    {
    }

    public void Group8(ushort Vx, ushort Vy, ushort code)
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
                    CPU.Registres[Vx] = checked((byte)(CPU.Registres[Vx] + CPU.Registres[Vy]));
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
                    CPU.Registres[Vx] = checked((byte)(CPU.Registres[Vx] - CPU.Registres[Vy]));
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
                    CPU.Registres[Vx] = checked((byte)(CPU.Registres[Vy] - CPU.Registres[Vx]));
                }
                break;

            case 0xE:
                bool overflow = (CPU.Registres[Vx] & 0x80) != 0;
                CPU.Registres[15] = (byte)(overflow ? 1 : 0);
                CPU.Registres[Vx] = (byte)(CPU.Registres[Vx] << 1);
                break;
        }
    }

    public void Jump(ushort position) => CPU.PC = position;

    public void Call(ushort position)
    {
        CPU.SP++;
        CPU.Stack[CPU.SP] = CPU.PC;
        CPU.PC = position;
    }

    public void SkipIfEqual(ushort Vx, ushort integer) =>
        CPU.PC += (ushort)(CPU.Registres[Vx] == integer ? 2 : 0);

    public void SkipIfNotEqual(ushort Vx, ushort integer) =>
        CPU.PC += (ushort)(CPU.Registres[Vx] == integer ? 0 : 2);

    public void SkipIfEqualReg(ushort Vx, ushort Vy) =>
        CPU.PC += (ushort)(CPU.Registres[Vx] == CPU.Registres[Vy] ? 2 : 0);

    public void SetReg(ushort Vx, byte integer) => CPU.Registres[Vx] = integer;

    public void Add(ushort Vx, byte integer) => CPU.Registres[Vx] += integer;

}
