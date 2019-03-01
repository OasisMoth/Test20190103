using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using FileAssortment.Properties;
using System.IO;
using log4net;
using System.Windows;

namespace FileAssortment
{
    public class FileAssort
    {
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void AssortAction(string targetDir)
        {
            var targetDirInfo = new DirectoryInfo(targetDir);

            var targetFiles = targetDirInfo.EnumerateFiles();
            var targetSubDirs = targetDirInfo.GetDirectories();


            foreach (var file in targetFiles)
            {
                var spaceIndex = file.Name.IndexOf(" ");
                var splitFileName = spaceIndex > 0 ? file.Name.Substring(0, spaceIndex) : file.Name.Replace("[", "").Replace("]", "");

                foreach (var subDir in targetSubDirs)
                {
                    if (subDir.Name == splitFileName && File.Exists(Path.Combine(subDir.FullName, file.Name)) == false)
                    {
                        file.MoveTo(Path.Combine(subDir.FullName, file.Name));
                        logger.Info($"Folder\"{subDir.Name}\" {file.Name}");
                        break;
                    }
                }
            }

            MessageBox.Show("ファイルの仕分けが完了しました。");
        }

        public string TargetAction()
        {
            var dialog = new CommonOpenFileDialog(Resources.W_FolderSelectDialog)
            {
                IsFolderPicker = true
            };

            var ret = dialog.ShowDialog();

            if (ret == CommonFileDialogResult.Ok) return dialog.FileName;
            return Resources.M_NoSelectFolder;
        }
    }
}
