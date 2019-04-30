//Released under the MIT License.
//
//Copyright (c) 2015 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Text;

namespace Ntreev.Library.Psd
{
    public class PsdReader : IDisposable
    {
        private readonly BinaryReader reader;
        private readonly PsdResolver resolver;
        private readonly Stream stream;
        private readonly Uri uri;

        private Int32 version = 1;

        public PsdReader(Stream stream, PsdResolver resolver, Uri uri)
        {
            this.stream = stream;
            this.reader = new InternalBinaryReader(stream);
            this.resolver = resolver;
            this.uri = uri;
        }

        public void Dispose()
        {
            this.reader.Close();
        }

        public String ReadType()
        {
            return this.ReadAscii(4);
        }

        public String ReadAscii(Int32 length)
        {
            return Encoding.ASCII.GetString(this.reader.ReadBytes(length));
        }

        public Boolean VerifySignature()
        {
            return this.VerifySignature(false);
        }

        public Boolean VerifySignature(Boolean check64bit)
        {
            String signature = this.ReadType();

            if (signature == "8BIM")
                return true;

            if (check64bit == true && signature == "8B64")
                return true;
            
            return false;
        }

        public void ValidateSignature(String signature)
        {
            String s = this.ReadType();
            if(s != signature)
                throw new InvalidFormatException();
        }

        public void ValidateSignature()
        {
            this.ValidateSignature(false);
        }

        public void ValidateSignature(Boolean check64bit)
        {
            if (this.VerifySignature(check64bit) == false)
                throw new InvalidFormatException();
        }

        public void ValidateDocumentSignature()
        {
            String signature = this.ReadType();

            if (signature != "8BPS")
                throw new InvalidFormatException();
        }

        private void ValidateValue<T>(T value, String name, Func<T> readFunc)
        {
            T v = readFunc();
            if (Object.Equals(value, v) == false)
            {
                throw new InvalidFormatException("{0}의 값이 {1}이 아닙니다.", name, value);
            }
        }

        public void ValidateInt16(Int16 value, String name)
        {
            this.ValidateValue<Int16>(value, name, () => this.ReadInt16());
        }

        public void ValidateInt32(Int32 value, String name)
        {
            this.ValidateValue<Int32>(value, name, () => this.ReadInt32());
        }

        public void ValidateType(String value, String name)
        {
            this.ValidateValue<String>(value, name, () => this.ReadType());
        }

        public String ReadPascalString(Int32 modLength)
        {
            Byte count = this.reader.ReadByte();
            String text = String.Empty;
            if (count == 0)
            {
                Stream baseStream = this.reader.BaseStream;
                baseStream.Position += modLength - 1;
                return text;
            }
            Byte[] bytes = this.reader.ReadBytes(count);
            text = Encoding.UTF8.GetString(bytes);
            for (Int32 i = count + 1; (i % modLength) != 0; i++)
            {
                Stream stream2 = this.reader.BaseStream;
                stream2.Position += 1L;
            }
            return text;
        }

        public String ReadString()
        {
            Int32 length = this.ReadInt32();
            if (length == 0)
                return String.Empty;

            Byte[] bytes = this.ReadBytes(length * 2);
            for (Int32 i = 0; i < length; i++)
            {
                Int32 index = i * 2;
                Byte b = bytes[index];
                bytes[index] = bytes[index + 1];
                bytes[index + 1] = b;
            }

            if (bytes[bytes.Length - 1] == 0 && bytes[bytes.Length - 2] == 0)
            {
                length--;
            }

            return Encoding.Unicode.GetString(bytes, 0, length * 2);
        }

        public String ReadKey()
        {
            Int32 length = this.ReadInt32();
            length = (length > 0) ? length : 4;
            return this.ReadAscii(length);
        }

        public Int32 Read(Byte[] buffer, Int32 index, Int32 count)
        {
            return this.reader.Read(buffer, index, count);
        }

        public Byte ReadByte()
        {
            return this.reader.ReadByte();
        }

        public Char ReadChar()
        {
            return (Char)this.ReadByte();
        }

        public Byte[] ReadBytes(Int32 count)
        {
            return this.reader.ReadBytes(count);
        }

        public Boolean ReadBoolean()
        {
            return this.ReverseValue(this.reader.ReadBoolean());
        }

        public Double ReadDouble()
        {
            return this.ReverseValue(this.reader.ReadDouble());
        }

        public Double[] ReadDoubles(Int32 count)
        {
            Double[] values = new Double[count];
            for (Int32 i = 0; i < count; i++)
            {
                values[i] = this.ReadDouble();
            }
            return values;
        }

        public Int16 ReadInt16()
        {
            return this.ReverseValue(this.reader.ReadInt16());
        }

        public Int32 ReadInt32()
        {
            return this.ReverseValue(this.reader.ReadInt32());
        }

        public Int64 ReadInt64()
        {
            return this.ReverseValue(this.reader.ReadInt64());
        }

        public UInt16 ReadUInt16()
        {
            return this.ReverseValue(this.reader.ReadUInt16());
        }

        public UInt32 ReadUInt32()
        {
            return this.ReverseValue(this.reader.ReadUInt32());
        }

        public UInt64 ReadUInt64()
        {
            return this.ReverseValue(this.reader.ReadUInt64());
        }

        public Int64 ReadLength()
        {
            return this.version == 1 ? this.ReadInt32() : this.ReadInt64();
        }

        public void Skip(Int32 count)
        {
            this.ReadBytes(count);
        }

        public void Skip(Char c)
        {
            Char ch = this.ReadChar();
            if (ch != c)
                throw new NotSupportedException();
        }

        public void Skip(Char c, Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                this.Skip(c);
            }
        }

        public ColorMode ReadColorMode()
        {
            return (ColorMode)this.ReadInt16();
        }

        public BlendMode ReadBlendMode()
        {
            return PsdUtility.ToBlendMode(this.ReadAscii(4));
        }

        public LayerFlags ReadLayerFlags()
        {
            return (LayerFlags)this.ReadByte();
        }

        public ChannelType ReadChannelType()
        {
            return (ChannelType)this.ReadInt16();
        }

        public CompressionType ReadCompressionType()
        {
            return (CompressionType)this.ReadInt16();
        }

        public void ReadDocumentHeader()
        {
            this.ValidateDocumentSignature();
            this.Version = this.ReadInt16();
            this.Skip(6);
        }

        public Int64 Position
        {
            get { return this.reader.BaseStream.Position; }
            set { this.reader.BaseStream.Position = value; }
        }

        public Int64 Length
        {
            get { return this.reader.BaseStream.Length; }
        }

        public Int32 Version
        {
            get { return this.version; }
            set 
            {
                if (value != 1 && value != 2)
                    throw new InvalidFormatException();

                this.version = value; 
            }
        }

        public PsdResolver Resolver
        {
            get { return this.resolver; }
        }

        public Stream Stream
        {
            get { return this.stream; }
        }

        public Uri Uri
        {
            get { return this.uri; }
        }

        private Boolean ReverseValue(Boolean value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToBoolean(bytes, 0);
        }

        private Double ReverseValue(Double value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToDouble(bytes, 0);
        }

        private Int16 ReverseValue(Int16 value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        private Int32 ReverseValue(Int32 value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        private Int64 ReverseValue(Int64 value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        private UInt16 ReverseValue(UInt16 value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        private UInt32 ReverseValue(UInt32 value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private UInt64 ReverseValue(UInt64 value)
        {
            Byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        class InternalBinaryReader : BinaryReader
        {
            public InternalBinaryReader(Stream stream)
                : base(stream)
            {

            }
        }
    }
}
