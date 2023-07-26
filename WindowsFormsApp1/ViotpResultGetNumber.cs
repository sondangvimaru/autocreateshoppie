using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ViotpResultGetNumber
    {
        private string phone_number;
        private string re_phone_number;
        private string countryISO;
        private string countryCode;
        private string request_id;
        private string balance;


        public string Phone_number { get => phone_number; set => phone_number = value; }
        public string Re_phone_number { get => re_phone_number; set => re_phone_number = value; }
        public string CountryISO { get => countryISO; set => countryISO = value; }
        public string CountryCode { get => countryCode; set => countryCode = value; }
        public string Request_id { get => request_id; set => request_id = value; }
        public string Balance { get => balance; set => balance = value; }
    }
}
