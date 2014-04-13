using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.IsolatedStorage;

using GermanWordsClassLibrary.Class;

namespace GermanWordsClassLibrary.Helper
{
    public class IsolatedStorageSettingsHelper
    {
        public static class SettingStrings
        {
            public static string APP_VERSION = "APP_VERSION";   // string

            public static string WORD_TODAY_CARD_TYPE = "WORD_TODAY_CARD_TYPE";
            public static string WORD_TODAY_WORD_ID = "WORD_TODAY_WORD_ID";
            public static string WORD_TODAY_SOURCE_ID = "WORD_TODAY_SOURCE_ID";
            public static string WORD_TODAY_LAST_UPDATE_DATETIME = "WORD_TODAY_LAST_UPDATE_DATETIME";
            public static string WORD_TODAY_SELECT_MODE_INDEX = "WORD_TODAY_SELECT_MODE_INDEX";
            public static string WORD_TODAY_SELECTED_SOURCE_LIST = "WORD_TODAY_SELECTED_SOURCE_LIST";

            public static string IS_EXAMPLE_SKIPPED = "IS_EXAMPLE_SKIPPED";
            public static string LAST_WORD_SOURCE_INDEX = "LAST_WORD_SOURCE_INDEX";
            public static string LAST_WORD_TYPE_INDEX = "LAST_WORD_TYPE_INDEX";
            public static string LAST_APPEARANCE_INDEX = "LAST_APPEARANCE_INDEX";
            public static string LAST_GENDER_INDEX = "LAST_GENDER_INDEX";

            public static string STARRED_WORD_CARD_TYPE = "STARRED_WORD_CARD_TYPE";
            public static string STARRED_WORD_ORDER_BY_INDEX = "STARRED_WORD_ORDER_BY_INDEX";
            public static string STARRED_WORD_ASC_OR_DESC_INDEX = "STARRED_WORD_ASC_OR_DESC_INDEX";

            public static string IS_MAIN_TILE_UPDATE_ON = "IS_MAIN_TILE_UPDATE_ON";
            public static string MAIN_TILE_LAST_UPDATE_DATE = "MAIN_TILE_LAST_UPDATE_DATE";
            public static string WORD_TILE_LAST_UPDATE_TIME = "WORD_TILE_LAST_UPDATE_TIME";

            public static string SINA_WEIBO_ACCESS_TOKEN = "SINA_WEIBO_ACCESS_TOKEN";
            public static string SINA_WEIBO_REFLESH_TOKEN = "SINA_WEIBO_REFLESH_TOKEN";

            public static string IS_DEUTSCH_PACKAGE_EXIST = "IS_DEUTSCH_PACKAGE_EXIST";
            public static string IS_SPEECH_OPEN = "IS_SPEECH_OPEN";

            public static bool Contains(string key)
            {
                if (key.Equals(APP_VERSION)) return true;

                if (key.Equals(WORD_TODAY_CARD_TYPE)) return true;
                if (key.Equals(WORD_TODAY_WORD_ID)) return true;
                if (key.Equals(WORD_TODAY_SOURCE_ID)) return true;
                if (key.Equals(WORD_TODAY_LAST_UPDATE_DATETIME)) return true;
                if (key.Equals(WORD_TODAY_SELECT_MODE_INDEX)) return true;
                if (key.Equals(WORD_TODAY_SELECTED_SOURCE_LIST)) return true;

                if (key.Equals(IS_EXAMPLE_SKIPPED)) return true;
                if (key.Equals(LAST_WORD_SOURCE_INDEX)) return true;
                if (key.Equals(LAST_WORD_TYPE_INDEX)) return true;
                if (key.Equals(LAST_APPEARANCE_INDEX)) return true;

                if (key.Equals(STARRED_WORD_CARD_TYPE)) return true;
                if (key.Equals(STARRED_WORD_ORDER_BY_INDEX)) return true;
                if (key.Equals(STARRED_WORD_ASC_OR_DESC_INDEX)) return true;

                if (key.Equals(IS_MAIN_TILE_UPDATE_ON)) return true;
                if (key.Equals(MAIN_TILE_LAST_UPDATE_DATE)) return true;
                if (key.Equals(WORD_TILE_LAST_UPDATE_TIME)) return true;

                if (key.Equals(SINA_WEIBO_ACCESS_TOKEN)) return true;
                if (key.Equals(SINA_WEIBO_REFLESH_TOKEN)) return true;

                if (key.Equals(IS_DEUTSCH_PACKAGE_EXIST)) return true;
                if (key.Equals(IS_SPEECH_OPEN)) return true;

                return false;
            }
        }

        public static void WriteSetting(string key, object value)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static object ReadSetting(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                object value = IsolatedStorageSettings.ApplicationSettings[key];
                return value;
            }
            else
                return null;
        }

