using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agency.Entities
{
    public partial class Hotel
    {
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
        public string ConrolVisibilityToHotel
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

    }
}
