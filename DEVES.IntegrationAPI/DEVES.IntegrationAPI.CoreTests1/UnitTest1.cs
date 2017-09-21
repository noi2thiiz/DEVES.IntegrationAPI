using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEVES.IntegrationAPI.CoreTests1
{

    [TestClass()]
    public class BaseApiControllerTests
    {
        [TestMethod()]
        public void Regex2Test()
        {
            string fieldName = "";
            string fieldMessage = "";
            string text =
                "Text 'culture' is not a valid culture name. Path 'culture', line 4, position 22.";
            //http://regexstorm.net/reference
            // "message": "Text 'culture' is not a valid culture name. Path 'culture', line 4, position 22."
            var pattern = new Regex(
                @"(?<message>(.+?)) Path '(?<field>\S+)', line (?<line>\d+), position (?<position>\d+).",
                RegexOptions.Singleline);
            Match match = pattern.Match(text);

            Console.WriteLine(text);
            Console.WriteLine(match.Groups["field"]);
            Console.WriteLine(match.Groups["line"]);
            Console.WriteLine(match.Groups["position"]);
            Console.WriteLine(match.Groups["message"]);

            if (match.Success)
            {
                fieldName = match.Groups["field"].Value;
                fieldMessage = match.Groups["message"].Value;
                Assert.IsNotNull(fieldMessage);
            }
            else
            {
                Assert.Fail("Invalid");
            }
        }

        [TestMethod()]
        public void Regex3Test()
        {
            string fieldName = "";
            string fieldMessage = "";
            string text =
                "Required properties are missing from object: generalHeader, profileInfo.name, profileInfo.";
            //http://regexstorm.net/reference
            //https://www.freeformatter.com/regex-tester.html
            // "message": "Text 'culture' is not a valid culture name. Path 'culture', line 4, position 22."
            var pattern = new Regex(
                @"(?<message>(.+?)): (?<field>.+).", RegexOptions.Singleline);
            //(^(?:(?:"((?:""|[^"])+)"|([^,]*))(?:$|,))+$))
            Match match = pattern.Match(text);

            Console.WriteLine(text);
            Console.WriteLine(match.Groups["field"]);

            Console.WriteLine(match.Groups["message"]);

            if (match.Success)
            {
                fieldName = match.Groups["field"].Value;
                fieldMessage = match.Groups["message"].Value;
                Assert.IsTrue(fieldMessage.Contains("Required properties are missing from object"));
                Assert.IsTrue(fieldName.Contains("generalHeader, profileInfo.name, profileInfo"));
            }
            else
            {
                Assert.Fail("Invalid");
            }
        }


    }
}