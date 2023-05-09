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
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        public EmployeePage()
        {
            InitializeComponent();
            if (App.Currentuser.Roleld == 1)
            {
                AddEmployee.Visibility = Visibility.Visible;
                PaymentCalculation.Visibility = Visibility.Visible;
            }
            else
            {
                AddEmployee.Visibility = Visibility.Collapsed;
                PaymentCalculation.Visibility = Visibility.Collapsed;
            }
            
            EmployyesList.ItemsSource = App.Context.Employees.ToList();
            EmployyesList.ItemsSource = App.Context.Posts.ToList();
            ComboPost.SelectedIndex = 0;
            ComboSortBy.SelectedIndex = 0;
            UpdatePosts();
        }
        //переход на другую страницу
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployee());
        }
        private void Page_Loaded(Object sender, RoutedEventArgs e)
        {
            UpdatePosts();
            
        }
        //Метод сортировки сотрудников по определённым свойствам
        private void UpdatePosts()
        {
            var posts = App.Context.Posts.ToList();
            var employees = App.Context.Employees.ToList();

            //Сортировка по заработной плате
            if (ComboSortBy.SelectedIndex == 0)
                employees = employees.OrderBy(p => p.Salary).ToList();
            else
                employees = employees.OrderByDescending(p => p.Salary).ToList();

            // Сортировка по должности
            if (ComboPost.SelectedIndex == 1)
                employees = employees.Where(p => p.PostID == 2).ToList();
            if (ComboPost.SelectedIndex == 2)
                employees = employees.Where(p => p.PostID == 3).ToList();
            if (ComboPost.SelectedIndex == 3)
                employees = employees.Where(p => p.PostID == 4).ToList();
            if (ComboPost.SelectedIndex == 4)
                employees = employees.Where(p => p.PostID == 5).ToList();
            if (ComboPost.SelectedIndex == 5)
                employees = employees.Where(p => p.PostID == 6).ToList();
            if (ComboPost.SelectedIndex == 6)
                employees = employees.Where(p => p.PostID == 7).ToList();
            if (ComboPost.SelectedIndex == 7)
                employees = employees.Where(p => p.PostID == 8).ToList();
           
            //Поиск по ФИО
            employees = employees.Where(p => p.Surname.ToLower().Contains(TBoxSearch.Text.ToLower()) 
            || p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            EmployyesList.ItemsSource = employees;
        }
        //Обработчик события для ComboBox-а для сортировки сотрудников по ЗП
        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePosts();
        }
        //Обработчик события для ComboBox-а для сортировки сотрудников по должности
        private void ComboPost_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdatePosts();
        }
        //Обработчик события для TextBox-а для поиска сотрудника по ФИО
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePosts();
        }
        //Метод удаления сотрудника из базы данных
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentEmployee = (sender as Button).DataContext as Entities.Employee;

            if (MessageBox.Show($"Вы уверены, что хотите удалить сотрудника: " + $"{currentEmployee.Surname}" + $" {currentEmployee.Name}" + $" {currentEmployee.Patronymic}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Employees.Remove(currentEmployee);
                App.Context.SaveChanges();
                UpdatePosts();
            }
        }
        //Метод кнопки "Изменить" для перехода на страницу изменения сотрудника
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentEmployees = (sender as Button).DataContext as Entities.Employee;
            NavigationService.Navigate(new AddEditEmployeePage(currentEmployees));
        }
        //Метод кнопки "Расчет аванса" для перехода на страницу расчета аванса сотрудника
        private void PaymentCalculation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaymentCalculationPage());
        }

        

    }
}

