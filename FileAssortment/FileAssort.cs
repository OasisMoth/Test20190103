﻿using System;
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
    public class AssortCompleteEventArgs : EventArgs
    {
        public bool isError;
    }

    public class FileAssort
    {
        public delegate void AssortCompleteEventHandler(object sender, AssortCompleteEventArgs e);

        public event AssortCompleteEventHandler Complete;

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void AssortFile(string targetDir)
        {
            var targetDirInfo = new DirectoryInfo(targetDir);
            var fileNamesGroup = new Dictionary<string, List<string>>();

            foreach (var file in targetDirInfo.EnumerateFiles())
            {
                try
                {
                    var splitFileName = PickNameTagFromFile(file.Name);
                    var newFullFilePath = Path.Combine(targetDir, splitFileName, file.Name);

                    if (targetDirInfo.GetDirectories().Any(dir => dir.Name == splitFileName) && File.Exists(newFullFilePath) == false)
                    {
                        file.MoveTo(newFullFilePath);
                        logger.Info($"File Move :{file.Name}");
                    }
                    else
                    {
                        if (fileNamesGroup.ContainsKey(splitFileName))
                        {
                            fileNamesGroup[splitFileName].Add(file.Name);
                        }
                        else
                        {
                            fileNamesGroup.Add(splitFileName, new List<string> { file.Name });
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Error($"FileAssort occured error. File:{file.Name}", e);
                }
            }

            // todo '3'に関して、ユーザー設定にする
            foreach (var fileNames in fileNamesGroup.Where(x => x.Value.Count() >= 3).Select(x => x.Value))
            {
                foreach (var fileName in fileNames)
                {
                    try
                    {
                        var newDirPath = Path.Combine(targetDir, PickNameTagFromFile(fileName));

                        if (Directory.Exists(newDirPath) == false) Directory.CreateDirectory(newDirPath);
                        File.Move(Path.Combine(targetDir, fileName), Path.Combine(newDirPath, fileName));
                        logger.Info($"File Move :{fileName}");
                    }
                    catch (Exception e)
                    {
                        logger.Error($"FileAssort occured error. File:{fileName}", e);
                    }
                }
            }

            MessageBox.Show(Resources.M_AssortComplete, Resources.W_ApplicationTitle, MessageBoxButton.OK);
        }

        /// <summary>
        /// ファイル名からスペース区切りで名前タグを抽出する
        /// 例: "name file.zip"→名前タグは"name"
        /// </summary>
        /// <param name="file">対象ファイル</param>
        /// <returns>名前部分</returns>
        private string PickNameTagFromFile(string fileName)
        {
            int spaceIndex = fileName.IndexOf(" ");
            return spaceIndex > 0 ? fileName.Substring(0, spaceIndex).Replace("[", "").Replace("]", "") : fileName;
        }

        public string SelectDirectory()
        {
            var dialog = new CommonOpenFileDialog(Resources.W_FolderSelectDialog)
            {
                IsFolderPicker = true
            };
            var ret = dialog.ShowDialog();

            if (ret == CommonFileDialogResult.Ok)
            {
                logger.Info($"Target folder :{dialog.FileName}");
                return dialog.FileName;
            }
            return Resources.M_NoSelectFolder;
        }
    }
}
