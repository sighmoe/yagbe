using Util;

public class Z80
{
    private Z80State z80s;
    public Z80()
    {
        z80s = new Z80State
        {
        };
    }

    public ushort Fetch()
    {
    }

    public DecodeState Decode(ushort instr)
    {
        return new DecodeState
        {
        };
    }

    public void Execute(DecodeState ds)
    {
        Opcodes.OpTable[ds.Op](ds,z80s); 
    }

    public void Cycle()
    {
        // Console.WriteLine("Z80 CPU State:\n {0}", z80s);
        var instr = Fetch();
        var ds = Decode(instr);
        Console.WriteLine("PC: {0:X2}\ninstr: {1:X2}\n", s8s.Pc-2, instr);
        Execute(ds);
        s8s.Cycles += 1;
    }
        
    public void LoadRom(byte[] rom)
    {
        for (var i = 0; i < rom.Length; i++)
        {
            s8s.Ram[0x200 + i] = rom[i];
        }
    }
}