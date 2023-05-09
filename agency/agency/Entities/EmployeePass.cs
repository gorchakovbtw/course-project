using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agency.Entities
{
    public partial class Employee
    {
        public string GetFI0
        {
            get
            {
                return $"{Surname} {Name} {Patronymic}";
            }
        }
        public string GetTip
        {
            get
            {
                return $"Должность: {Post.PostTitle}";
            }
        }
        public string GetPol
        {
            get
            {
                return $"Пол: {Gender.GenderTitle}";
            }
        }
        public string GetPh
        {
            get
            {
                return $"Телефон: {Phone}";
            }
        }
        public string GetSal
        {
            get
            {
                return $"Зарплата: {Salary:N2} руб.";
            }
        }
        public string BackColor
        {
            get
            {
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
    }
}
