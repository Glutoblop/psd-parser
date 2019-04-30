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
    public static class IPropertiesExtension
    {
        public static Boolean Contains(this IProperties props, String property, params String[] properties)
        {
            return props.Contains(GeneratePropertyName(property, properties));
        }

        public static T ToValue<T>(this IProperties props, String property, params String[] properties)
        {
            return (T)props[GeneratePropertyName(property, properties)];
        }

        public static Guid ToGuid(this IProperties props, String property, params String[] properties)
        {
            return new Guid(props.ToString(property, properties));
        }

        public static String ToString(this IProperties props, String property, params String[] properties)
        {
            return ToValue<String>(props, property, properties);
        }

        public static Byte ToByte(this IProperties props, String property, params String[] properties)
        {
            return ToValue<Byte>(props, property, properties);
        }

        public static Int32 ToInt32(this IProperties props, String property, params String[] properties)
        {
            return ToValue<Int32>(props, property, properties);
        }

        public static Single ToSingle(this IProperties props, String property, params String[] properties)
        {
            return ToValue<Single>(props, property, properties);
        }

        public static Double ToDouble(this IProperties props, String property, params String[] properties)
        {
            return ToValue<Double>(props, property, properties);
        }

        public static Boolean ToBoolean(this IProperties props, String property, params String[] properties)
        {
            return ToValue<Boolean>(props, property, properties);
        }

        public static Boolean TryGetValue<T>(this IProperties props, ref T value, String property, params String[] properties)
        {
            String propertyName = GeneratePropertyName(property, properties);
            if (props.Contains(propertyName) == false)
                return false;
            value = props.ToValue<T>(propertyName);
            return true;
        }

        private static String GeneratePropertyName(String property, params String[] properties)
        {
            if (properties.Length == 0)
                return property;

            return property + "." + String.Join(".", properties);
        }
    }
}
