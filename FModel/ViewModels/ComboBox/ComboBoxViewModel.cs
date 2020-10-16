﻿using System.Collections.ObjectModel;

namespace FModel.ViewModels.ComboBox
{
    static class ProgramLang
    {
        private static readonly string _Default = "en-US";
        private static readonly string _French = "fr-FR";
        private static readonly string _German = "de-DE";
        private static readonly string _Italian = "it-IT";
        private static readonly string _Spanish = "es";
        private static readonly string _Arabic = "ar";
        private static readonly string _Japanese = "ja-JP";
        private static readonly string _Russian = "ru-RU";
        private static readonly string _ChineseSimp = "zh-CN";
        private static readonly string _Portuguese = "pt-PT";

        public static string GetProgramLang()
        {
            return Properties.Settings.Default.ProgramLanguage switch
            {
                9 => _Portuguese,
                8 => _ChineseSimp,
                7 => _Russian,
                6 => _Japanese,
                5 => _Arabic,
                4 => _Spanish,
                3 => _Italian,
                2 => _German,
                1 => _French,
                _ => _Default
            };
        }
    }

    static class ComboBoxVm
    {
        public static ObservableCollection<ComboBoxViewModel> programLanguageCbViewModel = new ObservableCollection<ComboBoxViewModel>
        {
            new ComboBoxViewModel { Id = 0, Content = Properties.Resources.English },
            new ComboBoxViewModel { Id = 1, Content = Properties.Resources.French },
            new ComboBoxViewModel { Id = 2, Content = Properties.Resources.German },
            new ComboBoxViewModel { Id = 3, Content = Properties.Resources.Italian },
            new ComboBoxViewModel { Id = 4, Content = Properties.Resources.Spanish },
            new ComboBoxViewModel { Id = 5, Content = Properties.Resources.Arabic },
            new ComboBoxViewModel { Id = 6, Content = Properties.Resources.Japanese },
            new ComboBoxViewModel { Id = 7, Content = Properties.Resources.Russian },
            new ComboBoxViewModel { Id = 8, Content = Properties.Resources.Chinese },
            new ComboBoxViewModel { Id = 9, Content = Properties.Resources.PortugueseBrazil }
        };

        public static ObservableCollection<ComboBoxViewModel> languageCbViewModel = new ObservableCollection<ComboBoxViewModel>
        {
            new ComboBoxViewModel { Id = 0, Content = Properties.Resources.English, Property = ELanguage.English },
            new ComboBoxViewModel { Id = 15, Content = Properties.Resources.AustralianEnglish, Property = ELanguage.AustralianEnglish },
            new ComboBoxViewModel { Id = 16, Content = Properties.Resources.BritishEnglish, Property = ELanguage.BritishEnglish },
            new ComboBoxViewModel { Id = 1, Content = Properties.Resources.French, Property = ELanguage.French },
            new ComboBoxViewModel { Id = 2, Content = Properties.Resources.German, Property = ELanguage.German },
            new ComboBoxViewModel { Id = 3, Content = Properties.Resources.Italian, Property = ELanguage.Italian },
            new ComboBoxViewModel { Id = 4, Content = Properties.Resources.Spanish, Property = ELanguage.Spanish },
            new ComboBoxViewModel { Id = 5, Content = Properties.Resources.SpanishLatin, Property = ELanguage.SpanishLatin },
            new ComboBoxViewModel { Id = 17, Content = Properties.Resources.SpanishMexico, Property = ELanguage.SpanishMexico },
            new ComboBoxViewModel { Id = 6, Content = Properties.Resources.Arabic, Property = ELanguage.Arabic },
            new ComboBoxViewModel { Id = 7, Content = Properties.Resources.Japanese, Property = ELanguage.Japanese },
            new ComboBoxViewModel { Id = 8, Content = Properties.Resources.Korean, Property = ELanguage.Korean },
            new ComboBoxViewModel { Id = 9, Content = Properties.Resources.Polish, Property = ELanguage.Polish },
            new ComboBoxViewModel { Id = 10, Content = Properties.Resources.PortugueseBrazil, Property = ELanguage.PortugueseBrazil },
            new ComboBoxViewModel { Id = 18, Content = Properties.Resources.PortuguesePortugal, Property = ELanguage.PortuguesePortugal },
            new ComboBoxViewModel { Id = 11, Content = Properties.Resources.Russian, Property = ELanguage.Russian },
            new ComboBoxViewModel { Id = 12, Content = Properties.Resources.Turkish, Property = ELanguage.Turkish },
            new ComboBoxViewModel { Id = 13, Content = Properties.Resources.Chinese, Property = ELanguage.Chinese },
            new ComboBoxViewModel { Id = 14, Content = Properties.Resources.TraditionalChinese, Property = ELanguage.TraditionalChinese },
            new ComboBoxViewModel { Id = 19, Content = Properties.Resources.Swedish, Property = ELanguage.Swedish },
            new ComboBoxViewModel { Id = 20, Content = Properties.Resources.Thai, Property = ELanguage.Thai },
            new ComboBoxViewModel { Id = 21, Content = Properties.Resources.Indonesian, Property = ELanguage.Indonesian },
            new ComboBoxViewModel { Id = 22, Content = Properties.Resources.VietnameseVietnam, Property = ELanguage.VietnameseVietnam }
        };

        public static ObservableCollection<ComboBoxViewModel> jsonCbViewModel = new ObservableCollection<ComboBoxViewModel>
        {
            new ComboBoxViewModel { Id = 0, Content = Properties.Resources.Default, Property = EJsonType.Default },
            new ComboBoxViewModel { Id = 1, Content = Properties.Resources.WithPosition, Property = EJsonType.Positioned }
        };

        public static ObservableCollection<ComboBoxViewModel> designCbViewModel = new ObservableCollection<ComboBoxViewModel>
        {
            new ComboBoxViewModel { Id = 0, Content = Properties.Resources.Default, Property = EIconDesign.Default },
            new ComboBoxViewModel { Id = 1, Content = Properties.Resources.NoBackground, Property = EIconDesign.NoBackground },
            new ComboBoxViewModel { Id = 2, Content = Properties.Resources.NoText, Property = EIconDesign.NoText },
            new ComboBoxViewModel { Id = 3, Content = Properties.Resources.Minimalist, Property = EIconDesign.Mini },
            new ComboBoxViewModel { Id = 4, Content = Properties.Resources.Flat, Property = EIconDesign.Flat }
        };

        public static ObservableCollection<ComboBoxViewModel> gamesCbViewModel = new ObservableCollection<ComboBoxViewModel>();
    }

    public class ComboBoxViewModel : PropertyChangedBase
    {
        private string _content;
        public string Content
        {
            get { return _content; }

            set { this.SetProperty(ref this._content, value); }
        }

        private int _id;
        public int Id
        {
            get { return _id; }

            set { this.SetProperty(ref this._id, value); }
        }

        private object _property;
        public object Property
        {
            get { return _property; }

            set { this.SetProperty(ref this._property, value); }
        }
    }
}
