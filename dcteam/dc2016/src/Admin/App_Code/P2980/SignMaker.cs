using DC2016.Admin.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC2016.Admin.P2980
{
    public class SignMaker
    {
        private const string NegotiationValue = "pwolslwset";

        public string MakeSign(Dictionary<string, Object> dicParams)
        {
            if (dicParams == null || dicParams.Count == 0)
            {
                throw new ArgumentNullException(nameof(dicParams));
            }
            string[] keys = dicParams.Keys.ToArray();
            Array.Sort(keys);
            StringBuilder sb = new StringBuilder();
            foreach (var key in keys)
            {
                sb.Append(key);
                sb.Append(dicParams[key]);
            }
            sb.Append(NegotiationValue);

            return UrsHelper.MD5(sb.ToString()).ToLower();
        }
    }
}
