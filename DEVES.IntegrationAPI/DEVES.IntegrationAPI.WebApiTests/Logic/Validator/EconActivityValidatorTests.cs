using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi.Templates;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DEVES.IntegrationAPI.WebApi.Logic.Validator.Tests
{
    [TestClass()]
    public class EconActivityValidatorTests
    {
        [TestMethod()]
        public void Validate_EconActivityValidator_It_Should_Invalid_When_Give_Invalid_Code_Test()
        {
            var json = @"{
                         'econActivity': 'hgtyi'
                        }";
            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new EconActivityValidator() }
            };

            // the econActivity validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'econActivity': {
                                   'type': ['string','null'],
                                   'format': 'econActivity'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual("Value 'hgtyi' is not a valid econ activity code.", errors[0].Message);
        }

        [DataTestMethod]
        [DataRow("001")]
        [DataRow("002")]
        [DataRow("003")]
        [DataRow("004")]
        [DataRow("005")]
        [DataRow("006")]
        [DataRow("007")]
        [DataRow("008")]
        public void Validate_EconActivityValidator_It_Should_Valid_When_Give_Valid_Code_Test(string value1)
        {
            var json = $"{{'econActivity': '{value1}'}}";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new EconActivityValidator() }
            };


            // the econActivity validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'econActivity': {
                                   'type': ['string','null'],
                                   'format': 'econActivity'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);

            Assert.IsTrue(isValid);

        }
        [TestMethod()]
        public void Validate_EconActivityValidator_It_Should_Valid_When_Give_Empty_String_Test()
        {
            var json = @"{
                         'econActivity': ''
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new EconActivityValidator() }
            };

            // the econActivity validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'econActivity': {
                                   'type': ['string','null'],
                                   'format': 'econActivity'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);
            Console.WriteLine(errors?.ToJson());
            Assert.IsTrue(isValid, "It_Should_Valid_When_Give_Empty_String");

        }

        [TestMethod()]
        public void Validate_EconActivityValidator_It_Should_Valid_When_Give_Null_Value_Test()
        {


            var json = @"{
                         'econActivity': null
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new EconActivityValidator() }
            };

            // the econActivity validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'econActivity': {
                                   'type': ['string','null'],
                                   'format': 'econActivity'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);
            Console.WriteLine(errors?.ToJson());
            Assert.IsTrue(isValid, "It_Should_Valid_When_Give_Null_Value");

        }

        [TestMethod()]
        public void Validate_EconActivityValidator_It_Should_Valid_When_Missing_Property_Test()
        {


            var json = @"{
                         'name': 'JANE'
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new EconActivityValidator() }
            };

            // the econActivity validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'econActivity': {
                                   'type': ['string','null'],
                                   'format': 'econActivity'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);
            Console.WriteLine(errors?.ToJson());
            Assert.IsTrue(isValid, "It_Should_Valid_When_Missing_Property");

        }
    }
}