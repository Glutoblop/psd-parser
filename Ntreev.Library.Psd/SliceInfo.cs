﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ntreev.Library.Psd
{
    public sealed class SliceInfo
    {
        private readonly string name;
        private readonly int left;
        private readonly int top;
        private readonly int right;
        private readonly int bottom;

        private readonly string url;
        private readonly string target;
        private readonly string message;
        private readonly string altTag;
        private readonly int horzAlign;
        private readonly int vertAlign;
        private readonly byte alpha;
        private readonly byte red;
        private readonly byte green;
        private readonly byte blue;
        
        internal SliceInfo(PsdReader reader)
        {
            int id = reader.ReadInt32();
            int groupID = reader.ReadInt32();
            int origin = reader.ReadInt32();
            if (origin == 1)
            {
                int asso = reader.ReadInt32();
            }
            this.name = reader.ReadString();
            int type = reader.ReadInt32();

            this.left = reader.ReadInt32();
            this.top = reader.ReadInt32();
            this.right = reader.ReadInt32();
            this.bottom = reader.ReadInt32();

            this.url = reader.ReadString();
            this.target = reader.ReadString();
            this.message = reader.ReadString();
            this.altTag  = reader.ReadString();

            bool b = reader.ReadBoolean();

            string cellText = reader.ReadString();

            this.horzAlign = reader.ReadInt32();
            this.vertAlign = reader.ReadInt32();

            this.alpha = reader.ReadByte();
            this.red = reader.ReadByte();
            this.green = reader.ReadByte();
            this.blue = reader.ReadByte();
        }

        public SliceInfo(IProperties properties)
        {
            if(properties.Contains("Nm") == true)
                this.name = properties["Nm"] as string;

            this.left = (int)properties["bounds.Left"];
            this.top = (int)properties["bounds.Top"];
            this.right = (int)properties["bounds.Rght"];
            this.bottom = (int)properties["bounds.Btom"];

            this.url = properties["url"] as string;
            this.target = properties["null"] as string;
            this.message = properties["Msge"] as string;
            this.altTag = properties["altTag"] as string;
            //this.horzAlign;
            //this.vertAlign;
            //this.alpha;
            //this.red;
            //this.green;
            //this.blue;

            if (properties.Contains("bgColor") == true)
            {
                this.alpha = (byte)(int)properties["bgColor.alpha"];
                this.red = (byte)(int)properties["bgColor.Rd"];
                this.green = (byte)(int)properties["bgColor.Grn"];
                this.blue = (byte)(int)properties["bgColor.Bl"];
            }
        }

        public int Left
        {
            get { return this.left; }
        }

        public int Top
        {
            get { return this.top; }
        }

        public int Right
        {
            get { return this.right; }
        }

        public int Bottom
        {
            get { return this.bottom; }
        }

        public int Width
        {
            get { return this.right - this.left; }
        }

        public int Height
        {
            get { return this.bottom - this.top; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Url
        {
            get { return this.url; }
        }

        public string Target
        {
            get { return this.target; }
        }

        public string Message
        {
            get { return this.message; }
        }

        public string AltTag
        {
            get { return this.altTag; }
        }

        public int HorzAlign
        {
            get { return this.horzAlign; }
        }

        public int VertAlign
        {
            get { return this.vertAlign; }
        }

        public byte Alpha
        {
            get { return this.alpha; }
        }

        public byte Red
        {
            get { return this.red; }
        }

        public byte Green
        {
            get { return this.green; }
        }

        public byte Blue
        {
            get { return this.blue; }
        }
    }
}