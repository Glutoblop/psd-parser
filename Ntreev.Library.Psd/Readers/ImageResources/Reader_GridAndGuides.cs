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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ntreev.Library.Psd.Readers.ImageResources
{
    [ResourceID("1032", DisplayName = "GridAndGuides")]
    class Reader_GridAndGuides : ResourceReaderBase
    {
        public Reader_GridAndGuides(PsdReader reader, Int64 length)
            : base(reader, length)
        {

        }

        protected override void ReadValue(PsdReader reader, Object userData, out IProperties value)
        {
            Properties props = new Properties();

            Int32 version = reader.ReadInt32();

            if (version != 1)
                throw new InvalidFormatException();

            props["HorizontalGrid"] = reader.ReadInt32();
            props["VerticalGrid"] = reader.ReadInt32();

            Int32 guideCount = reader.ReadInt32();

            List<Int32> hg = new List<Int32>();
            List<Int32> vg = new List<Int32>();

            for (Int32 i = 0; i < guideCount; i++)
            {
                Int32 n = reader.ReadInt32();
                Byte t = reader.ReadByte();

                if (t == 0)
                    vg.Add(n);
                else
                    hg.Add(n);
            }

            props["HorizontalGuides"] = hg.ToArray();
            props["VerticalGuides"] = vg.ToArray();

            value = props;
        }
    }
}
