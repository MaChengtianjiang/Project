using UnityEngine;
using System.IO;
using System.Security.Cryptography;

//------------------------------------------------------------
/// <summary>
/// 暗号化クラス<br/>
/// </summary>
//------------------------------------------------------------
public class Crypt {

	/// <summary>暗号化で使うsaltキーのバイト長(※8以上必須)。</summary>
	private static readonly int ENCRYPT_RIJNDAEL_SALT_LENGTH = 8;

	//------------------------------------------------------------
	/// <summary>
	/// 暗号化
	/// </summary>
	//------------------------------------------------------------
	internal static string Encrypt(string text) {
		RijndaelManaged aes = new RijndaelManaged();
		aes.BlockSize = 128;
		aes.KeySize = 128;
		aes.Padding = PaddingMode.Zeros;
		aes.Mode = CipherMode.CBC;
		byte[] key, iv;
		GenerateKeyFromPassphrase(Crypt.EncryptPassphraseFunc(), Crypt.EncryptSaltFunc(), aes.KeySize, out key, aes.BlockSize, out iv);
		aes.Key = key;
		aes.IV = iv;

		ICryptoTransform encrypt = aes.CreateEncryptor();
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

		byte[] text_bytes = System.Text.Encoding.UTF8.GetBytes(text);

		cryptStream.Write(text_bytes, 0, text_bytes.Length);
		cryptStream.FlushFinalBlock();

		byte[] encrypted = memoryStream.ToArray();

		return ( System.Convert.ToBase64String(encrypted) );
	}

	//------------------------------------------------------------
	/// <summary>
	/// 復号化
	/// </summary>
	//------------------------------------------------------------
	internal static string Decrypt(string cryptText) {
		RijndaelManaged aes = new RijndaelManaged();
		aes.BlockSize = 128;
		aes.KeySize = 128;
		aes.Padding = PaddingMode.Zeros;
		aes.Mode = CipherMode.CBC;
		byte[] key, iv;
		GenerateKeyFromPassphrase(Crypt.EncryptPassphraseFunc(), Crypt.EncryptSaltFunc(), aes.KeySize, out key, aes.BlockSize, out iv);
		aes.Key = key;
		aes.IV = iv;

		ICryptoTransform decryptor = aes.CreateDecryptor();

		byte[] encrypted = System.Convert.FromBase64String(cryptText);
		byte[] planeText = new byte[encrypted.Length];

		MemoryStream memoryStream = new MemoryStream(encrypted);
		CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

		cryptStream.Read(planeText, 0, planeText.Length);

		return ( System.Text.Encoding.UTF8.GetString(planeText) );
	}

	//------------------------------------------------------------
	/// <summary>
	///	暗号化用パスフレーズの取得メソッド。<br/>
	/// 各メソッド使用前にここに必要なキー取得方法を登録してから呼び出す事。<br/>
	/// <see cref="EncryptType.Rijndael"/>を使う際に必須(他では不要)。<br/>
	/// ※クラス内で完結できていないが、これはユーティリティクラスなので、リソース所持は呼び出し側へ任せる。<br/>
	/// ※効果があるか疑問だが少しでも保持時間を減らす為に取得方法のみ保持する方式にしてみた。<br/>
	/// </summary>
	//------------------------------------------------------------
	internal static System.Func<string> EncryptPassphraseFunc {
		private get;
		set;
	}

	//------------------------------------------------------------
	/// <summary>
	///	基になるSaltキーの取得メソッド。<br/>
	/// 各メソッド使用前にここに必要なキー取得方法を登録してから呼び出す事。<br/>
	/// <see cref="EncryptType.Rijndael"/>を使う際に必須(他では不要)。<br/>
	/// ※クラス内で完結できていないが、これはユーティリティクラスなので、リソース所持は呼び出し側へ任せる。<br/>
	/// ※効果があるか疑問だが少しでも保持時間を減らす為に取得方法のみ保持する方式にしてみた。<br/>
	/// </summary>
	//------------------------------------------------------------
	internal static System.Func<byte[]> EncryptSaltFunc {
		private get;
		set;
	}

	//------------------------------------------------------------
	/// <summary>
	/// パスフレーズとsaltから共有キーと初期化ベクタを作成する。<br/>
	/// </summary>
	/// <param name="phrase">基になるパスフレーズ。</param>
	/// <param name="salt">基になるSaltキー。</param>
	/// <param name="keySize">共有キーのサイズ(単位ビット)。</param>
	/// <param name="key">作成された共有キー。</param>
	/// <param name="blockSize">初期化ベクタのサイズ(単位ビット)。</param>
	/// <param name="iv">作成された初期化ベクタ。</param>
	//------------------------------------------------------------
	private static void GenerateKeyFromPassphrase(string phrase, byte[] salt, int keySize, out byte[] key, int blockSize, out byte[] iv) {
		// saltは必ず8バイト以上。
		if (salt.Length < 8) {
			throw new System.Exception("wrong salt length=" + salt.Length);
		}

		// Rfc2898DeriveBytesオブジェクトを作成する。
		var deriveBytes = new Rfc2898DeriveBytes(phrase, salt); // ←.Net4以降でないとDisposeはない。

		// 共有キーと初期化ベクタを生成する。
		key = deriveBytes.GetBytes(keySize / 8);
		iv = deriveBytes.GetBytes(blockSize / 8);
	}
}