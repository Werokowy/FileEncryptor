using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace FileEcryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Note: Do not encrypt system files because they have huge privileges and if you do this the program will crash. THIS SOFTWARE IS NOT FOR EVIL PURPOSES ONLY ENCRYPT YOUR FILES!!!", "Readme", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select file to decrypt/encrypt";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if(textBox1.Text==""|| textBox1.Text == " "|| textBox1.Text == "  ")
                {
                    MessageBox.Show("Password for file cannot contains null or spaces!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                EncryptionFile.EncryptFile(openFileDialog1.FileName, textBox1.Text);
                MessageBox.Show("File encrypted");
            }
        }
        public class CoreDecryption
        {
            public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
            {
                byte[] decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }

        }
        public class CoreEncryption
        {
            public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
            {
                byte[] encryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
        }
        public class EncryptionFile
        {
            public static void EncryptFile(string file, string password)
            {

                byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = CoreEncryption.AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                string fileEncrypted = file;

                File.WriteAllBytes(fileEncrypted, bytesEncrypted);
            }
        }
        public class DecryptionFile
        {
            public static void DecryptFile(string fileEncrypted, string password)
            {

                byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = CoreDecryption.AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                string file = fileEncrypted;
                File.WriteAllBytes(file, bytesDecrypted);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select file to decrypt/encrypt";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (textBox1.Text == "" || textBox1.Text == " " || textBox1.Text == "  ")
                {
                    MessageBox.Show("Password for file cannot contains null or spaces!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    DecryptionFile.DecryptFile(openFileDialog1.FileName, textBox1.Text);
                    MessageBox.Show("File decrypted","Success!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                catch(Exception)
                {
                    MessageBox.Show("Wrong password!","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private void autorzyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Autor programu: Werokowy\nDiscord autora programu: https://dc.ghj-eu.ml\nNick autora programu na Discord: _Mr. Anonymous_#2137\nFirma: GHJ-EU.ML™️ Inc.\nGHJ-EU.ML®️ 2022. Wszelkie prawa zastrzeżone", "Autorzy", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void englishAngielskiToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program Author: Werokowy\nProgram Author Discord: https://dc.ghj-eu.ml\nProgram Author Discord DM: _Mr. Anonymous_#2137\nCompany: GHJ-EU.ML™️\nAll rights reserved to author and GHJ-EU.ML©️", "Authors", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
