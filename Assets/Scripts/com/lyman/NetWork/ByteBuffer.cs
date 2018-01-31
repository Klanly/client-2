using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
public class ByteBuffer
{
    
    MemoryStream stream = null;
    BinaryWriter writer = null;
    BinaryReader reader = null;

    public ByteBuffer(byte[] data = null)
    {
        if (data != null)
        {
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
        }
        else
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }
    }

    

    

    //后退
    public void Backward(int value)
    {
        if (stream != null)
        {
            if (stream.Position >= value)
            {
                stream.Position -= value;
            }
            else
            {
                stream.Position = 0;
            }
        }
    }
    //前移
    public void Forward(int value)
    {
        if (stream != null)
        {
            if ((stream.Length - stream.Position - 1) >= value)
            {
                stream.Position += value;
            }
            else
            {
                stream.Position = stream.Length - 1;
            }
        }
    }


    public void OffsetToZero()
    {
        if (stream != null)
            stream.Position = 0;
    }

    

    public void Close()
    {
        if (writer != null)
            writer.Close();
        if (reader != null)
            reader.Close();
        stream.Close();
        writer = null;
        reader = null;
        stream = null;
    }
    public void WriteSByte(sbyte v)
    {
        writer.Write((sbyte)v);
    }
    public void WriteByte(byte v)
    {
        writer.Write((byte)v);
    }
    
    public void WriteInt(int v)
    {
        writer.Write((int)v);
    }
    public void WriteUInt(uint v)
    {
        writer.Write((uint)v);
    }

    public void WriteShort(short v)
    {
        writer.Write((short)v);
    }

    public void WriteUShort(ushort v)
    {
        writer.Write((ushort)v);
    }

    public void WriteLong(long v)
    {
        writer.Write((long)v);
    }
    public void WriteULong(ulong v)
    {
        writer.Write((ulong)v);
    }
    public void WriteFloat(float v)
    {
        byte[] temp = BitConverter.GetBytes(v);
        Array.Reverse(temp);
        writer.Write(BitConverter.ToSingle(temp, 0));
    }

    public void WriteDouble(double v)
    {
        byte[] temp = BitConverter.GetBytes(v);
        //Array.Reverse(temp);
        writer.Write(BitConverter.ToDouble(temp, 0));
    }

    public void WriteString(string v)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(v);
        writer.Write(bytes);
    }

    public void WriteBytes(byte[] v)
    {
        writer.Write(v);
    }

    public void WriteBoolean(bool v)
    {
        writer.Write((bool)v);
    }




    //读取 =========================================//
    //length是字节数
    public string ReadString(int length)
    {
        byte[] bytes = ReadBytes(length);
        return Encoding.UTF8.GetString(bytes);
    }
    
    public bool ReadBoolean()
    {
        return reader.ReadBoolean();
    }
    
    //读sbyte
    public sbyte ReadSByte()
    {
        return reader.ReadSByte();
    }
    //读byte
    public byte ReadByte()
    {
        return reader.ReadByte();
    }

    public float ReadInt32()
	{
		int va = reader.ReadInt32();
		//LTLog.LogWarning ("=================int32 value:"+va);
		float fa = float.Parse(va.ToString());
		return va;
    }
    public float ReadUInt32()
    {
		uint va = reader.ReadUInt32();
		//LTLog.LogWarning ("=================int32 value:"+va);
		float fa = float.Parse(va.ToString());
		return va;
    }
        

    public long ReadInt64()
    {
        return reader.ReadInt64();
    }
    public ulong ReadUInt64()
    {
        return reader.ReadUInt64();
    }
    public short ReadShort()
    {
        return reader.ReadInt16();
    }
    public ushort ReadUShort()
    {
        return reader.ReadUInt16();
    }
    public float ReadFloat()
    {
        return reader.ReadSingle();
    }
    public double ReadDouble()
    {
        return reader.ReadDouble();
    }
    public byte[] ReadBytes(int len)
    {
        return reader.ReadBytes(len);
        
    }
        
    public byte[] ToBytes()
    {
        if (writer == null)
        {
            Debug.LogError("write === null");
        }
        writer.Flush();
        return stream.ToArray();
    }

    public int PacketLength
    {
        get { return ToBytes().Length; }
    }

    public void Flush()
    {
        writer.Flush();
    }
}