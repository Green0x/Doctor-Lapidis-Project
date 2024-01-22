using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using WDTEdit;
using static System.Runtime.InteropServices.JavaScript.JSType;



using (var stream = File.Open("fileName.test", FileMode.Create))
{
    using (var bw = new BinaryWriter(stream, Encoding.ASCII, false))
    {
        Chunk MVER = new Chunk();
        bw.Write(MVER.magic = Helper.MagicToSignature("MVER"));
        bw.Write(MVER.size = 4);
        MVER.data = new List<byte>();
        addChunkData(MVER.size, MVER.data);
        bw.Write(MVER.data.ToArray());
        bw.Seek(8, SeekOrigin.Begin);
        bw.Write(BitConverter.GetBytes(18)); // WDT version 18
        
        Chunk MPHD = new Chunk();
        bw.Write(MPHD.magic = Helper.MagicToSignature("MPHD"));
        bw.Write(MPHD.size = 32);
        MPHD.data = new List<byte>();
        addChunkData(MPHD.size, MPHD.data);
        bw.Write(MPHD.data.ToArray());
        
        Chunk MAIN = new Chunk();
        bw.Write(MAIN.magic = Helper.MagicToSignature("MAIN"));
        bw.Write(MAIN.size = 32768);
        MAIN.data = new List<byte>();
        addChunkData(MAIN.size, MAIN.data);
        bw.Write(MAIN.data.ToArray());
        bw.Seek(60, SeekOrigin.Begin);
        AreaInfo MAINArea = new AreaInfo();
        bw.Write(MAINArea.hasADT = 1);
    }
}

void addChunkData(UInt32 size, List<byte> data)
{
    if (size % sizeof(UInt32) == 0)
    {
        for (int i = 0; i < size; i++)
        {
            data.Add(0x00);
        }
    }
    else
    {
        Console.WriteLine("Size is not UInt32");
    }
    
}

public struct Chunk
{
    public UInt32 magic;
    public UInt32 size;
    public List<byte> data;
}

public struct MapHeader
{
    public UInt32 flags;
    public UInt32 lgtFileDataID;  // Unused in 3.3.5
    public UInt32 occFileDataID;  // Unused in 3.3.5
    public UInt32 fogsFileDataID; // Unused in 3.3.5
    public UInt32 mpvFileDataID;  // Unused in 3.3.5
    public UInt32 texFileDataID;  // Unused in 3.3.5
    public UInt32 wdlFileDataID;  // Unused in 3.3.5
    public UInt32 pd4FileDataID;  // Unused in 3.3.5
};

public struct AreaInfo
{
    public UInt32 hasADT;
    public UInt32 asyncID;
}




