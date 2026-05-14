using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HLVTimeSheet.AcsessData
{
    public class FormatString
    {
        public static string removeComma(string str)
        {
            string result = Regex.Replace(str, ",", "");
            return result;
        }

        public static string removePercent(string str)
        {
            string result = Regex.Replace(str, "%", "");
            return result;
        }

        public static bool checkSQLInjection(string str)
        {

            if (str.ToUpper().Contains("OR ") || str.ToUpper().Contains("OR ") || str.ToUpper().Contains("'") || str.ToUpper().Contains("'"))
                return false;
            return true;
        }

        public static string ForMatNumber(string input)
        {
            if (input.Trim().Length <= 0 || input.Trim().Equals("0"))
                return "0";

            if (!input.Contains("."))
                return double.Parse(input).ToString("#,#", CultureInfo.InvariantCulture);

            try
            {
                string[] data = Regex.Split(input, @"\.");

                string phanNGUYEN = double.Parse(data[0]).ToString("#,#", CultureInfo.InvariantCulture);
                if (phanNGUYEN.Trim().Length <= 0)
                    phanNGUYEN = "0";

                string phanle = "0";
                if (data[1].Trim().Length >= 4)
                    phanle = data[1].Substring(0, 4);
                else
                    phanle = data[1];

                while (phanle.EndsWith("0"))
                    phanle = phanle.Substring(0, phanle.Length - 1);

                if (phanle.Trim().Length > 0 && double.Parse(phanle) > 0)
                    return phanNGUYEN + "." + phanle;
                else
                    return phanNGUYEN;
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        public static string ForMatNumber_New(string input)
        {
            if (input.Trim().Length <= 0 || input.Trim().Equals("0"))
                return "";

            if (!input.Contains("."))
                return double.Parse(input).ToString("#,#", CultureInfo.InvariantCulture);

            try
            {
                string[] data = Regex.Split(input, @"\.");

                string phanNGUYEN = double.Parse(data[0]).ToString("#,#", CultureInfo.InvariantCulture);
                if (phanNGUYEN.Trim().Length <= 0)
                    phanNGUYEN = "0";

                string phanle = "0";
                if (data[1].Trim().Length >= 4)
                    phanle = data[1].Substring(0, 4);
                else
                    phanle = data[1];

                while (phanle.EndsWith("0"))
                    phanle = phanle.Substring(0, phanle.Length - 1);

                if (phanle.Trim().Length > 0 && double.Parse(phanle) > 0)
                    return phanNGUYEN + "." + phanle;
                else
                    return phanNGUYEN;
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        public static bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }

        public static string returnRandomString(int start)
        {
            string str = "";
            string[] array = { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "V", "U", "X", "Y", "Z", "W" };
            if (start > 24)
                start = 0;

            Random rd = new Random();
            int kq = rd.Next(start, 24);
            str = array[kq];
            return str;
        }

        public static string returnRandomChart(int start)
        {
            string str = "";
            string[] array = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "V", "U", "X", "Y", "Z", "W", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            if (start > 34)
                start = 0;
            Random rd = new Random();
            int kq = rd.Next(start, 34);
            str = array[kq];
            return str;
        }

        public static string returnRandomStringFirst(int start)
        {
            string str = "";
            string[] array = { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M"};
            if (start > 11)
                start = 0;
            Random rd = new Random();
            int kq = rd.Next(start, 11);
            str = array[kq];
            return str;
        }

        public static string returnRandomStringEND(int start)
        {
            string str = "";
            string[] array = { "N", "O", "P", "Q", "R", "S", "T", "V", "U", "X", "Y", "Z", "W" };
            if (start > 12)
                start = 0;
            Random rd = new Random();
            int kq = rd.Next(start, 12);
            str = array[kq];
            return str;
        }

        public static string returnRandomNumber(int start)
        {
            string num = "";
            string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            if (start > 9)
                start = 0;
            Random rd = new Random();
            int kq = rd.Next(start, 9);
            num = array[kq];
            return num;
        }

        public static string returnRandomTwoNumber()
        {
            Random rd = new Random();
            int num = rd.Next(10, 99);
            return num.ToString();
        }

        public static string GetValue(string search, int pos)
        {
            try
            {
                string[] arr = Regex.Split(search, ";;");

                if (arr[pos].Contains("__"))
                {
                    string[] arr2 = Regex.Split(arr[pos], "__");
                    return arr2[1];
                }
            }
            catch (Exception e)
            {
                return "";
            }

            return "";

        }

        public static string Change_AV(string ip_str_change)
        {
            ip_str_change = ip_str_change.Trim();

            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string v_str_FormD = ip_str_change.Normalize(NormalizationForm.FormD);
            string kq = v_reg_regex.Replace(v_str_FormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            return Regex.Replace(kq.Trim(), " ", "-");
        }

        public static List<string> returnListDay(string tungay, string denngay)
        {
            int day_tungay = int.Parse(tungay.Substring(0, 2));
            int month_tungay = int.Parse(tungay.Substring(3, 2));
            int year_tungay = int.Parse(tungay.Substring(6, 4));

            int day_denngay = int.Parse(denngay.Substring(0, 2));
            int month_denngay = int.Parse(denngay.Substring(3, 2));
            int year_denngay = int.Parse(denngay.Substring(6, 4));

            List<string> listStr = new List<string>();
            ExecuteData xl = new ExecuteData();

            string sql = "SELECT DATEDIFF(DAY, CONVERT(datetime, '" + tungay + "', 105), CONVERT(datetime, '" + denngay + "', 105))";
            object obj = xl.ExecuteScalarSQL(sql);

            for (int i = 0; i <= int.Parse(obj.ToString()); i++)
            {
                if (year_tungay < year_denngay)
                {
                    listStr.Add(FormatString.returnStringDay(day_tungay.ToString(), month_tungay.ToString(), year_tungay.ToString()));
                }
                else if (year_tungay == year_denngay)
                {
                    if (month_tungay < month_denngay)
                    {
                        listStr.Add(FormatString.returnStringDay(day_tungay.ToString(), month_tungay.ToString(), year_tungay.ToString()));
                    }
                    else if (month_tungay == month_denngay)
                    {
                        if (day_tungay <= day_denngay)
                        {
                            listStr.Add(FormatString.returnStringDay(day_tungay.ToString(), month_tungay.ToString(), year_tungay.ToString()));
                        }
                    }
                }

                int dayInMonth = DateTime.DaysInMonth(year_tungay, month_tungay);

                if (day_tungay == dayInMonth)
                {
                    if (month_tungay == 12)
                    {
                        year_tungay++;
                        month_tungay = 1;
                    }
                    else
                    {
                        month_tungay++;
                    }

                    day_tungay = 0;
                }
                day_tungay++;
            }
            return listStr;
        }

        public static string returnHour(string str)
        {
            str = str.Substring(0, 2);
            return str;
        }

        public static string returnMinite(string str)
        {
            str = str.Substring(3, 2);
            return str;
        }

        public static string returnStringDay(string day, string month, string year)
        {
            if (day.ToString().Length < 2)
                day = "0" + day;
            if (month.ToString().Length < 2)
                month = "0" + month;

            return day + "-" + month + "-" + year;

        }
        
        public static string returnNameMonth(int month)
        {
            string[] nameMonth = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            if (month > 12 || month < 0)
                return "Unknown " + month.ToString();

            return nameMonth[month].ToString();
        }

        public static string returnNameDay(int day)
        {
            string[] nameDay = new string[7] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            if (day > 6 || day < 0)
                return "Unknown " + day.ToString();

            return nameDay[day].ToString();
        }

        public static string returnNameDayShort(int day)
        {
            string[] nameDay = new string[7] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            if (day > 6 || day < 0)
                return "Unknown " + day.ToString();

            return nameDay[day].ToString();
        }
        
        public static string returnsortBy(string str)
        {
           while(str.Length < 10)
            {
                str = "0" + str;
            }
            return str;
        }
    }
}