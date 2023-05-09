using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace agency
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.ISP31Entities Context
        { get; } = new Entities.ISP31Entities();

        public static Entities.User Currentuser = null;
    }
}
