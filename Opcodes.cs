using Util;
public class Opcodes
{
    private static Random rng = new Random();

    public static Dictionary<int, Action<DecodeState, Z80State>> OpTable = new Dictionary<int, Action<DecodeState, Z80State>>
    {
        { 0x0, (_1,_2) => { Op0(_1,_2); } },
        { 0x1, (_1,_2) => { Op1(_1,_2); } },
        { 0x2, (_1,_2) => { Op2(_1,_2); } },
        { 0x3, (_1,_2) => { Op3(_1,_2); } },
        { 0x4, (_1,_2) => { Op4(_1,_2); } },
        { 0x5, (_1,_2) => { Op5(_1,_2); } },
        { 0x6, (_1,_2) => { Op6(_1,_2); } },
        { 0x7, (_1,_2) => { Op7(_1,_2); } },
        { 0x8, (_1,_2) => { Op8(_1,_2); } },
        { 0x9, (_1,_2) => { Op9(_1,_2); } },
        { 0xA, (_1,_2) => { OpA(_1,_2); } },
        { 0xB, (_1,_2) => { OpB(_1,_2); } },
        { 0xC, (_1,_2) => { OpC(_1,_2); } },
        { 0xD, (_1,_2) => { OpD(_1,_2); } },
        { 0xE, (_1,_2) => { OpE(_1,_2); } },
        { 0xF, (_1,_2) => { OpF(_1,_2); } },
    };
    
    public static byte Unimplemented(DecodeState ds, Z80State s8s)
    {
        Console.WriteLine("Unimplemented opcode {0:X2}. NNN: {1:X2}", ds.Op, ds.NNN);
        Console.WriteLine("HALTING...");
        for (; ; ) { }
        return 0x00;
    }

    private static void UpdateFlags(DecodeState ds, Z80State s8s)
    {
        var V = s8s.V;
        var X = ds.X;
        var Y = ds.Y;
        s8s.V[0xF] = ds.Op switch
        {
            0x8 => ds.N switch
            {
                0x4 => (byte)(V[X] + V[Y] > 255 ? 0x1 : 0x0),
                0x5 => (byte)(V[X] >= V[Y] ? 0x1 : 0x0),
                0x6 => (byte)((V[X] & 0x1) == 1 ? 0x1 : 0x0),
                0x7 => (byte)(V[Y] >= V[X] ? 0x1 : 0x0),
                0xE => (byte)((V[X] & 0x80) > 0 ? 0x1 : 0x0),
                _ => V[0xF]
            },
            _ => Unimplemented(ds, s8s),
        };
    }
}