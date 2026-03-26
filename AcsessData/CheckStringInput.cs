using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.AcsessData
{
    public class CheckStringInput
    {
        public static string result;

        public string ReturnResult(string input)
        {
            result = "";

            if (input.ToUpper().Contains("OR ") || input.ToUpper().Contains("'"))
            {
                return "Error!";  //SQL INNER JOIN
            }

            return result;
        }
    }
}