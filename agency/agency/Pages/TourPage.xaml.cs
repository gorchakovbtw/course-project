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
using agency.Entities;

namespace agency.Pages
{
    /// <summary>
    /// Логика взаимодействия для TourPage.xaml
    /// </summary>
    public partial class TourPage : Page
    {
        //private List<string> _peopleForPrint;
        
        public TourPage()
        {
            InitializeComponent();
            if (App.Currentuser.Roleld == 1)
            {
                BtnAddTour.Visibility = Visibility.Visible;
            }
            else
            {
                BtnAddTour.Visibility = Visibility.Collapsed;
            }
            LViewTours.ItemsSource = App.Context.Tours.ToList();
            ComboDiscount.SelectedIndex = 0;
            ComboSortBy.SelectedIndex = 0;
            UpdateTours();


        }
        /// <summary>
        /// Метод для кнопки "Изменить", который выполняет функцию навигации на другую страницу
        /// </summary> 
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentTours = (sender as Button).DataContext as Entities.Tour;
            NavigationService.Navigate(new AddEditPage(currentTours));
        }
        /// <summary>
        /// Обработчик события кнопки "Удалить" для возможности удаления актуального тура
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentTour = (sender as Button).DataContext as Entities.Tour;

            if (MessageBox.Show($"Вы уверены, что хотите удалить тур: " + $"{currentTour.Title}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Tours.Remove(currentTour);
                App.Context.SaveChanges();
                UpdateTours();
            }
        }
        private void BtnAddTour_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTourPage());
        }
        private void ComboDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }
        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }
        /// <summary>
        /// Метод, выполненный для сортироваки и фильтрации туров по ценам, размеру скидки, а так же есть возможность поиска по названию тура
        /// </summary>
        private void UpdateTours()
        {
            var tours = App.Context.Tours.ToList();
            //Сортировка по цене
            if (ComboSortBy.SelectedIndex == 0)
                tours = tours.OrderBy(p => p.CostWithDiscount).ToList();
            else
                tours = tours.OrderByDescending(p => p.CostWithDiscount).ToList();
            //Фильтрация по размеру скидки
            if (ComboDiscount.SelectedIndex == 1)
                tours = tours.Where(p => p.Discount >= 0 && p.Discount < 0.05).ToList();
            if (ComboDiscount.SelectedIndex == 2)
                tours = tours.Where(p => p.Discount >= 0.05 && p.Discount < 0.15).ToList();
            if (ComboDiscount.SelectedIndex == 3)
                tours = tours.Where(p => p.Discount >= 0.15 && p.Discount < 0.30).ToList();
            if (ComboDiscount.SelectedIndex == 4)
                tours = tours.Where(p => p.Discount >= 0.30 && p.Discount < 0.70).ToList();
            if (ComboDiscount.SelectedIndex == 5)
                tours = tours.Where(p => p.Discount >= 0.70 && p.Discount < 1).ToList();

            //Поиск по названию(регистронезависимый)
            tours = tours.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewTours.ItemsSource = tours;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }
        /// <summary>
        /// Обработчик события для кнопки "Подробнее", кнопка отображается только для пользователей.
        /// С помощью данного метода выполнен переход на следующую страницу
        /// </summary>
        private void BtnMore_Click(object sender, RoutedEventArgs e)
        {
            var currentTours = (sender as Button).DataContext as Entities.Tour;
            NavigationService.Navigate(new AddEditPage(currentTours));
        }
        /// <summary>
        /// Метод, который выполняет функцию печати основной страницы всех актуальных туров
        /// В нём указаны параметры печати
        /// </summary>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument fd = new FlowDocument();
            fd.FontFamily = new FontFamily("Times New Roman");
            fd.FontSize = 14;
            fd.ColumnWidth = 600;
            fd.Blocks.Add(new Paragraph(new Run("Список туров\v")));
            foreach (var item in LViewTours.Items)
            {
                fd.Blocks.Add(new Paragraph(new Run((item as Tour).Print()))); 
            }
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;
            fd.PageHeight = pd.PrintableAreaHeight;
            fd.PageWidth = pd.PrintableAreaWidth;
            IDocumentPaginatorSource idocument = fd as IDocumentPaginatorSource;

            pd.PrintDocument(idocument.DocumentPaginator, "Печать документа!!!");
        }
    }
}

    

