using Chip8.core;

namespace Chip8;

public class Emulator
{
    public void LoadProgram(byte[] program)
    {
        Array.Copy(program, 0, CPU.Memory, 0x200, program.Length);
    }

    public void Step()
    {
        ushort opcode = (ushort)(CPU.Memory[CPU.PC] << 8 | CPU.Memory[CPU.PC + 1]);


        CPU.PC += 2;

    }

    public void Run()
    {
        while (true)
        {
            Step();
            System.Threading.Thread.Sleep(500);
        }
    }
}
