using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CEN207_Assessment_2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string localDisplayName = "";

        public static string getDisplayName()
        {
            return localDisplayName;
        }

        public static void setDisplayName(string value)
        {
            localDisplayName = value;
        }

    }
}
