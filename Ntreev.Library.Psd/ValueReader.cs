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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Ntreev.Library.Psd
{
    abstract class ValueReader<T>
    {
        private readonly PsdReader reader;
        private readonly Int32 readerVersion;
        private readonly Int64 position;
        private readonly Int64 length;
        private readonly Object userData;
        private T value;
        private Boolean isRead;

        protected ValueReader(PsdReader reader, Boolean hasLength, Object userData)
        {
            if (hasLength == true)
            {
                this.length = this.OnLengthGet(reader);
            }

            this.reader = reader;
            this.readerVersion = reader.Version;
            this.position = reader.Position;
            this.userData = userData;

            if (hasLength == false)
            {
                this.Refresh();
                this.length = reader.Position - this.position;
            }
            else
            {
                //this.Refresh();
            }

            this.reader.Position = this.position + this.length;
        }

        protected ValueReader(PsdReader reader, Int64 length, Object userData)
        {
            if (length < 0)
                throw new InvalidFormatException();
            this.reader = reader;
            this.length = length;
            this.readerVersion = reader.Version;
            this.position = reader.Position;
            this.userData = userData;

            if (this.length == 0)
            {
                this.Refresh();
                this.length = reader.Position - this.position;
            }
            else
            {
                //this.Refresh();
            }

            this.reader.Position = this.position + this.length;
        }

        public void Refresh()
        {
            this.reader.Position = this.position;
            this.reader.Version = this.readerVersion;
            this.ReadValue(this.reader, this.userData, out this.value);
            if (this.length > 0)
                this.reader.Position = this.position + this.length;
            this.isRead = true;
        }

        public T Value
        {
            get
            {
                if (this.isRead == false && this.length > 0)
                {
                    Int64 position = reader.Position;
                    Int32 version = reader.Version;
                    this.Refresh();
                    reader.Position = position;
                    reader.Version = version;
                }
                return this.value;
            }
        }

        public Int64 Length
        {
            get { return this.length; }
        }

        public Int64 Position
        {
            get { return this.position; }
        }

        public Int64 EndPosition
        {
            get { return this.position + this.length; }
        }

        protected virtual Int64 OnLengthGet(PsdReader reader)
        {
            return reader.ReadLength();
        }

        protected abstract void ReadValue(PsdReader reader, Object userData, out T value);
    }
}