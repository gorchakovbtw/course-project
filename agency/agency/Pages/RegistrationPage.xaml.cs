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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// Страница Регистрации пользователя
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод кнопки "Регистрация". Он отражает специальные ограничения под которыми пользователь может зарегистрироваться
        /// для определённых объектов страницы
        /// </summary>
        public void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            //проверка на повторяющийся логин
            if (App.Context.Users.Count(p => p.Login == TBoxLogin.Text) > 0)
            {
                MessageBox.Show("Пользователь с таким логином есть! Придумайте новый", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //проверка на правильность введенных паролей
            if (PBoxPassword.Password != PBoxrepeatPassword.Password)
            {                
                MessageBox.Show("Пароли не совпадают", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;           
            }
            //условие длины пароля
            if (PBoxrepeatPassword.Password.Length >= 6)
            {               
                bool symbol = false;
                bool num = false;
                for (int i = 0; i < PBoxrepeatPassword.Password.Length; i++)
                {
                  
                    if (PBoxrepeatPassword.Password[i] >= '0' && PBoxrepeatPassword.Password[i] <= '9') num = true;
                    if (PBoxrepeatPassword.Password[i] == '_' || PBoxrepeatPassword.Password[i] == '-' || PBoxrepeatPassword.Password[i] == '@') symbol = true;
                }
                //условие для содержания в пароле специального символа
                if (!symbol)
                {
                    MessageBox.Show("Добавьте один из следующих символов: _ - @", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //условие для содержания в пароле хотябы одной цифры
                else if (!num)
                {
                    MessageBox.Show("Добавьте хотя бы одну цифру", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                //проверка вышеуказанных условий
                if (symbol && num)
                {
                    try
                    {
                        Entities.User user = new Entities.User()
                        {

                            Login = TBoxLogin.Text,
                            Password = PBoxrepeatPassword.Password,
                            Roleld = 2

                        };
                        App.Context.Users.Add(user);
                        App.Context.SaveChanges();
                        MessageBox.Show("Пользователь зарегистрирован!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при регистрации! Проверьте правильность введённых данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }              
            }
            else
            {
                MessageBox.Show("Пароль слишком короткий, минимум 6 символов", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //при успешной регистрации выполнен переход на страницу авторизации
            NavigationService.Navigate(new LoginPage());
        }
    }
}
