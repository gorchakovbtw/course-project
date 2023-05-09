using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace agency.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик события для кнопки "Авторизация" страницы LoginPage, в этом методе проверяется правильность введеных данных,
        /// </summary  
        public void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentUser = App.Context.Users.FirstOrDefault(p => p.Login == TBoxLogin.Text && p.Password == pBoxPassword.Password);
                //проверка введёных данных на правильность
                if (currentUser == null)
                {
                        MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;                  
                }

                if (currentUser != null)
                {
                    if (currentUser.Login != TBoxLogin.Text || currentUser.Password != pBoxPassword.Password)
                    {
                        MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        App.Currentuser = currentUser;
                        NavigationService.Navigate(new SelectPages());
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к базе данных!", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        //Переход на страницу "Регистрация"
        public void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
       
    }
}
