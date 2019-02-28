using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using FileAssortment.Properties;
using System.IO;


namespace FileAssortment
{
    public class FileAssort
    {
        public FileAssort()
        {

        }

        public void AssortAction(string targetDir)
        {
            var targetDirInfo = new DirectoryInfo(targetDir);

            var targetFiles = targetDirInfo.EnumerateFiles();
            var targetSubDirs = targetDirInfo.GetDirectories();

            foreach (var file in targetFiles)
            {
                var spaceIndex = file.Name.IndexOf(" ");
                var splitFileName = spaceIndex > 0 ? file.Name.Substring(0, spaceIndex) : file.Name;

                foreach(var subDir in targetSubDirs)
                {
                    if (subDir.Name == splitFileName)
                    {
                         file.MoveTo(Path.Combine(subDir.FullName, file.Name));
                        break;
                    }
                }
            }

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
