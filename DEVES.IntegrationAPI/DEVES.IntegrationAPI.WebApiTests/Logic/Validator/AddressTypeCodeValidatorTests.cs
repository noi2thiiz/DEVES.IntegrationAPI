﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class AddressTypeCodeValidatorTests
    {
        [TestMethod()]
        public void Validate_AddressTypeValidator_It_Should_Invalid_When_Give_Invalid_Code_Test()
        {
            var json = @"{
                         'addressType': 'hgtyi'
                        }";
            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new AddressTypeValidator() }
            };

            // the addressType validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'addressType': {
                                   'type': ['string','null'],
                                   'format': 'addressType'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);

            Assert.IsFalse(isValid);
            Assert.AreEqual("Value 'hgtyi' is not a valid address type code.", errors[0].Message);
        }

        [TestMethod()]
        public void Validate_AddressTypeValidator_It_Should_Valid_When_Give_Valid_Code_Test()
        {
            var json = @"{
                         'addressType': '01'
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new AddressTypeValidator() }
            };


            // the addressType validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'addressType': {
                                   'type': ['string','null'],
                                   'format': 'addressType'
                                 }
                               }
                            }", settings);

            var cultures = JObject.Parse(json);

            IList<ValidationError> errors;
            var isValid = cultures.IsValid(schema, out errors);

            Assert.IsTrue(isValid);

        }
        [TestMethod()]
        public void Validate_AddressTypeValidator_It_Should_Valid_When_Give_Empty_String_Test()
        {
            var json = @"{
                         'addressType': ''
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new AddressTypeValidator() }
            };

            // the addressType validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'addressType': {
                                   'type': ['string','null'],
                                   'format': 'addressType'
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
        public void Validate_AddressTypeValidator_It_Should_Valid_When_Give_Null_Value_Test()
        {


            var json = @"{
                         'addressType': null
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new AddressTypeValidator() }
            };

            // the addressType validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'addressType': {
                                   'type': ['string','null'],
                                   'format': 'addressType'
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
        public void Validate_AddressTypeValidator_It_Should_Valid_When_Missing_Property_Test()
        {


            var json = @"{
                         'name': 'JANE'
                        }";

            var settings = new JSchemaReaderSettings
            {
                Validators = new List<JsonValidator> { new AddressTypeValidator() }
            };

            // the addressType validator will be used to validate the object items
            var schema = JSchema.Parse(@"{
                               'type': 'object',
                               'properties': {
                                 'addressType': {
                                   'type': ['string','null'],
                                   'format': 'addressType'
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