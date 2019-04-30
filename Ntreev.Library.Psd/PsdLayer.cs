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

using Ntreev.Library.Psd.Readers.LayerAndMaskInformation;
using System;
using System.Linq;

namespace Ntreev.Library.Psd
{
    public class PsdLayer : IPsdLayer
    {
        private readonly PsdDocument document;
        private readonly LayerRecords records;

        private Int32 left, top, right, bottom;

        private PsdLayer[] childs;
        private PsdLayer parent;
        private ILinkedLayer linkedLayer;

        private ChannelsReader channels;

        private static PsdLayer[] emptyChilds = new PsdLayer[] { };

        public PsdLayer(PsdReader reader, PsdDocument document)
        {
            this.document = document;
            this.records = LayerRecordsReader.Read(reader);
            this.records = LayerExtraRecordsReader.Read(reader, this.records);

            this.left = this.records.Left;
            this.top = this.records.Top;
            this.right = this.records.Right;
            this.bottom = this.records.Bottom;
        }

        public override String ToString()
        {
            return this.Name;
        }

        public Channel[] Channels
        {
            get { return this.channels.Value; }
        }

        public SectionType SectionType
        {
            get { return this.records.SectionType; }
        }

        public String Name
        {
            get { return this.records.Name; }
        }

        public Boolean IsVisible
        {
            get { return (this.records.Flags & LayerFlags.Visible) != LayerFlags.Visible; }
        }

        public Single Opacity
        {
            get { return (((Single)this.records.Opacity) / 255f); }
        }

        public Int32 Left
        {
            get { return this.left; }
        }

        public Int32 Top
        {
            get { return this.top; }
        }

        public Int32 Right
        {
            get { return this.right; }
        }

        public Int32 Bottom
        {
            get { return this.bottom; }
        }

        public Int32 Width
        {
            get { return this.right - this.left; }
        }

        public Int32 Height
        {
            get { return this.bottom - this.top; }
        }

        public Int32 Depth
        {
            get { return this.document.FileHeaderSection.Depth; }
        }

        public Boolean IsClipping
        {
            get { return this.records.Clipping; }
        }

        public BlendMode BlendMode
        {
            get { return this.records.BlendMode; }
        }

        public PsdLayer Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        public PsdLayer[] Childs
        {
            get
            {
                if (this.childs == null)
                    return emptyChilds;
                return this.childs;
            }
            set { this.childs = value; }
        }

        public IProperties Resources
        {
            get { return this.records.Resources; }
        }

        public PsdDocument Document
        {
            get { return this.document; }
        }

        public LayerRecords Records
        {
            get { return this.records; }
        }

        public ILinkedLayer LinkedLayer
        {
            get
            {
                Guid placeID = this.records.PlacedID;

                if (placeID == Guid.Empty)
                    return null;

                if (this.linkedLayer == null)
                {
                    this.linkedLayer = this.document.LinkedLayers.Where(i => i.ID == placeID && i.HasDocument).FirstOrDefault();
                }
                return this.linkedLayer;
            }
        }

        public Boolean HasImage
        {
            get
            {
                if (this.records.SectionType != SectionType.Normal)
                    return false;
                if (this.Width == 0 || this.Height == 0)
                    return false;
                return true;
            }
        }

        public Boolean HasMask
        {
            get { return this.records.Mask != null; }
        }

        public void ReadChannels(PsdReader reader)
        {
            this.channels = new ChannelsReader(reader, this.records.ChannelSize, this);
        }

        public void ComputeBounds()
        {
            SectionType sectionType = this.records.SectionType;
            if (sectionType != SectionType.Opend && sectionType != SectionType.Closed)
                return;

            Int32 left = Int32.MaxValue;
            Int32 top = Int32.MaxValue;
            Int32 right = Int32.MinValue;
            Int32 bottom = Int32.MinValue;

            Boolean isSet = false;

            foreach (var item in this.Descendants())
            {
                if (item == this || item.HasImage == false)
                    continue;

                // 일반 레이어인데 비어 있을때
                if (item.Resources.Contains("PlLd.Transformation"))
                {
                    Double[] transforms = (Double[])item.Resources["PlLd.Transformation"];
                    Double[] xx = new Double[] { transforms[0], transforms[2], transforms[4], transforms[6], };
                    Double[] yy = new Double[] { transforms[1], transforms[3], transforms[5], transforms[7], };

                    Int32 l = (Int32)Math.Ceiling(xx.Min());
                    Int32 r = (Int32)Math.Ceiling(xx.Max());
                    Int32 t = (Int32)Math.Ceiling(yy.Min());
                    Int32 b = (Int32)Math.Ceiling(yy.Max());
                    left = Math.Min(l, left);
                    top = Math.Min(t, top);
                    right = Math.Max(r, right);
                    bottom = Math.Max(b, bottom);
                }
                else
                {
                    left = Math.Min(item.Left, left);
                    top = Math.Min(item.Top, top);
                    right = Math.Max(item.Right, right);
                    bottom = Math.Max(item.Bottom, bottom);
                }
                isSet = true;
            }

            if (isSet == false)
                return;

            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        #region IPsdLayer

        IPsdLayer IPsdLayer.Parent
        {
            get
            {
                if (this.parent == null)
                    return this.document;
                return this.parent;
            }
        }

        IChannel[] IImageSource.Channels
        {
            get { return this.channels.Value; }
        }

        IPsdLayer[] IPsdLayer.Childs
        {
            get { return this.Childs; }
        }

        #endregion
    }
}

