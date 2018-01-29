using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LFVPack
{
    public class Part: List<Part>
    {
        public byte[] Data;
        public PartType Type;

        public Part(PartType type)
        {
            this.Type = type;
            Data = new byte[] { };
        }

        public Part(PartType type, byte[] Data)
        {
            this.Type = type;
            this.Data = Data;
        }

        public Part(BinaryReader reader)
        {
            this.Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            this.Type = (PartType)reader.ReadByte();
            int length = reader.ReadInt32();
            Data = reader.ReadBytes(length);
            length = reader.ReadInt32();
            for (int i = 0; i < length; i++)
            {
                this.Add(new Part(reader));
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.Type);
            writer.Write(this.Data.Length);
            writer.Write(this.Data);
            writer.Write(this.Count);
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Write(writer);
            }
        }

        public void ToFile(string strFileName)
        {
            Stream stw = File.Open(strFileName, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stw);            
            this.Write(writer);
            writer.Close();
            stw.Close();
        }

        public static Part FromFile(string strFileName)
        {
            Stream str = File.Open(strFileName, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(str);
            Part part = new Part(reader);
            reader.Close();
            str.Close();
            return part;
        }


        MemoryStream stream = null;
        private byte[] buffer;
        private void FillBuffer(int length)
        {
            buffer = new byte[length];
            stream.Read(buffer, (int)stream.Position, length);          
        }
        int bufferMaxLength = 0x100;

        public void Open()
        {
            this.stream = new MemoryStream(this.Data);
            buffer = new byte[bufferMaxLength];
        }

        public void Close()
        {
            this.stream.Close();
            this.stream.Dispose();
            this.stream = null;            
            buffer = null;
        }
        
        public int ReadInt()
        {
            this.FillBuffer(4);
            return (int)(((this.buffer[0] | (this.buffer[1] << 0x8)) | (this.buffer[2] << 0x10)) | (this.buffer[3] << 0x18));
        }

        public short ReadShort()
        {
            this.FillBuffer(2);
            return (short)(this.buffer[0] | (this.buffer[1] << 8));
        }

        public string ReadString()
        {
            int length = this.ReadInt();
            char[] aux = new char[length];
            for (int i = 0; i < length; i++)
                aux[i] = (char)this.stream.ReadByte();

            return new string(aux);
        }

        public char ReadChar()
        {
            return (char)this.stream.ReadByte();
        }

        public byte ReadByte()
        {
            return (byte)this.stream.ReadByte();
        }

        public void WriteInt(int value)
        {
            this.buffer[0] = (byte)value;
            this.buffer[1] = (byte)(value >> 8);
            this.buffer[2] = (byte)(value >> 0x10);
            this.buffer[3] = (byte)(value >> 0x18);
            this.stream.Write(this.buffer, 0, 4);            
        }

        public void WriteShort(short value)
        {
            this.buffer[0] = (byte)value;
            this.buffer[1] = (byte)(value >> 8);
            this.stream.Write(this.buffer, 0, 2); 
        }

        public void WriteString(string value)
        {
            this.WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
                this.stream.WriteByte((byte)value[i]);
        }

        public void WriteChar(char value)
        {
            stream.WriteByte((byte)value);
        }

        public void WriteByte(byte value)
        {
            stream.WriteByte(value);
        }
    }

    public enum PartType: byte
    {
        Image = 0x0,
        Map = 0x1,
        Object = 0x2
    }
}
