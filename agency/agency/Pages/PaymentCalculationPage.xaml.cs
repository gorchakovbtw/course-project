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
using HourlyPayrollLib;

namespace agency.Pages
{
    /// <summary>
    /// Логика взаимодействия для PaymentCalculationPage.xaml
    /// </summary>
    public partial class PaymentCalculationPage : Page
    {
        public PaymentCalculationPage()
        {
            InitializeComponent();
            ComboPost.ItemsSource = App.Context.Posts.Select(c => c.PostTitle).ToList();
        }
        //Обработчик события кнопки "Рассчитать" аванс сотрудника, в котором отображается вывод определённой информации в объекты страницы
        private void ButCalculation_Click(object sender, RoutedEventArgs e)
        {
            var salaryPost = App.Context.Posts.Where(p => p.PostTitle == ComboPost.Text)
                .Select(p => p.PostWorkHour).FirstOrDefault();
            var hour = HourlyPayroll.SalaryMonth((decimal)salaryPost, Convert.ToInt32(BoxHour.Text));
            MessageBox.Show(hour.ToString(), "Рассчёт выполнен", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void BoxHour_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

    }
}
