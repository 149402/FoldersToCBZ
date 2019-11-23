using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FoldersToCBZ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openDirectoryDialogButton_Click(object sender, EventArgs e)
        {
            this.openDirectoryDialogButton.Enabled = false;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            Task.Run(() =>
            {
                var directories = Directory.GetDirectories(dialog.FileName);

                for (int currentDirectoryIndex = 0; currentDirectoryIndex < directories.Length; currentDirectoryIndex++)
                {
                    this.Invoke((MethodInvoker)delegate { statusLabel.Text = $"Working: {currentDirectoryIndex} / {directories.Length}."; });
                    try
                    {
                        ZipFile.CreateFromDirectory(directories[currentDirectoryIndex], directories[currentDirectoryIndex] + ".cbz");
                    }
                    catch (IOException exception)
                    {
                        MessageBox.Show(exception.Message);
                        this.Invoke((MethodInvoker)delegate 
                        {
                            statusLabel.Text = $"Failed";
                            this.openDirectoryDialogButton.Enabled = true;
                        });

                        return;
                    }
                }

                this.Invoke((MethodInvoker)delegate 
                {
                    statusLabel.Text = $"Done: {directories.Length} / {directories.Length}.";
                    this.openDirectoryDialogButton.Enabled = true;
                });

            });
        }
    }
}
