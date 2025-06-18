using Chip8.Core;

namespace Chip8;

public class Emulator
{
    private static Instruction.OpHandler[] Instructions = Instruction.CreateInstructionTable();

    public static void LoadProgram(byte[] program)
    {
        Array.Copy(FontSet.fontSet, 0, CPU.Memory, 0x0, FontSet.fontSet.Length);
        Array.Copy(program, 0, CPU.Memory, 0x200, program.Length);
    }

    public static void Step()
    {
        ushort opcode = (ushort)(CPU.Memory[CPU.PC] << 8 | CPU.Memory[CPU.PC + 1]);
        byte hiNibble = (byte)((opcode & 0xF000) >> 12);
        Instructions[hiNibble](opcode);

        CPU.PC += 2;
    }

    public static void Start(byte[] program)
    {
        LoadProgram(program);
        _ = Task.Run(() => Keyboard.KeyListner());
        _ = Task.Run(() => InstructionController());
        _ = Task.Run(() => FrameController());
    }

    public static void InstructionController()
    {
        while (true)
        {
            Step();
            Thread.Sleep(10);
        }
    }

    public static void FrameController()
    {
        int frame = 1000 / 60;
        while (true)
        {
            Display.ShowDisplay();
            Thread.Sleep(frame);
        }
    }
}
