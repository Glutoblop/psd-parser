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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ntreev.Library.Psd
{
    class RangeStream : Stream
    {
        private readonly Stream stream;
        private readonly Int64 position;
        private readonly Int64 length;

        public RangeStream(Stream stream, Int64 position, Int64 length)
        {
            this.stream = stream;
            this.position = position;
            this.length = length;
        }

        public override Boolean CanRead
        {
            get { return true; }
        }

        public override Boolean CanSeek
        {
            get { return true; }
        }

        public override Boolean CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            //this.stream.Flush();
        }

        public override Int64 Length
        {
            get { return this.length; }
        }

        public override Int64 Position
        {
            get
            {
                return this.stream.Position - this.position;
            }
            set
            {
                this.stream.Position = this.position + value;
            }
        }

        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            return this.stream.Read(buffer, offset, count);
        }

        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Current)
                return this.stream.Seek(offset, origin) - this.position;

            return this.stream.Seek(this.position + offset, origin) - this.position;
        }

        public override void SetLength(Int64 value)
        {
            throw new NotImplementedException();
        }

        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            throw new NotImplementedException();
        }
    }
}
