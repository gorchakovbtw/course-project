using agency.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Page

    {
        private Entities.Employee _currentEmployee = null;
        private byte[] _mainImageData;
        public AddEmployee()
        {
            InitializeComponent();
            this.TboxPhone.PreviewTextInput += new TextCompositionEventHandler(PhoneBox_PreviewTextInput);
            this.BoxSalary.PreviewTextInput += new TextCompositionEventHandler(TBoxSalary_PreviewTextInput);
            ComboType.ItemsSource = App.Context.Genders.Select(c => c.GenderTitle).ToList();
            ComboTypePost.ItemsSource = App.Context.Posts.Select(c => c.PostTitle).ToList();
        }
        //Метод, в котором указаны условия для проверки введенных данных при добавлении сотрудника
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(BoxFamilia.Text))
                errorBuilder.AppendLine("Поле Фамилия обязательно для заполнения;");

            if (string.IsNullOrWhiteSpace(BoxIma.Text))
                errorBuilder.AppendLine("Поле Имя обязательно для заполнения;");

            if (string.IsNullOrWhiteSpace(TboxPhone.Text))
                errorBuilder.AppendLine("Поле Телефон обязательно для заполнения;");

            var surnameFromDB = App.Context.Employees.ToList().FirstOrDefault(p => p.Surname.ToLower() == BoxFamilia.Text.ToLower());
            var nameFromDB = App.Context.Employees.ToList().FirstOrDefault(p => p.Name.ToLower() == BoxIma.Text.ToLower());
            var phoneFromDB = App.Context.Employees.ToList().FirstOrDefault(p => p.Phone.ToLower() == TboxPhone.Text.ToLower());

            if ((surnameFromDB != null) && (surnameFromDB != _currentEmployee) && (nameFromDB != null) && (nameFromDB != _currentEmployee) && (phoneFromDB != null) && (phoneFromDB != _currentEmployee))
                errorBuilder.AppendLine("Такой сотрудник уже есть");

            if (string.IsNullOrWhiteSpace(ComboType.Text))
                errorBuilder.AppendLine("Заполните поле Пол;");

            if (string.IsNullOrWhiteSpace(ComboTypePost.Text))
                errorBuilder.AppendLine("Укажите Должность сотрудника;");

            if (string.IsNullOrWhiteSpace(BoxSalary.Text))
                errorBuilder.AppendLine("Поле Зарплата обязательно для заполнения;");

            decimal salary = 0;
            if (decimal.TryParse(BoxSalary.Text, out salary) == false || salary <= 0)
            {
                errorBuilder.AppendLine("Зарплата сотрудника должна быть положительным числом;");
            }

            if (errorBuilder.Length > 0)

                errorBuilder.Insert(0, "Устраните следующие ошибки: \n");

            return errorBuilder.ToString();
        }
        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void TBoxSalary_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
        int genderID = 0;
        /// <summary>
        /// Обработчик события кнопки "Добавить", в котором есть обращение к методу проверки данных, 
        /// а также заполнение введенных данных в определённые поля БД
        /// </summary>
        private void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (_currentEmployee == null)
                {
                    switch (ComboType.Text)
                    {
                        case "женский":
                            genderID = 2;
                            break;
                        case "мужской":
                            genderID = 1;
                            break;
                    }
                    var idPost = App.Context.Posts.Where(c => c.PostTitle == ComboTypePost.Text)
                .Select(c => c.ID).FirstOrDefault();
                    var employee = new Entities.Employee
                    {
                        Surname = BoxFamilia.Text,
                        Name = BoxIma.Text,
                        Patronymic = Boxotchestvo.Text,
                        Phone = TboxPhone.Text,
                        GenderTitleID = genderID,
                        PostID = idPost,
                        Salary = decimal.Parse(BoxSalary.Text),
                        ImageEmp = _mainImageData
                    };
                    App.Context.Employees.Add(employee);
                    App.Context.SaveChanges();
                    MessageBox.Show("Сотрудник добавлен");
                    NavigationService.GoBack();
                }
            }

        }
        /// <summary>
        /// Метод выбора картинки тура определённого расширения, а так же конвертирование её в байтовый вид
        /// </summary>
        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageTours.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
            }
        }


    }
}
