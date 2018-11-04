/*
--------------------------------------------------
| Copyright © 2008 Mr-Alan. All rights reserved. |
| Website: www.0x69h.com                         |
| Mail: mr.alan.china@gmail.com                  |
| QQ: 835988221                                  |
--------------------------------------------------
*/

namespace BlackFire.ToolKit
{
    /// <summary>
    /// 安全文件类。
    /// </summary>
    public static class SecurityFile
    {
        /// <summary>
        /// 加密文件。
        /// </summary>
        /// <param name="original">源文件内容。</param>
        /// <param name="password">密钥。</param>
        /// <returns>加密后的文件内容。</returns>
        public static string Encrypt(string original, string password)
        {
            return ToolkitUtility.Security.AES_Encrypt(original,password);
        }

        /// <summary>
        /// 解密文件。
        /// </summary>
        /// <param name="cipherText">加密后的文件内容。</param>
        /// <param name="password">密钥。</param>
        /// <returns>加密后的文件内容。</returns>
        public static string Decrypt(string cipherText, string password)
        {
            return ToolkitUtility.Security.AES_Decrypt(cipherText,password);
        }
    }
}