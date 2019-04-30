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
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Globalization;


namespace Ntreev.Library.PsdViewer.Converters
{
    /// <summary>
    /// Convert Level to left margin
    /// Pass a prarameter if you want a unit length other than 19.0.
    /// </summary>
    public class LevelToIndentConverter : IValueConverter
    {
        public Object Convert(Object o, Type type, Object parameter, CultureInfo culture)
        {
            return new Thickness((Int32)o * c_IndentSize, 0, 0, 0);
        }

        public Object ConvertBack(Object o, Type type, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private const Double c_IndentSize = 19.0;
    }
}
