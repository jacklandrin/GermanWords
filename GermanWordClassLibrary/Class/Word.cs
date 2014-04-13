using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Runtime.Serialization;

namespace GermanWordsClassLibrary.Class
{
    public enum WordType
    {
        // Noun
        Noun,
        PluralNoun,
        // Verb
        Vt,
        Vi,
        VtOrVi,
        Vr,
        // Adj Adv
        Adjective,
        Adverb,
        AdvOrAdj,
        P1,
        P2,
        // Other
        Numeral,
        Preposition,
        Pronoun,
        Conjection,
        Interjection,
        Abbreviation,
        Phrase,
        // Multiple
        PronOrNum,
        AdvOrConj
    }

    public enum WordGender
    {
        Masculine,//阳性
        Feminine,//阴性
        Neuter,//中性
        MasculineOrNeuter,//阳性或中性
        None//无
    }

    public enum PerfectAuxiliaryVerb
    {
        Haben,
        Sein,
        HabenOrSein,
        NotAvailable
    }

    [DataContract]
    [KnownType(typeof(Noun))]
    [KnownType(typeof(Verb))]
    [KnownType(typeof(Abbreviation))]
    public class Word
    {
        [DataMember]
        public int wordId;  // word id inside word source
        [DataMember]
        public WordType wordType;
        [DataMember]
        public string word { get; set; }
        [DataMember]
        public string translation;

        public Word()
        {
            wordId = 0;
        }

        public Word(WordType wordType, string word, string translation)
            : this()
        {
            this.wordType = wordType;
            this.word = word;
            this.translation = translation;
        }

        public Word(int wordId, WordType wordType, string word, string translation)
            : this(wordType, word, translation)
        {
            this.wordId = wordId;
        }

        public static string WordTypeToString(WordType wordType)
        {
            switch (wordType)
            {
                case WordType.Abbreviation:
                    return "Abbr.";
                case WordType.Adjective:
                    return "Adj.";
                case WordType.Adverb:
                    return "Adv.";
                case WordType.AdvOrAdj:
                    return "Adv./Adj.";
                case WordType.AdvOrConj:
                    return "Adv./Conj.";
                case WordType.Conjection:
                    return "Conj.";
                case WordType.Interjection:
                    return "Int.";
                case WordType.Noun:
                    return "N.";
                case WordType.Numeral:
                    return "Num.";
                case WordType.P1:
                    return "PI.";
                case WordType.P2:
                    return "PII.";
                case WordType.Phrase:
                    return "";
                case WordType.PluralNoun:
                    return "Pl.";
                case WordType.Preposition:
                    return "Prep.";
                case WordType.PronOrNum:
                    return "Pron./Num.";
                case WordType.Pronoun:
                    return "Pron.";
                case WordType.Vi:
                    return "Vi.";
                case WordType.Vr:
                    return "Vr.";
                case WordType.Vt:
                    return "Vt.";
                case WordType.VtOrVi:
                    return "Vt./Vi.";
                default:
                    return "";
            }
        }
    }

    [DataContract]
    public class Noun : Word
    {
        [DataMember]
        public WordGender gender;
        [DataMember]
        public string pluralForm;
        [DataMember]
        public string genitivForm;

        public Noun(string word, string translation, WordGender gender, string pluralForm, string genitivForm)
            : base(WordType.Noun, word, translation)
        {
            this.gender = gender;
            this.pluralForm = pluralForm;
            this.genitivForm = genitivForm;
        }

        public Noun(int wordId, string word, string translation, WordGender gender, string pluralForm, string genitivForm)
            : base(wordId, WordType.Noun, word, translation)
        {
            this.gender = gender;
            this.pluralForm = pluralForm;
            this.genitivForm = genitivForm;
        }

        public static string GenderToString(WordGender gender)
        {
            switch (gender)
            {
                case WordGender.Masculine:
                    return "der";
                case WordGender.Feminine:
                    return "die";
                case WordGender.Neuter:
                    return "das";
                case WordGender.MasculineOrNeuter:
                    return "der/das";
                case WordGender.None:
                    return "";
                default:
                    return "";
            }
        }
    }

    [DataContract]
    public class Verb : Word
    {
        [DataMember]
        public string presentForm;
        [DataMember]
        public string pastForm;
        [DataMember]
        public string perfectForm;
        [DataMember]
        public PerfectAuxiliaryVerb perfectAuxiliaryVerb;

        public Verb(WordType wordType, string word, string translation, string presentForm, string pastForm, string perfectForm, PerfectAuxiliaryVerb perfectAuxiliaryVerb)
            : base(wordType, word, translation)
        {
            this.presentForm = presentForm;
            this.pastForm = pastForm;
            this.perfectForm = perfectForm;
            this.perfectAuxiliaryVerb = perfectAuxiliaryVerb;
        }

        public Verb(int wordId, WordType wordType, string word, string translation, string presentForm, string pastForm, string perfectForm, PerfectAuxiliaryVerb perfectAuxiliaryVerb)
            : base(wordId, wordType, word, translation)
        {
            this.presentForm = presentForm;
            this.pastForm = pastForm;
            this.perfectForm = perfectForm;
            this.perfectAuxiliaryVerb = perfectAuxiliaryVerb;
        }

        public static string PerfectAuxiliaryVerbToString(PerfectAuxiliaryVerb perfectAuxiliaryVerb)
        {
            switch (perfectAuxiliaryVerb)
            {
                case PerfectAuxiliaryVerb.Haben:
                    return "hat ";
                case PerfectAuxiliaryVerb.Sein:
                    return "ist ";
                case PerfectAuxiliaryVerb.HabenOrSein:
                    return "hat/ist ";
                default:
                    return "";
            }
        }
    }

    [DataContract]
    public class Abbreviation : Word
    {
        [DataMember]
        public string fullWord;

        public Abbreviation(string word, string translation, string fullWord)
            : base(WordType.Abbreviation, word, translation)
        {
            this.fullWord = fullWord;
        }

        public Abbreviation(int wordId, string word, string translation, string fullWord)
            : base(wordId, WordType.Abbreviation, word, translation)
        {
            this.fullWord = fullWord;
        }
    } 
}
