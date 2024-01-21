using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using WDTEdit;

const string fileName = "KalidarAlpha.xxx";
const string fileNameOut = "test.wdt";



using (var stream = File.Open(fileName, FileMode.Open))
{
    var list = new List<UInt32>();
    var list2 = new List<UInt32>();
    var list3 = new List<UInt32>();
    var list4 = new List<byte[]>();
    using (var br = new BinaryReader(stream, Encoding.ASCII, false))
    {   
        // WDT STARTS HERE
        Helper.SeekChunk(br, "MVER");
        UInt32 MVERsize = br.ReadUInt32();
        UInt32 version = br.ReadUInt32(); // 18

        Helper.SeekChunk(br, "MPHD");
        UInt32 MPHDsize = br.ReadUInt32();
        UInt32 nDoodadName = br.ReadUInt32();
        UInt32 offsDoodadNames = br.ReadUInt32();
        UInt32 nMapObjNames = br.ReadUInt32();
        UInt32 offsMapObjNames = br.ReadUInt32();
        byte[] pad = br.ReadBytes(112);
        
        Helper.SeekChunk(br, "MAIN");
        UInt32 offset;
        UInt32 MAINSize;
        UInt32 flags;
        byte[] padding; 
        
        for (int i = 0; i < 4096; i++)
        {
            offset = br.ReadUInt32();
            list.Add(offset);

            MAINSize = br.ReadUInt32();
            list2.Add(MAINSize);

            flags = br.ReadUInt32();
            list3.Add(flags);

            padding = br.ReadBytes(4);
            list4.Add(padding);
            
        }
        br.ReadBytes(4);

        using (var stream2 = File.Open(fileNameOut, FileMode.Create))
        {
            using (var bw = new BinaryWriter(stream2, Encoding.ASCII, false))
            {
                var MVER = Helper.MagicToSignature("MVER");
                bw.Write(MVER);
                bw.Write(MVERsize);
                bw.Write(version);

                var MPHD = Helper.MagicToSignature("MPHD");
                bw.Write(MPHD);
                bw.Write(32);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);
                bw.Write(0);

                var MAIN = Helper.MagicToSignature("MAIN");
                bw.Write(MAIN);
                bw.Write(MAINSize);

            }
        }
        /*Helper.SeekChunk(br, "MDNM");
        var st = br.ReadUInt32();
        int sizeOfStrs = Convert.ToInt32(st);
        var pp = br.ReadBytes(sizeOfStrs);
        var str = Encoding.Default.GetString(pp);
        Console.WriteLine(str);*/
    }
    
}

