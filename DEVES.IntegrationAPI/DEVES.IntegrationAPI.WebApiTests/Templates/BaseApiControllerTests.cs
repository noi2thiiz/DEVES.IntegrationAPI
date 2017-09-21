using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Templates.Tests
{
   // [TestClass()]
    public class BaseApiControllerTests
    {
       // [TestMethod()]
        public void GetTransactionIdTest()
        {
            string fieldName = "";
            string fieldMessage = "";
            string text =
                "Text \\u0027culture\\u0027 is not a valid culture name. Path \\u0027culture\\u0027, line 4, position 22.";
            //http://regexstorm.net/reference
            // "message": "Text 'culture' is not a valid culture name. Path 'culture', line 4, position 22."
            var pattern = new Regex(
                @"(?<message>(.*?)(?:(\r\n){2,}|\r{2,}|\n{2,}|$)). Path '(?<field>\S+)', line (?<line>\d+), position (?<position>\d+).", RegexOptions.Singleline);
            Match match = pattern.Match(text);

            if (match.Success)
            {
                fieldName = match.Groups["field"].Value;
                fieldMessage = match.Groups["message"].Value;
            }
            else
            {
                fieldMessage = "Invalid";
            }
            Console.WriteLine(match.Groups.ToJson());
        }
    }
}