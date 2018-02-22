using dywebsdk.Web;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using dywebsdk.Extension;
using dywebsdk.Cryptography;
using System.Security.Cryptography;
using IF2017.Admin.Models;
using IF2017.Admin.Configs;
using Newtonsoft.Json;

namespace IF2017.Admin.ModelBinding
{
    public class DecryptorValueProviderFactory: IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.ActionContext.HttpContext.Request;
            WebParams webParams = new WebParams(request);
            string encryptKey = "__encryptdata";

            string encryptData = request.Query.ContainsKey(encryptKey) ? request.Query[encryptKey].ToString() : string.Empty;
            if (encryptData.IsNullOrEmpty() && request.HasFormContentType)
            {
                encryptData = request.Form[encryptKey];
            }

            bool isEncryptDatas = webParams.ContainsKey(encryptKey);
            bool isDecryptSucceed = false;
            Exception decryptException = null;
            IDictionary<string, string> dicDecryptDatas = new Dictionary<string, string>();

            if (encryptData.IsNotNullOrEmpty())
            {
                try
                {
                    //生成密钥
                    //string rsaKey = RSAEncrypt.GenerateKey();
                    string rsaKey = IFConfigReader.RSAPrivateKey;
                    //通过密钥创建对象
                    RSAEncrypt privateRSA = new RSAEncrypt(rsaKey);
                    //解密
                    string decryptData = privateRSA.Decrypt(encryptData);

                    //导出公钥
                    //string publicKey = privateRSA.ExportParameters(false);
                    //通过公钥加密
                    //RSAEncrypt publicRSA = new RSAEncrypt(publicKey);

                    foreach (var item in decryptData.Split('&'))
                    {
                        string[] values = item.Split('=');
                        dicDecryptDatas.Add(values[0], values[1]);
                    }
                    isDecryptSucceed = true;
                }
                catch (Exception ex)
                {
                    decryptException = ex;
                    isDecryptSucceed = false;
                }
                if (dicDecryptDatas.Count() > 0)
                {
                    AddResultsToHttpContext(context, isEncryptDatas, isDecryptSucceed, decryptException, dicDecryptDatas);
                    return AddValueProviderAsync(context, dicDecryptDatas);
                }
            }

            AddResultsToHttpContext(context, isEncryptDatas, isDecryptSucceed, decryptException, dicDecryptDatas);
            return TaskCache.CompletedTask;
        }

        private static void AddResultsToHttpContext(ValueProviderFactoryContext context, bool isEncryptDatas, bool isDecryptSucceed, Exception decryptException, IDictionary<string, string> dicDecryptDatas)
        {
            RequestDataModel reqData = new RequestDataModel();
            reqData.IsEncryptDatas = isEncryptDatas;
            reqData.IsDecryptSucceed = isDecryptSucceed;
            reqData.DicDecryptDatas = dicDecryptDatas;
            reqData.DecryptException = decryptException;

            context.ActionContext.HttpContext.Items.Add(RequestDataModel.RequestDataKey, reqData);
        }

        private static async Task AddValueProviderAsync(ValueProviderFactoryContext context, IDictionary<string, string> dicValues)
        {
            var request = context.ActionContext.HttpContext.Request;
            var valueProvider = new DecryptorValueProvier( BindingSource.ModelBinding, dicValues, CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);
        }
    }
}
