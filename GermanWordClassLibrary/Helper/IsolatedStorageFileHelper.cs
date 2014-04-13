using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.UserControl.LiveTilePicture;

namespace GermanWordsClassLibrary.Helper
{
    public class IsolatedStorageFileHelper
    {
        public static void CreateDirectory(string path)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.DirectoryExists(path))
                    file.CreateDirectory(path);
            }
        }

        public static void DeleteFile(string path)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists(path))
                {
                    file.DeleteFile(path);
                }
            }
        }

        public static void DeleteAllFilesOfDirectory(string path)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.DirectoryExists(path))
                {
                    string[] fileNames = file.GetFileNames(path + "/*");
                    for (int i = 0; i < fileNames.Count(); i++)
                    {
                        file.DeleteFile(path + "/" + fileNames[i]);
                    }
                }
            }
        }

        public static string[] GetFileNames(string pattern)
        {
            string[] result;
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                result = file.GetFileNames(pattern);
            }
            return result;
        }

        public static void SaveWriteableBitmap(string path, WriteableBitmap bmp, int width, int height, int quality)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = file.OpenFile(path, FileMode.Create, FileAccess.Write))
                {
                    bmp.SaveJpeg(fileStream, width, height, 0, quality);
                }
            }
        }

        #region save/read/delete RunState

        public static void SaveRunState(List<Word> wordList, int sourceId, int cardType, int startIndex)
        {
            RunState rs = new RunState(wordList, sourceId, cardType, startIndex);

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = file.OpenFile("LastRun", FileMode.Create, FileAccess.Write))
                { 
                    new DataContractSerializer(typeof(RunState)).WriteObject(fileStream, rs);
                }
            }
        }

        public static bool ReadRunState(List<WordSource> wordSourceList, out List<Word> wordList, out WordSource wordSource, out int cardType, out int startIndex)
        {
            wordList = null;
            wordSource = null;
            cardType = 0;
            startIndex = 0;

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists("LastRun"))
                {
                    using (IsolatedStorageFileStream fileStream = file.OpenFile("LastRun", FileMode.Open, FileAccess.Read))
                    {
                        RunState rs = new DataContractSerializer(typeof(RunState)).ReadObject(fileStream) as RunState;
                        int sourceId = rs.sourceId;
                        wordSource = WordSourceHelper.GetWordSourceById(wordSourceList, sourceId);
                        if (wordSource == null)
                        {
                            DeleteRunState();
                            return false;
                        }
                        else
                        {
                            wordList = rs.wordList;
                            cardType = rs.cardType;
                            startIndex = rs.startIndex;
                            return true;
                        }
                    }
                }
                else return false;
            }
        }

        public static void DeleteRunState()
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists("LastRun"))
                    file.DeleteFile("LastRun");
            }
        }

        #endregion

        // will not be used
        public static void CreateLiveTilePictures(List<WordSource> wordSourceList)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // empty direcotry
                if (file.DirectoryExists("/Shared/ShellContent/WordPictures/"))
                {
                    string[] fileNames = file.GetFileNames("/Shared/ShellContent/WordPictures/*");
                    for (int i = 0; i < fileNames.Count(); i++)
                    {
                        file.DeleteFile("/Shared/ShellContent/WordPictures/" + fileNames[i]);
                    }
                }
                else
                {
                    file.CreateDirectory("/Shared/ShellContent/WordPictures");
                }

                // generate pictures
                int wordNo = 1;
                for (int i = 0; i < wordSourceList.Count; i++)
                {
                    List<Word> wordList = WordFileHelper.ReadWordFileToList(wordSourceList[i].fileUri);
                    for (int j = 0; j < wordList.Count; j++)
                    {
                        Word word = wordList[j];
                        System.Windows.Controls.UserControl liveTilePictureControl;
                        if (word is Noun)
                            liveTilePictureControl = new LiveTileNounPicture(word as Noun);
                        else if (word is Verb)
                            liveTilePictureControl = new LiveTileVerbPicture(word as Verb);
                        else if (word is Abbreviation)
                            liveTilePictureControl = new LiveTileAbbrPicture(word as Abbreviation);
                        else
                            liveTilePictureControl = new LiveTileWordPicture(word);

                        WriteableBitmap bmp = new WriteableBitmap(liveTilePictureControl, null);
                        using (IsolatedStorageFileStream fileStream = file.OpenFile("Shared/ShellContent/WordPictures/" + wordNo + ".jpg", FileMode.Create, FileAccess.Write))
                        {
                            bmp.SaveJpeg(fileStream, 336, 336, 0, 70);
                        }
                        wordNo++;
                    }
                }
            }
        }
    }

    [DataContract]
    public class RunState
    {
        [DataMember]
        public List<Word> wordList;
        [DataMember]
        public int sourceId;
        [DataMember]
        public int cardType;
        [DataMember]
        public int startIndex;

        public RunState(List<Word> wordList, int sourceId, int cardType, int startIndex)
        {
            this.wordList = wordList;
            this.sourceId = sourceId;
            this.cardType = cardType;
            this.startIndex = startIndex;
        }
    }
}
