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

namespace Ntreev.Library.Psd
{
    public static class Extensions
    {
        public static Byte[] MergeChannels(this IImageSource imageSource)
        {
            IChannel[] channels = imageSource.Channels;

            Int32 length = channels.Length;
            Int32 num2 = channels[0].Data.Length;

            Byte[] buffer = new Byte[(imageSource.Width * imageSource.Height) * length];
            Int32 num3 = 0;
            for (Int32 i = 0; i < num2; i++)
            {
                for (Int32 j = channels.Length - 1; j >= 0; j--)
                {
                    buffer[num3++] = channels[j].Data[i];
                }
            }
            return buffer;
        }

        public static IEnumerable<IPsdLayer> Descendants(this IPsdLayer layer)
        {
            return Descendants(layer, item => true);
        }

        public static IEnumerable<IPsdLayer> Descendants(this IPsdLayer layer, Func<IPsdLayer, Boolean> filter)
        {
            foreach (var item in layer.Childs)
            {
                if (filter(item) == false)
                    continue;

                yield return item;

                foreach (var child in item.Descendants(filter))
                {
                    yield return child;
                }
            }
        }

        internal static IEnumerable<PsdLayer> Descendants(this PsdLayer layer)
        {
            yield return layer;

            foreach (var item in layer.Childs)
            {
                foreach (var child in item.Descendants())
                {
                    yield return child;
                }
            }
        }
    }
}
