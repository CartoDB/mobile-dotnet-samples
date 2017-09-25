using System;
using System.Collections.Generic;

namespace Shared.Model
{
    public class Languages
    {
        public static List<Language> List = new List<Language>
        {
            new Language("Local", ""),
            new Language("English", "en"),
            new Language("German", "de"),
            new Language("Spanish", "es"),                
            new Language("Italian", "it"),
            new Language("French", "fr"),
            new Language("Russian", "ru")
        };
    }

    public class Language
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public Language(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
