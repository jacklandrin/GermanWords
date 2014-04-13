using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows;
using System.Windows.Resources;

using GermanWordsClassLibrary.Class;

namespace GermanWordsClassLibrary.Helper
{
    public class WordFileHelper
    {
        public static List<Word> ReadWordFileToList(Uri fileUri)
        {
            List<Word> list = new List<Word>();

            StreamResourceInfo streamResourceInfo = Application.GetResourceStream(fileUri);
            if (streamResourceInfo != null)
            {
                Stream stream = streamResourceInfo.Stream;
                string fileContent = new StreamReader(stream).ReadToEnd();

                string[] lines = fileContent.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    Word word = ConvertLineToWord(lines[i]);
                    if (word != null)
                        list.Add(word);
                }
            }
            return list;
        }

        public static Word GetWordById(List<WordSource> wordSourceList, int sourceId, int wordId)
        {
            WordSource wordSource = WordSourceHelper.GetWordSourceById(wordSourceList, sourceId);
            if (wordSource == null)
                return null;

            StreamResourceInfo streamResourceInfo = Application.GetResourceStream(wordSource.fileUri);
            if (streamResourceInfo != null)
            {
                Stream stream = streamResourceInfo.Stream;
                string fileContent = new StreamReader(stream).ReadToEnd();

                string[] lines = fileContent.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] lineParts = lines[i].Trim().Split('\t');
                    if (int.Parse(lineParts[0]) == wordId)
                    {
                        Word word = ConvertLineToWord(lines[i]);
                        return word;
                    }
                }
            }
            return null;
        }

        // if convertion failed, return null.
        private static Word ConvertLineToWord(string line)
        {
            string[] lineParts = line.Trim().Split('\t');
            if (lineParts.Length != 5)
                return null;

            int wordId = int.Parse(lineParts[0]);
            string word;
            string translation;

            switch (lineParts[1])   // includes word type
            {
                case "n":
                    WordGender wordGender;
                    string pluralForm;
                    string genitivForm;

                    translation = lineParts[4];

                    if (lineParts[2].StartsWith("der "))
                    {
                        wordGender = WordGender.Masculine;
                        word = lineParts[2].Substring(4);
                    }
                    else if (lineParts[2].StartsWith("die "))
                    {
                        wordGender = WordGender.Feminine;
                        word = lineParts[2].Substring(4);
                    }
                    else if (lineParts[2].StartsWith("das "))
                    {
                        wordGender = WordGender.Neuter;
                        word = lineParts[2].Substring(4);
                    }
                    else if (lineParts[2].StartsWith("der/das "))
                    {
                        wordGender = WordGender.MasculineOrNeuter;
                        word = lineParts[2].Substring(8);
                    }
                    else
                    {
                        wordGender = WordGender.None;
                        word = lineParts[2];
                    }


                    string[] formParts = lineParts[3].Split(',');
                    if (formParts.Length == 1)
                    {
                        pluralForm = formParts[0];
                        genitivForm = "";
                    }
                    else if (formParts.Length == 2)
                    {
                        genitivForm = formParts[0];
                        pluralForm = formParts[1].Trim();
                    }
                    else return null;

                    return new Noun(wordId, word, translation, wordGender, pluralForm, genitivForm);

                case "pl":
                    word = lineParts[2];
                    translation = lineParts[4];
                    return new Word(wordId, WordType.PluralNoun, word, translation);

                case "vt":
                case "vi":
                case "vtvi":
                case "vr":
                    WordType wordType;
                    string presentForm;
                    string pastForm;
                    string perfectForm;
                    PerfectAuxiliaryVerb perfectAuxiliaryVerb;

                    if (lineParts[1].Equals("vt"))
                        wordType = WordType.Vt;
                    else if (lineParts[1].Equals("vi"))
                        wordType = WordType.Vi;
                    else if (lineParts[1].Equals("vtvi"))
                        wordType = WordType.VtOrVi;
                    else if (lineParts[1].Equals("vr"))
                        wordType = WordType.Vr;
                    else return null;

                    word = lineParts[2];
                    translation = lineParts[4];

                    string[] wordForms = lineParts[3].Split(',');
                    if (lineParts[3].Trim().Equals(string.Empty))
                    {
                        presentForm = "";
                        pastForm = "";
                        perfectAuxiliaryVerb = PerfectAuxiliaryVerb.NotAvailable;
                        perfectForm = "";
                    }
                    else if (wordForms.Length == 3)
                    {
                        presentForm = wordForms[0].Trim();
                        pastForm = wordForms[1].Trim();

                        if (wordForms[2].Trim().StartsWith("hat "))
                        {
                            perfectAuxiliaryVerb = PerfectAuxiliaryVerb.Haben;
                            perfectForm = wordForms[2].Trim().Substring(4);
                        }
                        else if (wordForms[2].Trim().StartsWith("ist "))
                        {
                            perfectAuxiliaryVerb = PerfectAuxiliaryVerb.Sein;
                            perfectForm = wordForms[2].Trim().Substring(4);
                        }
                        else if (wordForms[2].Trim().StartsWith("ist/hat ") || wordForms[2].Trim().StartsWith("hat/ist "))
                        {
                            perfectAuxiliaryVerb = PerfectAuxiliaryVerb.HabenOrSein;
                            perfectForm = wordForms[2].Trim().Substring(8);
                        }
                        else return null;
                    }
                    else return null;

                    return new Verb(wordId, wordType, word, translation, presentForm, pastForm, perfectForm, perfectAuxiliaryVerb);

                case "abbr":
                    string fullWord;

                    word = lineParts[2];
                    fullWord = lineParts[3];
                    translation = lineParts[4];

                    return new Abbreviation(wordId, word, translation, fullWord);

                case "adj":
                case "adv":
                case "advadj":
                case "adv/conj":
                case "p1":
                case "p2":
                case "prep":
                case "pron":
                case "conj":
                case "num":
                case "int":
                case "pron/num":
                case "ph":
                    if (lineParts[1].Equals("adj"))
                        wordType = WordType.Adjective;
                    else if (lineParts[1].Equals("adv"))
                        wordType = WordType.Adverb;
                    else if (lineParts[1].Equals("advadj"))
                        wordType = WordType.AdvOrAdj;
                    else if (lineParts[1].Equals("adv/conj"))
                        wordType = WordType.AdvOrConj;
                    else if (lineParts[1].Equals("p1"))
                        wordType = WordType.P1;
                    else if (lineParts[1].Equals("p2"))
                        wordType = WordType.P2;
                    else if (lineParts[1].Equals("prep"))
                        wordType = WordType.Preposition;
                    else if (lineParts[1].Equals("pron"))
                        wordType = WordType.Pronoun;
                    else if (lineParts[1].Equals("conj"))
                        wordType = WordType.Conjection;
                    else if (lineParts[1].Equals("num"))
                        wordType = WordType.Numeral;
                    else if (lineParts[1].Equals("int"))
                        wordType = WordType.Interjection;
                    else if (lineParts[1].Equals("pron/num"))
                        wordType = WordType.PronOrNum;
                    else if (lineParts[1].Equals("ph"))
                        wordType = WordType.Phrase;
                    else return null;

                    word = lineParts[2];
                    translation = lineParts[4];

                    return new Word(wordId, wordType, word, translation);

                default:
                    return null;
            }
        }
    }
}
