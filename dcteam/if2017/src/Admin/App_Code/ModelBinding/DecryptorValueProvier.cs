using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.ModelBinding
{
    public class DecryptorValueProvier : BindingSourceValueProvider, IEnumerableValueProvider
    {
        private readonly CultureInfo _culture;
        private PrefixContainer _prefixContainer;
        private IDictionary<string, string> _values;

        public DecryptorValueProvier(BindingSource bindingSource, IDictionary<string, string> values, CultureInfo culture) : base(bindingSource)
        {
            if (bindingSource == null)
            {
                throw new ArgumentNullException(nameof(bindingSource));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            _values = values;
            _culture = culture;
        }

        public CultureInfo Culture
        {
            get
            {
                return _culture;
            }
        }

        protected PrefixContainer PrefixContainer
        {
            get
            {
                if (_prefixContainer == null)
                {
                    _prefixContainer = new PrefixContainer(_values.Keys);
                }
                return _prefixContainer;
            }
        }

        public override bool ContainsPrefix(string prefix)
        {
            return PrefixContainer.ContainsPrefix(prefix);
        }

        public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            return PrefixContainer.GetKeysFromPrefix(prefix);
        }

        public override ValueProviderResult GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (_values.ContainsKey(key))
            {
                string value = _values[key];
                if (!string.IsNullOrEmpty(value))
                {
                    return new ValueProviderResult(_values[key], _culture);
                }
            }
            return ValueProviderResult.None;
        }
    }
}
