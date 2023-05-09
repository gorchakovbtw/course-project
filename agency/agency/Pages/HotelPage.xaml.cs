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
    /// Логика взаимодействия для HotelPage.xaml
    /// </summary>
    public partial class HotelPage : Page
    {
        public HotelPage()
        {
            InitializeComponent();
            
            LViewServices.ItemsSource = App.Context.Hotels.ToList();
            ComboCity.SelectedIndex = 0;

            UpdateHotels();
        }
        /// <summary>
        /// Метод для кнопки "Узнать", при нажатии на которую происходит навигация на страницу информации об отеле
        /// </summary>
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            var currentHotels = (sender as Button).DataContext as Entities.Hotel;
            NavigationService.Navigate(new AddEditTourPage(currentHotels));
        }
        
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHotels();
        }
        //Метод сортировки отелей по определённым параметрам
        private void UpdateHotels()
        {
            var hotels = App.Context.Hotels.ToList();
            //Сортировка по городу
            if (ComboCity.SelectedIndex == 1)
                hotels = hotels.Where(p => p.HotelCity == "Великий Устюг").ToList();
            if (ComboCity.SelectedIndex == 2)
                hotels = hotels.Where(p => p.HotelCity == "Москва").ToList();
            if (ComboCity.SelectedIndex == 3)
                hotels = hotels.Where(p => p.HotelCity == "Екатеренбург").ToList();
            if (ComboCity.SelectedIndex == 4)
                hotels = hotels.Where(p => p.HotelCity == "Нижний Тагил").ToList();
            if (ComboCity.SelectedIndex == 5)
                hotels = hotels.Where(p => p.HotelCity == "Новосибирск").ToList();
            if (ComboCity.SelectedIndex == 6)
                hotels = hotels.Where(p => p.HotelCity == "Красноярск").ToList();
            if (ComboCity.SelectedIndex == 7)
                hotels = hotels.Where(p => p.HotelCity == "Сочи").ToList();
            if (ComboCity.SelectedIndex == 8)
                hotels = hotels.Where(p => p.HotelCity == "Геленджик").ToList();
            if (ComboCity.SelectedIndex == 9)
                hotels = hotels.Where(p => p.HotelCity == "Астрахань").ToList();

            //Поиск по названию(регистронезависимый)
            hotels = hotels.Where(p => p.HotelTitle.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewServices.ItemsSource = hotels;      
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateHotels();
        }
        private void ComboCity_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateHotels();
        }
        private void BtnEdit_Click (object sender, RoutedEventArgs e)
        {
            var currentHotels = (sender as Button).DataContext as Entities.Hotel;
            NavigationService.Navigate(new AddEditTourPage(currentHotels));
        }
        private void BtnDelete_Click (object sender, RoutedEventArgs e)
        {
            var currentHotel = (sender as Button).DataContext as Entities.Hotel;

            if (MessageBox.Show($"Вы уверены, что хотите удалить отель: " + $"{currentHotel.HotelTitle}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            { 
                App.Context.Hotels.Remove(currentHotel);
                App.Context.SaveChanges();
                UpdateHotels();
            }
        }
    }
}
