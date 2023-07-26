using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class OcrIdCardResult
    {
        public string address { get; set; }
        public string name_card { get; set; }
        public string id_card { get; set; }
        public DateTime birthday_date { get; set; }
        public string gender { get; set; }
    }
}
