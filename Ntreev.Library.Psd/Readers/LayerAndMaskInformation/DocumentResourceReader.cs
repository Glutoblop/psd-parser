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

namespace Ntreev.Library.Psd.Readers.LayerAndMaskInformation
{
    class DocumentResourceReader : LazyProperties
    {
        private static String[] doubleTypeKeys = { "LMsk", "Lr16", "Lr32", "Layr", "Mt16", "Mt32", "Mtrn", "Alph", "FMsk", "lnk2", "FEid", "FXid", "PxSD", "lnkE", "extd", };

        public DocumentResourceReader(PsdReader reader, Int64 length)
            : base(reader, length, null)
        {

        }

        protected override void ReadValue(PsdReader reader, Object userData, out IProperties value)
        {
            Properties props = new Properties();

            while (reader.Position < this.EndPosition)
            {
                reader.ValidateSignature(true);
                String resourceID = reader.ReadType();
                Int64 length = this.ReadLength(reader, resourceID);

                ResourceReaderBase resourceReader = ReaderCollector.CreateReader(resourceID, reader, length);
                String resourceName = ReaderCollector.GetDisplayName(resourceID);

                props[resourceName] = resourceReader;
            }

            value = props;
        }

        private Int64 ReadLength(PsdReader reader, String resourceID)
        {
            Int64 length = 0;
            if (doubleTypeKeys.Contains(resourceID) && reader.Version == 2)
            {
                length = reader.ReadInt64();
            }
            else if(reader.Version == 2)
            {
                length = reader.ReadInt64();
            }
            else
            {
                length = reader.ReadInt32();
            }

            return (length + 3) & (~3);
        }
    }
}