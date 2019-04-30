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

namespace Ntreev.Library.Psd.Readers.ImageResources
{
    [ResourceID("1050", DisplayName = "Slices")]
    class Reader_SlicesInfo : ResourceReaderBase
    {
        public Reader_SlicesInfo(PsdReader reader, Int64 length)
            : base(reader, length)
        {

        }

        protected override void ReadValue(PsdReader reader, Object userData, out IProperties value)
        {
            Properties props = new Properties();

            Int32 version = reader.ReadInt32();
            if (version == 6)
            {
                var r1 = reader.ReadInt32();
                var r2 = reader.ReadInt32();
                var r3 = reader.ReadInt32();
                var r4 = reader.ReadInt32();
                String text = reader.ReadString();
                var count = reader.ReadInt32();

                List<IProperties> slices = new List<IProperties>(count);
                for (Int32 i = 0; i < count; i++)
                {
                    slices.Add(ReadSliceInfo(reader));
                }
            }
            {
                var descriptor = new DescriptorStructure(reader) as IProperties;

                var items = descriptor["slices.Items[0]"] as Object[];
                List<IProperties> slices = new List<IProperties>(items.Length);
                foreach (var item in items)
                {
                    slices.Add(ReadSliceInfo(item as IProperties));
                }
                props["Items"] = slices.ToArray();
            }

            value = props;
        }

        private static Properties ReadSliceInfo(PsdReader reader)
        {
            Properties props = new Properties();
            props["ID"] = reader.ReadInt32();
            props["GroupID"] = reader.ReadInt32();
            Int32 origin = reader.ReadInt32();
            if (origin == 1)
            {
                Int32 asso = reader.ReadInt32();
            }
            props["Name"] = reader.ReadString();
            Int32 type = reader.ReadInt32();

            props["Left"] = reader.ReadInt32();
            props["Top"] = reader.ReadInt32();
            props["Right"] = reader.ReadInt32();
            props["Bottom"] = reader.ReadInt32();

            props["Url"] = reader.ReadString();
            props["Target"] = reader.ReadString();
            props["Message"] = reader.ReadString();
            props["AltTag"] = reader.ReadString();

            Boolean b = reader.ReadBoolean();

            String cellText = reader.ReadString();

            props["HorzAlign"] = reader.ReadInt32();
            props["VertAlign"] = reader.ReadInt32();

            props["Alpha"] = reader.ReadByte();
            props["Red"] = reader.ReadByte();
            props["Green"] = reader.ReadByte();
            props["Blue"] = reader.ReadByte();

            return props;
        }

        private static Properties ReadSliceInfo(IProperties properties)
        {
            Properties props = new Properties();
            props["ID"] = (Int32)properties["sliceID"];
            props["GroupID"] = (Int32)properties["groupID"];
            if (properties.Contains("Nm") == true)
                props["Name"] = properties["Nm"] as String;

            props["Left"] = (Int32)properties["bounds.Left"];
            props["Top"] = (Int32)properties["bounds.Top"];
            props["Right"] = (Int32)properties["bounds.Rght"];
            props["Bottom"] = (Int32)properties["bounds.Btom"];

            props["Url"] = properties["url"] as String;
            props["Target"] = properties["null"] as String;
            props["Message"] = properties["Msge"] as String;
            props["AltTag"] = properties["altTag"] as String;

            if (properties.Contains("bgColor") == true)
            {
                props["Alpha"] = (Byte)(Int32)properties["bgColor.alpha"];
                props["Red"] = (Byte)(Int32)properties["bgColor.Rd"];
                props["Green"] = (Byte)(Int32)properties["bgColor.Grn"];
                props["Blue"] = (Byte)(Int32)properties["bgColor.Bl"];
            }

            return props;
        }
    }
}
