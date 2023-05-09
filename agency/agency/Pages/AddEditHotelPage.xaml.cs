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
    /// Логика взаимодействия для AddEditTourPage.xaml
    /// </summary>
    public partial class AddEditTourPage : Page
    {
        private Entities.Hotel _currentHotel = null;
        private byte[] _mainImageData;
        public AddEditTourPage()
        {
            InitializeComponent();
            
        }
        //подгрузка страницы и заполнение её объектов соответствующими данными из БД
        public AddEditTourPage(Entities.Hotel hotel)
        {
            InitializeComponent();
            if (App.Currentuser.Roleld == 1)
            {
                TBoxTitle.IsReadOnly = false;
                TBoxCity.IsReadOnly = false;
                TBoxDescription.IsReadOnly = false;  
                BtnSelectImage.Visibility = Visibility.Visible;
                BtnSave.Visibility = Visibility.Visible;
            }
            else
            {
                TBoxTitle.IsReadOnly = true;
                TBoxCity.IsReadOnly = true;
                TBoxDescription.IsReadOnly = true;
                BtnSelectImage.Visibility = Visibility.Collapsed;
                BtnSave.Visibility = Visibility.Collapsed;
            }
            _currentHotel = hotel;
            Title = "Информация о отеле";
            TBoxTitle.Text = _currentHotel.HotelTitle;
            TBoxCity.Text = _currentHotel.HotelCity;
            TBoxDescription.Text = _currentHotel.DescriptionHotel;
            if (_currentHotel.PhotoHotel != null)
                ImageHotels.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_currentHotel.PhotoHotel);  
        }
        private void BtnReserve_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxTitle.Text))
                errorBuilder.AppendLine("Название отеля обязательно для заполнения;");

            var serviceFromDB = App.Context.Hotels.ToList().FirstOrDefault(p => p.HotelTitle.ToLower() == TBoxTitle.Text.ToLower());

            if (serviceFromDB != null && serviceFromDB != _currentHotel)
                errorBuilder.AppendLine("Такой отель уже есть в базе данных;");

            if (string.IsNullOrWhiteSpace(TBoxCity.Text))
                errorBuilder.AppendLine("Укажите город, в котором находится отель");

            if (errorBuilder.Length > 0)

                errorBuilder.Insert(0, "Устраните следующие ошибки: \n");

            return errorBuilder.ToString();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (_currentHotel == null)
                {
                    var hotel = new Entities.Hotel
                    {
                        HotelTitle = TBoxTitle.Text,
                        HotelCity = TBoxCity.Text,
                        DescriptionHotel = TBoxDescription.Text,
                        PhotoHotel = _mainImageData
                    };
                    App.Context.Hotels.Add(hotel);
                    App.Context.SaveChanges();
                }
                else
                {
                    _currentHotel.HotelTitle = TBoxTitle.Text;
                    _currentHotel.HotelCity = TBoxCity.Text;
                    _currentHotel.DescriptionHotel = TBoxDescription.Text;
                    if (_mainImageData != null)
                        _currentHotel.PhotoHotel = _mainImageData;
                    App.Context.SaveChanges();
                    MessageBox.Show("Информация об отеле успешно изменена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                NavigationService.GoBack();
            }
        }
        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageHotels.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
            }
        }
    }
}
