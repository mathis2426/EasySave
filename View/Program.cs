// See https://aka.ms/new-console-template for more information
using System;
using Enumerations;
using Languages;

class Program
{
    static void Main()
    {
        Language myLang = new Language();
        Console.WriteLine("Langue actuelle : " + myLang.language);
        Console.WriteLine("Veuillez entrer la langue souhaitée (fr, en) : ");
        string input = Console.ReadLine()?.Trim().ToLower();

        if (Enum.TryParse(typeof(EnumLanguage), input, true, out var result))
        {
            myLang.changeLanguage((EnumLanguage)result);
            Console.WriteLine($"Langue après changement : " + myLang.language);
        }
        else
        {
            Console.WriteLine("Langue non reconnue. Langue inchangée.");
        }

    }
}
