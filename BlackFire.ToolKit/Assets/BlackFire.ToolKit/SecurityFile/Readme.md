# Security File

面向的业务是加密配置文件，这里采用非对称加密的AES算法和Base64算法进行加密。

# 如何使用

## 编辑器中如何使用

找到Unity Editor菜单栏中的Window/BlackFire ToolKit/Security File。
1.点击弹出面板，输入你要加密的配置文件信息。
2.点击Path按钮选取配置文件的所在路径。
3.输入32位密码后，点击Encrypt按钮即可。

## 代码中如何使用

```csharp

var original = "源文件内容。";
var password = "32位密钥";

var cipherText = SecurityFile.Encrypt(original,password); //获取加密文件内容。

var text = SecurityFile.Decrypt(cipherText,password); //获取解密文件内容。

```