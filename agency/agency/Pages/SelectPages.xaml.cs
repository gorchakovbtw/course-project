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
    /// Логика взаимодействия для SelectPages.xaml
    /// Страница с выбором разделов
    /// </summary>
    public partial class SelectPages : Page
    {
        public SelectPages()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Данный обработчик кнопки "Туры" используется для навигации на страницу TourPage
        /// </summary>
        private void BtnSelectTour_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TourPage());
        }
        /// <summary>
        /// Данный обработчик кнопки "Сотрудники" спользуется для навигации на страницу EmployeePage
        /// </summary>
        private void BtnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeePage());
        }

        private void BtnSelectHotel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HotelPage());
        }
    }
}
