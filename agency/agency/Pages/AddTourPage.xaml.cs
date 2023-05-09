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
    /// Логика взаимодействия для AddTourPage.xaml
    /// </summary>
    public partial class AddTourPage : Page
    {
        private Entities.Tour _currentTour = null;
        private byte[] _mainImageData;
        public AddTourPage()
        {
            InitializeComponent();
        }
        //подгрузка страницы и заполнение её объектов соответствующими данными из БД
        public AddTourPage(Entities.Tour tour)
        {
            InitializeComponent();

            _currentTour = tour;
            Title = "Добавление тура";
            TBoxTitle.Text = _currentTour.Title;
            TBoxCost.Text = _currentTour.Cost.ToString("N2");
            TBoxDuration.Text = (_currentTour.DaysOfStay / 24).ToString();
            if (_currentTour.Discount > 0)
                TBoxDiscount.Text = (_currentTour.Discount.Value * 100).ToString();
            if (_currentTour.MainImage != null)
                ImageTours.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_currentTour.MainImage);
        }
        //Метод, в котором указаны условия для проверки введенных данных при добавлении тура
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TBoxTitle.Text))
                errorBuilder.AppendLine("Название тура обязательно для заполнения;");

            var serviceFromDB = App.Context.Tours.ToList().FirstOrDefault(p => p.Title.ToLower() == TBoxTitle.Text.ToLower());

            if (serviceFromDB != null && serviceFromDB != _currentTour)
                errorBuilder.AppendLine("Такой тур уже есть в базе данных;");

            decimal cost = 0;
            if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost <= 0)
            {
                errorBuilder.AppendLine("Стоимость тура должна быть положительным числом;");
            }
            int duractionInHours = 0;
            if (int.TryParse(TBoxDuration.Text, out duractionInHours) == false || duractionInHours > 15 || duractionInHours <= 0)
            {
                errorBuilder.AppendLine("Длительность тура должна быть положительным " + "числом (не больше, чем 15 дней);");
            }
            if (!string.IsNullOrEmpty(TBoxDiscount.Text))
            {
                int discount = 0;
                if (int.TryParse(TBoxDiscount.Text, out discount) == false || discount < 0 || discount > 100)
                {
                    errorBuilder.AppendLine("Размер скидки - целое число в диапазоне от 0 до 100%");
                }
            }
            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки: \n");
            return errorBuilder.ToString();
        }
        /// <summary>
        /// Обработчик события кнопки "Добавить", в котором есть обращение к методу проверки данных, 
        /// а также заполнение введенных данных в определённые поля БД
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (_currentTour == null)
                {
                    var service = new Entities.Tour
                    {
                        Title = TBoxTitle.Text,
                        Cost = decimal.Parse(TBoxCost.Text),
                        DaysOfStay = int.Parse(TBoxDuration.Text) * 24,
                        Description = TBoxDescription.Text,
                        Discount = string.IsNullOrWhiteSpace(TBoxDiscount.Text)
                    ? 0 : double.Parse(TBoxDiscount.Text) / 100,
                        MainImage = _mainImageData
                    };
                    App.Context.Tours.Add(service);
                    App.Context.SaveChanges();
                }
                else
                {
                    _currentTour.Title = TBoxTitle.Text;
                    _currentTour.Cost = decimal.Parse(TBoxCost.Text);
                    _currentTour.DaysOfStay = int.Parse(TBoxDuration.Text) * 24;
                    _currentTour.Description = TBoxDescription.Text;
                    _currentTour.Discount = string.IsNullOrWhiteSpace(TBoxDiscount.Text)
                    ? 0 : double.Parse(TBoxDiscount.Text) / 100;
                    if (_mainImageData != null)
                        _currentTour.MainImage = _mainImageData;
                    App.Context.SaveChanges();
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
                ImageTours.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
            }
        }


    }
}
