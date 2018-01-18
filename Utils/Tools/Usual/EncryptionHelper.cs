using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FancyFix.Tools.Usual
{
    public class EncryptionHelper
    {
        private static Des3 des3 = new Des3();
        public static string Des3_Encrypt(string str)
        {
            return des3.EncryptString(str);
        }

        public static string Des3_Decrypt(string str)
        {
            return des3.DecryptString(str);
        }


        #region 3DES加密类
        /// <summary>
        /// 加解密类
        /// </summary>
        class Des3
        {
            //密钥
            private const string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM4";
            //向量，必须是12个字符
            private const string sIV = "jsafojxliqd=";

            //构造一个对称算法
            private SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();

            #region 加密解密函数

            /// <summary>
            /// 字符串的加密
            /// </summary>
            /// <param name="Value">要加密的字符串</param>
            /// <returns>加密后的字符串</returns>
            public string EncryptString(string Value)
            {
                try
                {
                    ICryptoTransform ct;
                    MemoryStream ms;
                    CryptoStream cs;
                    byte[] byt;
                    mCSP.Key = Convert.FromBase64String(sKey);
                    mCSP.IV = Convert.FromBase64String(sIV);
                    //指定加密的运算模式
                    mCSP.Mode = System.Security.Cryptography.CipherMode.CBC;
                    //获取或设置加密算法的填充模式
                    mCSP.Padding = System.Security.Cryptography.PaddingMode.Zeros;
                    ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);//创建加密对象
                    byt = Encoding.UTF8.GetBytes(Value);
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                    cs.Write(byt, 0, byt.Length);
                    cs.FlushFinalBlock();
                    cs.Close();

                    return Convert.ToBase64String(ms.ToArray());
                }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
                catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// 解密字符串
            /// </summary>
            /// <param name="Value">加密后的字符串</param>
            /// <returns>解密后的字符串</returns>
            public string DecryptString(string Value)
            {
                try
                {
                    ICryptoTransform ct;//加密转换运算
                    MemoryStream ms;//内存流
                    CryptoStream cs;//数据流连接到数据加密转换的流
                    byte[] byt;
                    //将3DES的密钥转换成byte
                    mCSP.Key = Convert.FromBase64String(sKey);
                    //将3DES的向量转换成byte
                    mCSP.IV = Convert.FromBase64String(sIV);
                    mCSP.Mode = System.Security.Cryptography.CipherMode.CBC;
                    mCSP.Padding = System.Security.Cryptography.PaddingMode.Zeros;
                    ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);//创建对称解密对象
                    byt = Convert.FromBase64String(Value);
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                    cs.Write(byt, 0, byt.Length);
                    cs.FlushFinalBlock();
                    cs.Close();

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
                catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
                {
                    return string.Empty;
                }
            }

            #endregion
        }
        #endregion
    }
}
