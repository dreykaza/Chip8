namespace Chip8.Core;

public class CPU
{
    public static byte[] Memory = new byte[4096];
    public static ushort PC = 0x200;
    public static ushort[] Stack = new ushort[16];
    public static int DT = 0;  // delay timer 
    public static int ST = 0;  // sound timer
    public static int SP = -1; // stack pointer 
    public static ushort I = 0; //memory addres store
    public static byte[] Registres = new byte[16];
}
