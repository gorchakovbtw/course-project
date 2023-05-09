using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddEditEmployeePage.xaml
    /// </summary>
    public partial class AddEditEmployeePage : Page
    {
        private Entities.Employee _currentEmployee = null;   
        private byte[] _mainImageData;

        public AddEditEmployeePage()
        {
            InitializeComponent();       
        }
        //подгрузка страницы и заполнение её объектов соответствующими данными из БД
        public AddEditEmployeePage(Entities.Employee employee)
        {           
            InitializeComponent();
            this.TboxPhone.PreviewTextInput += new TextCompositionEventHandler(PhoneBox_PreviewTextInput);
            this.BoxSalary.PreviewTextInput += new TextCompositionEventHandler(TBoxSalary_PreviewTextInput);
            if (App.Currentuser.Roleld == 1)
            {    
                BoxIma.IsReadOnly = true;
                Boxotchestvo.IsReadOnly = true;
            }
            _currentEmployee = employee;
            Title = "Редактирование сотрудника";
            ComboTypePost.ItemsSource = App.Context.Posts.Select(c => c.PostTitle).ToList();
            ComboTypePost.SelectedItem = App.Context.Posts.Find(_currentEmployee.PostID).PostTitle;
            BoxFamilia.Text = _currentEmployee.Surname;
            BoxIma.Text = _currentEmployee.Name;
            Boxotchestvo.Text = _currentEmployee.Patronymic;
            TboxPhone.Text = _currentEmployee.Phone;
            if (_currentEmployee.GenderTitleID == 2)
            {
                TboxType.Text = "женский";
            }
            else
            {
                TboxType.Text = "мужской";
            }

            if (_currentEmployee.ImageEmp != null)
            {
                ImageEmployees.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_currentEmployee.ImageEmp);
            }
            BoxSalary.Text = _currentEmployee.Salary.ToString("N2");         
            var idPost = App.Context.Posts.Where(c => c.PostTitle == ComboTypePost.Text)
                    .Select(c => c.ID).FirstOrDefault();       
            _currentEmployee.PostID = idPost;
        }
        //Метод для проверки TextBox-a на наличие буквенных символов
        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void TBoxSalary_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
        //Метод, в котором указаны условия для проверки введенных данных при редактировании информации о сотруднике
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

        int genderID = 0;
        /// <summary>
        /// Обработчик события кнопки "Сохранить", в котором есть обращение к методу проверки данных, 
        /// а также редактирование данных в имеющихся объектах страницы
        /// </summary>
        private void ButtonSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //проверка на заполненность информации о сотруднике
                if (_currentEmployee == null)
                {
                    switch (TboxType.Text)
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
                }
                else
                { 
                    var idPost = App.Context.Posts.Where(c => c.PostTitle == ComboTypePost.Text)
                    .Select(c => c.ID).FirstOrDefault();

                    _currentEmployee.Surname = BoxFamilia.Text;
                    _currentEmployee.Name = BoxIma.Text;
                    _currentEmployee.Patronymic = Boxotchestvo.Text;
                    _currentEmployee.Gender.GenderTitle = TboxType.Text;
                    _currentEmployee.PostID = App.Context.Posts.FirstOrDefault(c => c.PostTitle == ComboTypePost.SelectedItem.ToString()).ID;
                    _currentEmployee.Phone = TboxPhone.Text;
                    _currentEmployee.Salary = decimal.Parse(BoxSalary.Text);
                    if (_mainImageData != null)
                        _currentEmployee.ImageEmp = _mainImageData;
                    App.Context.SaveChanges();
                    MessageBox.Show("Персональная информация о сотруднике изменена");       
                }
                NavigationService.GoBack();                
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
                ImageEmployees.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
            }
        }
    }
}
