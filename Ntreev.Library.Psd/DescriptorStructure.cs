﻿//Released under the MIT License.
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

using Ntreev.Library.Psd.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Ntreev.Library.Psd
{
    class DescriptorStructure : Properties
    {
        private readonly Int32 version;

        public DescriptorStructure(PsdReader reader)
            : this(reader, true)
        {

        }

        public DescriptorStructure(PsdReader reader, Boolean hasVersion)
        {
            if (hasVersion == true)
            {
                this.version = reader.ReadInt32();
            }

            this.Add("Name", reader.ReadString());
            this.Add("ClassID", reader.ReadKey());

            Int32 count = reader.ReadInt32();
            for (Int32 i = 0; i < count; i++)
            {
                String key = reader.ReadKey();
                String osType = reader.ReadType();
                if (key == "EngineData")
                {
                    this.Add(key.Trim(), new StructureEngineData(reader));
                }
                else
                {
                    Object value = StructureReader.Read(osType, reader);
                    this.Add(key.Trim(), value);
                }
            }
        }
    }
}