        public static bool IsSettingExisted(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                return true;
            else
                return false;
        }

        public static void DeleteSetting(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(key);
            }
        }

        public static void DeleteUselessSettings()
        {
            System.Collections.ICollection settingCollection = IsolatedStorageSettings.ApplicationSettings.Keys;
            System.Collections.IEnumerator enumerator = settingCollection.GetEnumerator();
            while (enumerator != null && enumerator.MoveNext())
            {
                string key = enumerator.Current as string;
                if (SettingStrings.Contains(key) == false)
                    DeleteSetting(key);
            }
        }

        #region APP_VERSION
        /*
        public static void SetAppVersion(Version version)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.APP_VERSION) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.APP_VERSION, version);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.APP_VERSION] = version;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }
        */

        public static Version GetAppVersion()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.APP_VERSION))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.APP_VERSION];
            }

            return value as Version;
        }

        #endregion

        #region WORD_TODAY_CARD_TYPE

        public static void SetWordTodayCardType(int cardType)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_CARD_TYPE) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.WORD_TODAY_CARD_TYPE, cardType);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_CARD_TYPE] = cardType;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetWordTodayCardType()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_CARD_TYPE))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_CARD_TYPE];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        #endregion

        #region WORD_TODAY_WORD_ID

        public static void SetWordTodayWordId(int wordId)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_WORD_ID) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.WORD_TODAY_WORD_ID, wordId);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_WORD_ID] = wordId;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetWordTodayWordId()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_WORD_ID))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_WORD_ID];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        #endregion

        #region WORD_TODAY_SOURCE_ID

        public static void SetWordTodaySourceId(int sourceId)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_SOURCE_ID) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.WORD_TODAY_SOURCE_ID, sourceId);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_SOURCE_ID] = sourceId;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetWordTodaySourceId()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_SOURCE_ID))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_SOURCE_ID];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        #endregion

        #region WORD_TODAY_LAST_UPDATE_DATETIME

        public static void SetWordTodayLastUpdateDateTime(DateTime dateTime)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME, dateTime);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME] = dateTime;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static DateTime GetWordTodayLastUpdateDateTime()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME];
            }

            if (value is DateTime)
                return (DateTime)value;
            else
                return new DateTime();
        }

        #endregion

        #region WORD_TODAY_SELECT_MODE_INDEX

        public static void SetWordTodaySelectModeIndex(int index)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_SELECT_MODE_INDEX) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.WORD_TODAY_SELECT_MODE_INDEX, index);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_SELECT_MODE_INDEX] = index;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetWordTodaySelectModeIndex()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_SELECT_MODE_INDEX))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_SELECT_MODE_INDEX];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        #endregion

        #region WORD_TODAY_SELECTED_SOURCE_LIST

        public static void SetWordTodaySelectedSourceList(List<WordSource> selectedList)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST, selectedList);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST] = selectedList;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static List<WordSource> GetWordTodaySelectedSourceList()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST];
            }

            return value as List<WordSource>;
        }

        #endregion

        #region IS_EXAMPLE_SKIPPED

        public static void SetExampleSkippedState(bool isSkipped)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_EXAMPLE_SKIPPED) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.IS_EXAMPLE_SKIPPED, isSkipped);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_EXAMPLE_SKIPPED] = isSkipped;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool isExampleSkipped()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_EXAMPLE_SKIPPED))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_EXAMPLE_SKIPPED];
            }

            if (value is bool)
                return (bool)value;
            else
                return false;
        }

        #endregion

        #region LAST_WORD_SOURCE_INDEX

        public static void SetLastWordSourceIndex(int index)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_WORD_SOURCE_INDEX) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.LAST_WORD_SOURCE_INDEX, index);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_WORD_SOURCE_INDEX] = index;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetLastWordSourceIndex()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_WORD_SOURCE_INDEX))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_WORD_SOURCE_INDEX];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        #endregion

        #region LAST_WORD_TYPE_INDEX

        public static void SetLastWordTypeIndex(int index)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_WORD_TYPE_INDEX) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.LAST_WORD_TYPE_INDEX, index);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_WORD_TYPE_INDEX] = index;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetLastWordTypeIndex()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_WORD_TYPE_INDEX))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_WORD_TYPE_INDEX];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        #endregion

        #region LAST_APPEARANCE_INDEX

        public static void SetLastAppearanceIndex(int index)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_APPEARANCE_INDEX) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.LAST_APPEARANCE_INDEX, index);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_APPEARANCE_INDEX] = index;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        public static int GetLastAppearanceIndex()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_APPEARANCE_INDEX))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_APPEARANCE_INDEX];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        public static void SetLastGenderIndex(int index)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_GENDER_INDEX) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.LAST_GENDER_INDEX, index);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_GENDER_INDEX] = index;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetLastGenderIndex()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.LAST_GENDER_INDEX))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.LAST_GENDER_INDEX];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }
        #endregion

        #region STARRED_WORD_CARD_TYPE

        public static void SetStarredWordCardType(int cardType)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.STARRED_WORD_CARD_TYPE) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.STARRED_WORD_CARD_TYPE, cardType);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.STARRED_WORD_CARD_TYPE] = cardType;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static int GetStarredWordCardType()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.STARRED_WORD_CARD_TYPE))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.STARRED_WORD_CARD_TYPE];
            }

            if (value is int)
                return (int)value;
            else
                return -1;
        }

        #endregion

        #region IS_MAIN_TILE_UPDATE_ON

        public static void SetMainTileUpdateState(bool isUpdateOn)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_MAIN_TILE_UPDATE_ON) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.IS_MAIN_TILE_UPDATE_ON, isUpdateOn);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_MAIN_TILE_UPDATE_ON] = isUpdateOn;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool IsMainTileUpdateOn() // default on
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_MAIN_TILE_UPDATE_ON))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_MAIN_TILE_UPDATE_ON];
            }

            if (value is bool)
                return (bool)value;
            else
                return true;
        }

        #endregion

        #region IS_SPEECH_ON

        public static void SetSpeechPackageState(bool isUpdateOn)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_DEUTSCH_PACKAGE_EXIST) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.IS_DEUTSCH_PACKAGE_EXIST, isUpdateOn);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_DEUTSCH_PACKAGE_EXIST] = isUpdateOn;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool IsSpeechPackageExist() // default off
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_DEUTSCH_PACKAGE_EXIST))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_DEUTSCH_PACKAGE_EXIST];
            }

            if (value is bool)
                return (bool)value;
            else
                return false;
        }

        public static void SetSPeechState(bool isUpdateOn)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_SPEECH_OPEN) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.IS_SPEECH_OPEN, isUpdateOn);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_SPEECH_OPEN] = isUpdateOn;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool IsSpeechExist() // default off
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.IS_SPEECH_OPEN))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.IS_SPEECH_OPEN];
            }

            if (value is bool)
                return (bool)value;
            else
                return false;
        }

        #endregion

        #region MAIN_TILE_LAST_UPDATE_DATE

        public static void SetMainTileLastUpdateDate(DateTime date)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.MAIN_TILE_LAST_UPDATE_DATE) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.MAIN_TILE_LAST_UPDATE_DATE, date);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.MAIN_TILE_LAST_UPDATE_DATE] = date;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static DateTime GetMainTileLastUpdateDate()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.MAIN_TILE_LAST_UPDATE_DATE))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.MAIN_TILE_LAST_UPDATE_DATE];
            }

            if (value is DateTime)
                return (DateTime)value;
            else
                return new DateTime();
        }

        #endregion

        #region WORD_TILE_LAST_UPDATE_TIME

        public static void SetWordTileLastUpdateTime(DateTime time)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TILE_LAST_UPDATE_TIME) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.WORD_TILE_LAST_UPDATE_TIME, time);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TILE_LAST_UPDATE_TIME] = time;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static DateTime GetWordTileLastUpdateTime()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.WORD_TILE_LAST_UPDATE_TIME))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.WORD_TILE_LAST_UPDATE_TIME];
            }
            
            if (value is DateTime)
                return (DateTime)value;
            else
                return new DateTime();
        }

        #endregion

        #region SINA_WEIBO_ACCESS_TOKEN

        public static void SetSinaWeiboAccessToken(string accessToken)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.SINA_WEIBO_ACCESS_TOKEN) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.SINA_WEIBO_ACCESS_TOKEN, accessToken);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.SINA_WEIBO_ACCESS_TOKEN] = accessToken;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static string GetSinaWeiboAccessToken()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.SINA_WEIBO_ACCESS_TOKEN))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.SINA_WEIBO_ACCESS_TOKEN];
            }

            return value as string;
        }

        #endregion

        #region SINA_WEIBO_REFLESH_TOKEN

        public static void SetSinaWeiboRefleshToken(string refleshToken)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.SINA_WEIBO_REFLESH_TOKEN) == false)
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingStrings.SINA_WEIBO_REFLESH_TOKEN, refleshToken);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingStrings.SINA_WEIBO_REFLESH_TOKEN] = refleshToken;
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static string GetSinaWeiboRefleshToken()
        {
            object value = null;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingStrings.SINA_WEIBO_REFLESH_TOKEN))
            {
                value = IsolatedStorageSettings.ApplicationSettings[SettingStrings.SINA_WEIBO_REFLESH_TOKEN];
            }

            return value as string;
        }

        #endregion
    }
}
