using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace agency.Entities
{
    public partial class Tour
    {
        public string DiscountText
        {
            get
            {
                if(Discount == 0 || Discount == null)
                    return "";
                else
                    return $"* скидка {Discount * 100} %";
            }
        }
        public double CostWithDiscount
        {
            get
            {
                if (Discount == 0 || Discount == null)
                {
                    return (double)Cost;
                }
                else
                {
                    var costWithDiscount = (double)Cost * (1.00 - Discount);
                    return costWithDiscount.Value;
                }
            }
        }
        public string TotalCost
        {
            get
            {
                if (Discount == 0 || Discount == null)
                {
                    return $" {Cost:N2} рублей   ({DaysOfStay / 24} дн.)";

                }
                else
                {
                    return $" {CostWithDiscount:N2} рублей  ({DaysOfStay / 24} дн.) ";
                }
            }
        }
        public Visibility DiscountVisibility
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        public string BackColor
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return "#FFFFF0F5";
                else
                    return "Azure";
            }
        }
        public string AdminControlsVisibility
        {
            get
            {
                if (App.Currentuser.Roleld == 1)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
        }
        public string ConrolVisibilityToTour
        {
            get
            {
                if (App.Currentuser.Roleld == 2)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
        }
        public string Print()
        {

            return $"Название тура: {Title} \nСтоимость: {CostWithDiscount} руб. со скидкой в {Discount * 100}% \nПродолжительность: {DaysOfStay / 24} дней ";

        }
    }
}
