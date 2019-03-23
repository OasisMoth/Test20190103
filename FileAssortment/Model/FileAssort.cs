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

        public delegate void AssortCompleteEventHandler(object sender, AssortCompleteEventArgs e);

        public event AssortCompleteEventHandler AssortComplete;

        // HACK: かっこわるいのでなんとかしたい
        private bool hasAssortError = false;

        public void AssortFile(string targetDirPath)
        {
            this.hasAssortError = false;
            var targetDirInfo = new DirectoryInfo(targetDirPath);
            var fileNamesGroup = new Dictionary<string, List<string>>();

            foreach (var fileName in targetDirInfo.EnumerateFiles().ToList().Select(x => x.Name))
            {
                try
                {
                    MoveFileToNameTagDir(targetDirPath, fileName, ref fileNamesGroup);
                }
                catch (Exception e)
                {
                    this.hasAssortError = true;
                    logger.Warn($"FileAssort occured error. File:{fileName}", e);
                }
            }

            foreach (var fileName in fileNamesGroup.Where(x => x.Value.Count() >= 3).Select(x => x.Value).SelectMany(x => x))
            {
                try
                {
                    MoveFileToNewNameTagDir(targetDirPath, fileName);
                }
                catch (Exception e)
                {
                    this.hasAssortError = true;
                    logger.Warn($"FileAssort occured error. File:{fileName}", e);
                }
            }

            AssortComplete(this , new AssortCompleteEventArgs(this.hasAssortError));
        }

        /// <summary>
        /// ファイル名から半角スペース区切りで"名前タグ"を抽出する
        /// 例: "hoge foo.zip"→名前タグは"hoge"
        /// </summary>
        /// <param name="fileName">対象ファイル名</param>
        /// <returns>名前タグ</returns>
        private string PickNameTag(string fileName)
        {
            int spaceIndex = fileName.IndexOf(" ");
            return spaceIndex > 0 ? fileName.Substring(0, spaceIndex).Replace("[", "").Replace("]", "") : fileName;
        }

        /// <summary>
        /// 対象ファイルを名前タグと同名のサブフォルダに移動する。
        /// 同名サブフォルダが存在しない場合は、Dictionaryにメモする
        /// </summary>
        /// <param name="targetDirPath">対象ファイルの存在するフォルダ</param>
        /// <param name="fileName">対象ファイルの名前</param>
        /// <param name="fileNamesGroup">メモするDictionary</param>
        private void MoveFileToNameTagDir(string targetDirPath, string fileName, ref Dictionary<string, List<string>> fileNamesGroup)
        {
            var nameTag = PickNameTag(fileName);
            var destDirPath = Path.Combine(targetDirPath, nameTag);
            var destFullPath = Path.Combine(destDirPath, fileName);

            if (Directory.Exists(destDirPath) && File.Exists(destFullPath) == false)
            {
                File.Move(Path.Combine(targetDirPath, fileName), destFullPath);
                logger.Info($"File Move :{fileName}");
            }
            else
            {
                if (fileNamesGroup.ContainsKey(nameTag))
                {
                    fileNamesGroup[nameTag].Add(fileName);
                }
                else
                {
                    fileNamesGroup.Add(nameTag, new List<string> { fileName });
                }
            }
        }

        /// <summary>
        /// 対象ファイルの名前タグと同名のサブフォルダを作成し、移動する。
        /// </summary>
        /// <param name="targetDirPath">対象ファイルの存在するフォルダ</param>
        /// <param name="fileName">対象ファイルの名前</param>
        private void MoveFileToNewNameTagDir(string targetDirPath, string fileName)
        {
            var nameTag = PickNameTag(fileName);
            var destDirPath = Path.Combine(targetDirPath, nameTag);

            if (Directory.Exists(destDirPath) == false)
            {
                Directory.CreateDirectory(destDirPath);
                logger.Info($"Folder Create :{nameTag}");
            }

            File.Move(Path.Combine(targetDirPath, fileName), Path.Combine(destDirPath, fileName));
            logger.Info($"File Move :{fileName}");
        }

        public string SelectDirectory()
        {
            var dialog = new CommonOpenFileDialog(Resources.W_FolderSelectDialog)
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                logger.Info($"Target folder :{dialog.FileName}");
                return dialog.FileName;
            }
            return Resources.M_NoSelectFolder;
        }
    }
}
