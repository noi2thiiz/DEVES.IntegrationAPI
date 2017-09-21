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
    public class ProvinceCodeValidatorTests
    {
        [TestMethod()]
        public void Validate_ProvinceCodeValidator_It_Should_Invalid_When_Give_Invalid_Code_Test()
        {
            var json = @"{
                         'province': '123412'
                        }";
            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new ProvinceValidator() }
            };

            // the province validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'province': {
                                   'type': ['string','null'],
                                   'format': 'province'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual("Value '123412' is not a valid province code.", errors[0].Message);
        }

        [TestMethod()]
        public void Validate_ProvinceCodeValidator_It_Should_Valid_When_Give_Valid_Code_Test()
        {
            var json = @"{
                         'province': '10'
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new ProvinceValidator() }
            };


            // the province validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'province': {
                                   'type': ['string','null'],
                                   'format': 'province'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);

            Assert.IsTrue(isValid);

        }
        [TestMethod()]
        public void Validate_ProvinceCodeValidator_It_Should_Valid_When_Give_Empty_String_Test()
        {
            var json = @"{
                         'province': ''
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new ProvinceValidator() }
            };

            // the province validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'province': {
                                   'type': ['string','null'],
                                   'format': 'province'
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
        public void Validate_ProvinceCodeValidator_It_Should_Valid_When_Give_Null_Value_Test()
        {


            var json = @"{
                         'province': null
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new ProvinceValidator() }
            };

            // the province validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'province': {
                                   'type': ['string','null'],
                                   'format': 'province'
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
        public void Validate_ProvinceCodeValidator_It_Should_Valid_When_Missing_Property_Test()
        {


            var json = @"{
                         'name': 'JANE'
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new ProvinceValidator() }
            };

            // the province validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'province': {
                                   'type': ['string','null'],
                                   'format': 'province'
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