using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using System.Windows.Navigation;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.UserControl.WordCard;
using GermanWordsClassLibrary.Helper;

namespace GermanWords.Page
{
    public partial class LearningSetting : PhoneApplicationPage
    {
        private bool isNewPageInstance = false;
        private bool canNav = true;

        WordSource selectedWordSource;
        Uri wordSourceUri;
        List<Word> wordList;
        int wordCardType;

        public LearningSetting()
        {
            InitializeComponent();
            isNewPageInstance = true;

            if (App.dictationOrLearn)
            {
                //exampleItem.Visibility = Visibility.Visible;
                exampleItem.Header = "卡片示例";
                Grid_ExampleCard.Visibility = Visibility.Visible;
                TextBlock_Explain.Visibility = Visibility.Collapsed;
                TextBlock_CardType.Visibility = Visibility.Visible;
                ListPicker_CardType.Visibility = Visibility.Visible;
                TextBlock_Notify.Visibility = Visibility.Visible;
                // skip example
                if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.IS_EXAMPLE_SKIPPED))
                {
                    if (IsolatedStorageSettingsHelper.isExampleSkipped())
                    {
                        CheckBox_SkipExample.IsChecked = true;
                        Pivot_Main.SelectedIndex = 1;
                    }
                    else
                    {
                        CheckBox_SkipExample.IsChecked = false;
                    }
                }
            }
            else
            {
                exampleItem.Header = "默写说明";
                Grid_ExampleCard.Visibility = Visibility.Collapsed;
                //exampleItem.Visibility = Visibility.Collapsed;
                TextBlock_Explain.Visibility = Visibility.Visible;
                TextBlock_CardType.Visibility = Visibility.Collapsed;
                ListPicker_CardType.Visibility = Visibility.Collapsed;
                TextBlock_Notify.Visibility = Visibility.Collapsed;
                Pivot_Main.SelectedIndex = 1;
            }

            

            // initial word source list
            ListPicker_WordSource.DataContext = (Application.Current as App).wordSourceList;

