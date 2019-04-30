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
using System.Linq;
using System.Text;

namespace Ntreev.Library.Psd.Readers.LayerResources
{
    [ResourceID("lrFX")]
    class Reader_lrFX : ResourceReaderBase
    {
        public Reader_lrFX(PsdReader reader, Int64 length)
            : base(reader, length)
        {

        }

        protected override void ReadValue(PsdReader reader, Object userData, out IProperties value)
        {
            value = new Properties();

            Int16 version = reader.ReadInt16();
            Int32 count = reader.ReadInt16();

            for (Int32 i = 0; i < count; i++)
            {
                String _8bim = reader.ReadAscii(4);
                String effectType = reader.ReadAscii(4);
                Int32 size = reader.ReadInt32();
                Int64 p = reader.Position;

                switch (effectType)
                {
                    case "dsdw":
                        {
                            //ShadowInfo.Parse(reader);
                        }
                        break;
                    case "sofi":
                        {
                            //this.solidFillInfo = SolidFillInfo.Parse(reader);
                        }
                        break;
                }

                reader.Position = p + size;
            }
        }
    }
}
