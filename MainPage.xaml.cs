using Microsoft.Maui.Storage;
using System.Security.Cryptography;
using System.Text;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace SHA
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private async void btnDosyaYukle_Clicked(object sender, EventArgs e)
        {
            editorKullaniciMetni.Text = "";
            PickOptions options = new()
            {
                PickerTitle = "Please select a comic file",
            };
            var result = await FilePicker.Default.PickAsync(options);
            var fileName = result.FullPath;
            var textEncrypted = " ";
            var isSha256 = btnSha256.IsChecked;
            var isSha512 = btnSha512.IsChecked;
            if (isSha256)
            {
                textEncrypted = BytesToString(GetFileHashSha256(fileName));
            }
            if (isSha512)
            {
                textEncrypted = BytesToString(GetFileHashSha512(fileName));
            }
            editorSifrelenmisMetin.Text = textEncrypted;
        }

        // Return a byte array as a sequence of hex values.
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }

        // Compute the file's hash.
        private byte[] GetFileHashSha256(string filename)
        {
            // The cryptographic service provider.
            SHA256 Sha256 = SHA256.Create();
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        private byte[] GetFileHashSha512(string filename)
        {
            // The cryptographic service provider.
            SHA512 Sha512 = SHA512.Create();
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha512.ComputeHash(stream);
            }
        }

        private void editorKullaniciMetni_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = editorKullaniciMetni.Text;
            var textEncrypted = "";
            var isSha256 = btnSha256.IsChecked;
            var isSha512 = btnSha512.IsChecked;

            if(text is not null && text is not "")
            {
                if (isSha256 )
                {
                    textEncrypted = ComputeSha256Hash(text);
                }
                if (isSha512 )
                {
                    textEncrypted = ComputeSha512Hash(text);
                }
            }
            editorSifrelenmisMetin.Text = textEncrypted;

        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string ComputeSha512Hash(string rawData)
        {
            // Create a SHA256
            using (SHA512 sha256Hash = SHA512.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnSha256_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(editorKullaniciMetni.Text is null || editorKullaniciMetni.Text is "")
            {
                btnDosyaYukle_Clicked(sender, null);
            }
            else
            {
                editorKullaniciMetni_TextChanged(sender, null);
            }
            
        }

    }

}
