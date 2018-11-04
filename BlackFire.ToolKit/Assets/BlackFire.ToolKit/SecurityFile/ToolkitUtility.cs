/*
--------------------------------------------------
| Copyright © 2008 Mr-Alan. All rights reserved. |
| Website: www.0x69h.com                         |
| Mail: mr.alan.china@gmail.com                  |
| QQ: 835988221                                  |
--------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace BlackFire.ToolKit
{
    public static partial class ToolkitUtility
    {
        public static class Security
        {

            #region AES
            
                /// <summary>
                /// AES算法加密字符串。
                /// </summary>
                /// <param name="original">源字符串。</param>
                /// <param name="password">解密密码。(32 | 16)</param>
                /// <returns>加密字符串。</returns>
                public static string AES_Encrypt(string original,string password)
                {
                    if (string.IsNullOrEmpty(original)) return null;
                    Byte[] toEncryptArray = Encoding.UTF8.GetBytes(original);

                    RijndaelManaged rm = new RijndaelManaged
                    {
                        Key = Encoding.UTF8.GetBytes(password),
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    };

                    ICryptoTransform cTransform = rm.CreateEncryptor();
                    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
    
                /// <summary>
                ///  AES算法解密字符串。
                /// </summary>
                /// <param name="cipherText">加密字符串。</param>
                /// <param name="password">解密密码。(32 | 16)</param>
                /// <returns>解密后的明文字符串。</returns>
                public static string AES_Decrypt(string cipherText,string password)
                {
                    if (string.IsNullOrEmpty(cipherText)) return null;
                    Byte[] toEncryptArray = Convert.FromBase64String(cipherText);

                    RijndaelManaged rm = new RijndaelManaged
                    {
                        Key = Encoding.UTF8.GetBytes(password),
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    };

                    ICryptoTransform cTransform = rm.CreateDecryptor();
                    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    
                    return Encoding.UTF8.GetString(resultArray);
                }

            #endregion

        }

        public static class Device
        {
            /// <summary>
            /// 获取Mac地址列表。
            /// </summary>
            /// <returns>Mac地址列表。</returns>
            public static string[] AcquireMacList()
            {
                Boo.Lang.List<string> macList = new Boo.Lang.List<string>();
                NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
                foreach(NetworkInterface ni in nis)
                {
                    var mac = ni.GetPhysicalAddress().ToString();
                    if (!string.IsNullOrEmpty(mac))
                    {
                        macList.Add(mac.ToUpper());
                    }
                }
                return macList.ToArray();
            }


            /// <summary>
            /// 是否包含目标Mac地址。
            /// </summary>
            /// <param name="mac">目标Mac地址。</param>
            /// <returns>是否包含。</returns>
            public static bool HasMac(string mac)
            {
                var macList = AcquireMacList();
                return _HasMac(macList,mac);
            }


            private static bool _HasMac(string[] macList,string mac)
            {
                for (int i = 0; i < macList.Length; i++)
                {
                    if (mac.ToUpper()==macList[i])
                    {
                        return true;
                    }
                }
                return false;
            }


            /// <summary>
            /// 是否包含目标Mac地址。
            /// </summary>
            /// <param name="macs">目标Mac地址枚举。</param>
            /// <returns>是否包含。</returns>
            public static bool HasMac(IEnumerable<string> macs)
            {
                var macList = AcquireMacList();
                foreach (var item in macs)
                {
                    if (_HasMac(macList,item))
                    {
                        return true;
                    }
                }
                return false;
            }

        }
    }
}