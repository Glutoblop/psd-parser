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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ntreev.Library.Psd
{
    class Properties : IProperties
    {
        private readonly Dictionary<String, Object> props;

        public Properties()
        {
            this.props = new Dictionary<String, Object>();
        }

        public Properties(Int32 capacity)
        {
            this.props = new Dictionary<String, Object>(capacity);
        }

        public void Add(String key, Object value)
        {
            this.props.Add(key, value);
        }

        public Boolean Contains(String property)
        {
            String[] ss = property.Split(new Char[] { '.', '[', ']', }, StringSplitOptions.RemoveEmptyEntries);

            Object value = this.props;

            foreach (var item in ss)
            {
                if (value is ArrayList == true)
                {
                    ArrayList arrayList = value as ArrayList;
                    Int32 index;
                    if (Int32.TryParse(item, out index) == false)
                        return false;
                    if (index >= arrayList.Count)
                        return false;
                    value = arrayList[index];
                }
                else if (value is IDictionary<String, Object> == true)
                {
                    IDictionary<String, Object> props = value as IDictionary<String, Object>;
                    if (props.ContainsKey(item) == false)
                    {
                        return false;
                    }

                    value = props[item];
                }

            }
            return true; 
        }

        private Object GetProperty(String property)
        {
            String[] ss = property.Split(new Char[] { '.', '[', ']', }, StringSplitOptions.RemoveEmptyEntries);

            Object value = this.props;

            foreach (var item in ss)
            {
                if (value is ArrayList == true)
                {
                    ArrayList arrayList = value as ArrayList;
                    value = arrayList[Int32.Parse(item)];
                }
                else if (value is IDictionary<String, Object> == true)
                {
                    IDictionary<String, Object> props = value as IDictionary<String, Object>;
                    value = props[item];
                }
                else if (value is IProperties == true)
                {
                    IProperties props = value as IProperties;
                    value = props[item];
                }
            }
            return value;
        }

        public Int32 Count
        {
            get { return this.props.Count; }
        }

        public Object this[String property]
        {
            get
            {
                return this.GetProperty(property);
            }
            set
            {
                this.props[property] = value;
            }
        }

        #region IProperties

        IEnumerator<KeyValuePair<String, Object>> IEnumerable<KeyValuePair<String, Object>>.GetEnumerator()
        {
            return this.props.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.props.GetEnumerator();
        }

        #endregion
    }
}
