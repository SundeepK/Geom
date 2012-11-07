using System;

using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace geom
{
    class EndianDataInputStream : BinaryReader
    {

        
	public EndianDataInputStream(Stream stream) : base(stream)
	{

	}


    public String read4ByteString( ) 
	{
		byte[] bytes = new byte[4];
        Read(bytes, 0, bytes.Length);
        String str = System.Text.Encoding.UTF8.GetString( bytes );
       	return str;
	}
	
	public short readShortLittleEndian( )
	{
		int result = ReadByte();
		result |= ReadByte() << 8;
        //Debug.WriteLine(result);
        return (short)result;		
	}
	
	public int readIntLittleEndian( ) 
	{
		int result =ReadByte();
		result |= ReadByte() << 8;
		result |= ReadByte() << 16;
		result |= ReadByte() << 24;
		return result;		
	}

    }
}
