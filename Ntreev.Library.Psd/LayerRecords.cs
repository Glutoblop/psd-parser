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
using System.Linq;

namespace Ntreev.Library.Psd
{
    public class LayerRecords
    {
        private Channel[] channels;

        private LayerMask layerMask;
        private LayerBlendingRanges blendingRanges;
        private IProperties resources;
        private String name;
        private SectionType sectionType;
        private Guid placedID;
        private Int32 version;

        public void SetExtraRecords(LayerMask layerMask, LayerBlendingRanges blendingRanges, IProperties resources, String name)
        {
            this.layerMask = layerMask;
            this.blendingRanges = blendingRanges;
            this.resources = resources;
            this.name = name;

            this.resources.TryGetValue<String>(ref this.name, "luni.Name");
            this.resources.TryGetValue<Int32>(ref this.version, "lyvr.Version");
            if (this.resources.Contains("lsct.SectionType") == true)
                this.sectionType = (SectionType)this.resources.ToInt32("lsct.SectionType");
            if (this.resources.Contains("lsdk.SectionType") == true)
                this.sectionType = (SectionType)this.resources.ToInt32("lsdk.SectionType");

            if (this.resources.Contains("SoLd.Idnt") == true)
                this.placedID = this.resources.ToGuid("SoLd.Idnt");
            else if (this.resources.Contains("SoLE.Idnt") == true)
                this.placedID = this.resources.ToGuid("SoLE.Idnt");

            foreach (var item in this.channels)
            {
                switch (item.Type)
                {
                    case ChannelType.Mask:
                        {
                            if (this.layerMask != null)
                            {
                                item.Width = this.layerMask.Width;
                                item.Height = this.layerMask.Height;
                            }
                        }
                        break;
                    case ChannelType.Alpha:
                        {
                            if (this.resources.Contains("iOpa") == true)
                            {
                                Byte opa = this.resources.ToByte("iOpa", "Opacity");
                                item.Opacity = opa / 255.0f;
                            }
                        }
                        break;
                }
            }
        }

        public void ValidateSize()
        {
            Int32 width = this.Right - Left;
            Int32 height = this.Bottom - this.Top;

            if ((width > 0x3000) || (height > 0x3000))
            {
                throw new NotSupportedException(String.Format("Invalidated size ({0}, {1})", width, height));
            }
        }

        public Int32 Left { get; set; }

        public Int32 Top { get; set; }

        public Int32 Right { get; set; }

        public Int32 Bottom { get; set; }

        public Int32 Width
        {
            get { return this.Right - this.Left; }
        }

        public Int32 Height
        {
            get { return this.Bottom - this.Top; }
        }

        public Int32 ChannelCount
        {
            get
            {
                if (this.channels == null)
                    return 0;
                return this.channels.Length;
            }
            set
            {
                if (value > 0x38)
                {
                    throw new Exception(String.Format("Too many channels : {0}", value));
                }

                this.channels = new Channel[value];
                for (Int32 i = 0; i < value; i++)
                {
                    this.channels[i] = new Channel();
                }
            }
        }

        public Channel[] Channels
        {
            get { return this.channels; }
        }

        public BlendMode BlendMode { get; set; }

        public Byte Opacity { get; set; }

        public Boolean Clipping { get; set; }

        public LayerFlags Flags { get; set; }

        public Int32 Filter { get; set; }

        public Int64 ChannelSize
        {
            get { return this.channels.Select(item => item.Size).Aggregate((v, n) => v + n); }
        }

        public SectionType SectionType
        {
            get { return this.sectionType; }
        }

        public Guid PlacedID
        {
            get { return this.placedID; }
        }

        public String Name
        {
            get { return this.name; }
        }

        public LayerMask Mask
        {
            get { return this.layerMask; }
        }

        public Object BlendingRanges
        {
            get { return this.blendingRanges; }
        }

        public IProperties Resources
        {
            get { return this.resources; }
        }

        public Int32 Version
        {
            get { return this.version; }
        }
    }
}