            // read settings
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.LAST_WORD_SOURCE_INDEX))
            {
                int index = IsolatedStorageSettingsHelper.GetLastWordSourceIndex();
                if (index < ListPicker_WordSource.Items.Count)
                    ListPicker_WordSource.SelectedIndex = index;
            }
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.LAST_WORD_TYPE_INDEX))
            {
                int index = IsolatedStorageSettingsHelper.GetLastWordTypeIndex();
                if (index < ListPicker_WordType.Items.Count)
                    ListPicker_WordType.SelectedIndex = index;
            }
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.LAST_APPEARANCE_INDEX))
            {
                int index = IsolatedStorageSettingsHelper.GetLastAppearanceIndex();
                if (index < ListPicker_AppearanceOrder.Items.Count)
                    ListPicker_AppearanceOrder.SelectedIndex = index;
            }
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.LAST_GENDER_INDEX))
            {
                int index = IsolatedStorageSettingsHelper.GetLastGenderIndex();
                if (index < ListPicker_Gender.Items.Count)
                    ListPicker_Gender.SelectedIndex = index;
            }
            // save settings when user changes seletion
            // this should be done only after initialization
            ListPicker_WordSource.SelectionChanged += ListPicker_WordSource_SelectionChanged;
            ListPicker_WordType.SelectionChanged += ListPicker_WordType_SelectionChanged;
            ListPicker_AppearanceOrder.SelectionChanged += ListPicker_AppearanceOrder_SelectionChanged;
            ListPicker_Gender.SelectionChanged += ListPicker_Gender_SelectionChanged;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (e.NavigationMode == NavigationMode.New)
            {
                if (NavigationContext.QueryString.ContainsKey("cardtype"))
                    ListPicker_CardType.SelectedIndex = int.Parse(NavigationContext.QueryString["cardtype"]) - 1;
            }

            // restore from saved page states
            if (isNewPageInstance)
            {
                RestorePageStates();
                isNewPageInstance = false;
            }
        }

        private void SelectedWordList()
        {
            selectedWordSource = ListPicker_WordSource.SelectedItem as WordSource;
            wordSourceUri = selectedWordSource.fileUri;
            wordList = WordFileHelper.ReadWordFileToList(wordSourceUri);
            wordCardType = ListPicker_CardType.SelectedIndex + 1;

            // filter words
            if (ListPicker_WordType.SelectedIndex == 1) // noun only
            {
                List<Word> NoneWordList = new List<Word>();
                for (int i = 0; i < wordList.Count; i++)
                {
                    if (wordList[i] is Noun)
                        NoneWordList.Add(wordList[i]);
                }
                if (ListPicker_Gender.SelectedIndex == 0)
                {

                    wordList = NoneWordList;
                }
                else
                {
                    if (ListPicker_Gender.SelectedIndex == 1)
                    {
                        List<Word> updatedWordList = new List<Word>();
                        foreach (Noun temp in NoneWordList)
                        {
                            if (temp.gender == WordGender.Masculine)
                            {
                                updatedWordList.Add(temp);
                            }
                        }
                        wordList = updatedWordList;
                    }
                    else if (ListPicker_Gender.SelectedIndex == 2)
                    {
                        List<Word> updatedWordList = new List<Word>();
                        foreach (Noun temp in NoneWordList)
                        {
                            if (temp.gender == WordGender.Feminine)
                            {
                                updatedWordList.Add(temp);
                            }
                        }
                        wordList = updatedWordList;
                    }
                    else if (ListPicker_Gender.SelectedIndex == 3)
                    {
                        List<Word> updatedWordList = new List<Word>();
                        foreach (Noun temp in NoneWordList)
                        {
                            if (temp.gender == WordGender.Neuter)
                            {
                                updatedWordList.Add(temp);
                            }
                        }
                        wordList = updatedWordList;
                    }
                    else if (ListPicker_Gender.SelectedIndex == 4)
                    {
                        List<Word> updatedWordList = new List<Word>();
                        foreach (Noun temp in NoneWordList)
                        {
                            if (temp.gender == WordGender.MasculineOrNeuter)
                            {
                                updatedWordList.Add(temp);
                            }
                        }
                        wordList = updatedWordList;
                    }
                }


            }
            else if (ListPicker_WordType.SelectedIndex == 2) // verb only
            {
                List<Word> updatedWordList = new List<Word>();
                for (int i = 0; i < wordList.Count; i++)
                {
                    if (wordList[i] is Verb)
                        updatedWordList.Add(wordList[i]);
                }
                wordList = updatedWordList;
            }
            else if (ListPicker_WordType.SelectedIndex == 3) // phrase only
            {
                List<Word> updatedWordList = new List<Word>();
                for (int i = 0; i < wordList.Count; i++)
                {
                    if (wordList[i].wordType == WordType.Phrase)
                        updatedWordList.Add(wordList[i]);
                }
                wordList = updatedWordList;
            }

            if (ListPicker_AppearanceOrder.SelectedIndex == 1)  // random
            {
                List<Word> updatedWordList = new List<Word>();
                int wordListCount = wordList.Count;
                for (int i = 0; i < wordListCount; i++)
                {
                    Random random = new Random();
                    int selectedIndex = random.Next(wordList.Count);
                    updatedWordList.Add(wordList[selectedIndex]);
                    wordList.RemoveAt(selectedIndex);
                }
                wordList = updatedWordList;
            }
            if (wordList.Count == 0)
            {
                canNav = false;
                MessageBox.Show("此范围无卡片");
            }
            else
            {
                canNav = true;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
             //save page states
            if (e.NavigationMode != NavigationMode.Back)
            {
                SavePageStates();
            }

             //pass parameters when navigating
            if (e.Content is Page.WordCardLearning)
            {
                Page.WordCardLearning learntargetPage = e.Content as Page.WordCardLearning;
                learntargetPage.SetParameters(wordList, selectedWordSource, wordCardType, 0);
            }
            else if(e.Content is Page.DictationPage)
            {
                Page.DictationPage dictationtargetPage = e.Content as Page.DictationPage;
                dictationtargetPage.SetParameters(wordList, selectedWordSource, wordCardType, 0);
            }
        }

        private class PageStateStrings
        {
            public static string PIVOT_ITEM_INDEX = "PIVOT_ITEM_INDEX";
            public static string CARD_TYPE = "CARD_TYPE";
        }

        private void SavePageStates()
        {
            State[PageStateStrings.PIVOT_ITEM_INDEX] = Pivot_Main.SelectedIndex.ToString();
            State[PageStateStrings.CARD_TYPE] = ListPicker_CardType.SelectedIndex.ToString();
        }

        private void RestorePageStates()
        {
            // direct to pivot item
            if (State.ContainsKey(PageStateStrings.PIVOT_ITEM_INDEX))
            {
                Pivot_Main.SelectedIndex = int.Parse(State[PageStateStrings.PIVOT_ITEM_INDEX] as string);
            }

            // set values
            if (State.ContainsKey(PageStateStrings.CARD_TYPE))
            {
                ListPicker_CardType.SelectedIndex = int.Parse(State[PageStateStrings.CARD_TYPE] as string);
            }
        }

        private void Pivot_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Pivot_Main.SelectedIndex == 0)
            {
                StackPanel_ExampleCard.Children.Clear();

                Noun noun = new Noun("Lektion", "课，单元", WordGender.Feminine, "-en", "");
                System.Windows.Controls.UserControl uc;
                switch (ListPicker_CardType.SelectedIndex)
                {
                    case 0:
                        uc = new OneSideNounCard(noun);
                        TextBlock_ReversiblePrompt.Opacity = 0;
                        break;
                    case 1:
                        uc = new TwoSideNounCard(noun, true);
                        TextBlock_ReversiblePrompt.Opacity = 100;
                        break;
                    case 2:
                        uc = new TwoSideNounCard(noun, false);
                        TextBlock_ReversiblePrompt.Opacity = 100;
                        break;
                    default:
                        uc = null;
                        break;
                }
                StackPanel_ExampleCard.Children.Add(uc);
            }
        }

        private void CheckBox_SkipExample_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetExampleSkippedState(true);
        }

        private void CheckBox_SkipExample_Unchecked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetExampleSkippedState(false);
        }

        private void ListPicker_WordSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetLastWordSourceIndex(ListPicker_WordSource.SelectedIndex);
        }

        private void ListPicker_WordType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetLastWordTypeIndex(ListPicker_WordType.SelectedIndex);
            if (ListPicker_WordType.SelectedIndex == 1)
            {
                Gender_TextBlock.Visibility = Visibility.Visible;
                ListPicker_Gender.Visibility = Visibility.Visible;
            }
            else
            {
                Gender_TextBlock.Visibility = Visibility.Collapsed;
                ListPicker_Gender.Visibility = Visibility.Collapsed;
            }
        }

        void ListPicker_Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetLastGenderIndex(ListPicker_Gender.SelectedIndex);
        }

        private void ListPicker_AppearanceOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetLastAppearanceIndex(ListPicker_AppearanceOrder.SelectedIndex);
        }

        private void Button_Return_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            SelectedWordList();
            if (canNav)
            {
                //NavigationService.Navigate(new Uri("/Page/WordCardLearning.xaml", UriKind.Relative));
                if (App.dictationOrLearn)
                {
                    NavigationService.Navigate(new Uri("/Page/WordCardLearning.xaml", UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Page/DictationPage.xaml", UriKind.Relative));
                }
                
            }

            /* parameters will be passed in OnNavigatedFrom */
        }
    }
}