using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace ASPNET_Project_Eleven.Utilities
{
    public class ValidationEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidationEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }

    public static class Extensions
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();


        }
    }

    public enum Season
    {
        [Display(Name = "It's autumn")]
        Autumn,

        [Display(Name = "It's winter")]
        Winter,

        [Display(Name = "It's spring")]
        Spring,

        [Display(Name = "It's summer")]
        Summer
    }

    public class Foo
    {
        public Season Season = Season.Summer;

        public void DisplayName()
        {
            var seasonDisplayName = Season.GetAttribute<DisplayAttribute>();
            Console.WriteLine("Which season is it?");
            Console.WriteLine(seasonDisplayName.Name);
        }
    }

    public static class Logger
    {
        public static void WriteLog(string str)
        {
            string currentDirectory = System.Environment.CurrentDirectory.ToString();
            string outputFileInfo = currentDirectory + @"/LogFile.txt";

            StreamWriter textWriter = new StreamWriter(new FileStream(outputFileInfo, FileMode.Append, FileAccess.Write, FileShare.None));
            textWriter.WriteLine(str);
            textWriter.Flush();
            textWriter.Close();
        }
    }
}
