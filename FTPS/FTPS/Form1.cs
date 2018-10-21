using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtHost.Text = "uprod.mfe.mastercard.com";
            txtPort.Text = "16021";
            txtUser.Text = "Z630123";
            txtPassword.Text = "Password1!";
            txtCert.Text = "D:/Backup/ftpsclient.pfx";
        }

        void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            // add logic to test if certificate is valid here
            e.Accept = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new FtpClient(txtHost.Text, int.Parse(txtPort.Text), txtUser.Text, txtPassword.Text);

                client.EncryptionMode = FtpEncryptionMode.Explicit;
                client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
                client.SocketKeepAlive = false;
                client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);

                FtpTrace.AddListener(new TextWriterTraceListener("log_file.txt")
                {
                    Filter = new EventTypeFilter(SourceLevels.Error)
                });
                FtpTrace.AddListener(new CustomTraceListener(txtResult));

                try
                {
                    client.ClientCertificates.Add(new System.Security.Cryptography.X509Certificates.X509Certificate2(txtCert.Text, txtPassword.Text));
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Error when loading certificate: {ex.Message} " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                }

                client.Connect();

                if (client.IsConnected)
                {
                    client.SetWorkingDirectory("0072351");

                    var files = client.GetListing();
                    var str = "";

                    foreach (var file in files)
                    {
                        str += file.FullName + ", ";
                    }

                    MessageBox.Show("connected " + str);

                    client.Disconnect();
                    client.Dispose();
                }
                else
                    MessageBox.Show("Failed");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} " + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }
    }
}
