using System;
using Mantle.Extensions;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void Should_merge_anonymous_data_object_into_string()
        {
            const string sourceString = "My name is {FirstName} {LastName} and I am {Age}.";
            const string expectedString = "My name is Casey Watson and I am 33.";

            var testObj = new {FirstName = "Casey", LastName = "Watson", Age = 33};

            var mergedString = sourceString.Merge(testObj);

            Assert.IsNotNull(mergedString);
            Assert.AreEqual(expectedString, mergedString);
        }

        [Test]
        public void Should_parse_string_to_Date_Time_value()
        {
            var dateTimeResult = "1/11/1981".TryParseDateTime();

            Assert.IsNotNull(dateTimeResult);
            Assert.AreEqual(dateTimeResult.Value, DateTime.Parse("1/11/1981"));
        }

        [Test]
        public void Should_parse_string_to_Guid_value()
        {
            var testGuid = Guid.NewGuid();
            var guidResult = testGuid.ToString().TryParseGuid();

            Assert.IsNotNull(guidResult);
            Assert.AreEqual(guidResult.Value, testGuid);
        }

        [Test]
        public void Should_parse_string_to_boolean_value()
        {
            var boolResult = "true".TryParseBoolean();

            Assert.IsNotNull(boolResult);
            Assert.IsTrue(boolResult.Value);
        }

        [Test]
        public void Should_parse_string_to_double_value()
        {
            var doubleResult = "1.0".TryParseDouble();

            Assert.IsNotNull(doubleResult);
            Assert.AreEqual(doubleResult.Value, 1.0);
        }

        [Test]
        public void Should_parse_string_to_int_value()
        {
            var intResult = "1".TryParseInt();

            Assert.IsNotNull(intResult);
            Assert.AreEqual(intResult.Value, 1);
        }

        [Test]
        public void Should_parse_string_to_long_value()
        {
            var longResult = "1".TryParseLong();

            Assert.IsNotNull(longResult);
            Assert.AreEqual(longResult.Value, 1);
        }

        [Test]
        public void Should_return_null_if_unable_to_parse_string_to_Date_Time_value()
        {
            var dateTimeResult = "invalid".TryParseDateTime();

            Assert.IsNull(dateTimeResult);
        }

        [Test]
        public void Should_return_null_if_unable_to_parse_string_to_Guid_value()
        {
            var guidResult = "invalid".TryParseGuid();

            Assert.IsNull(guidResult);
        }

        [Test]
        public void Should_return_null_if_unable_to_parse_string_to_boolean_value()
        {
            var boolResult = "invalid".TryParseBoolean();

            Assert.IsNull(boolResult);
        }

        [Test]
        public void Should_return_null_if_unable_to_parse_string_to_double_value()
        {
            var doubleResult = "invalid".TryParseDouble();

            Assert.IsNull(doubleResult);
        }

        [Test]
        public void Should_return_null_if_unable_to_parse_string_to_int_value()
        {
            var intResult = "invalid".TryParseInt();

            Assert.IsNull(intResult);
        }

        [Test]
        public void Should_return_null_if_unable_to_parse_string_to_long_value()
        {
            var longResult = "invalid".TryParseLong();

            Assert.IsNull(longResult);
        }

        [Test]
        public void Should_throw_ArgumentException_if_attempting_to_parse_null_string_to_Date_Time_value()
        {
            Assert.Throws<ArgumentException>(() => default(string).TryParseDateTime());
        }

        [Test]
        public void Should_throw_ArgumentException_if_attempting_to_parse_null_string_to_Guid_value()
        {
            Assert.Throws<ArgumentException>(() => default(string).TryParseGuid());
        }

        [Test]
        public void Should_throw_ArgumentException_if_attempting_to_parse_null_string_to_boolean_value()
        {
            Assert.Throws<ArgumentException>(() => default(string).TryParseBoolean());
        }

        [Test]
        public void Should_throw_ArgumentException_if_attempting_to_parse_null_string_to_double_value()
        {
            Assert.Throws<ArgumentException>(() => default(string).TryParseDouble());
        }

        [Test]
        public void Should_throw_ArgumentException_if_attempting_to_parse_null_string_to_int_value()
        {
            Assert.Throws<ArgumentException>(() => default(string).TryParseInt());
        }

        [Test]
        public void Should_throw_ArgumentException_if_attempting_to_parse_null_string_to_long_value()
        {
            Assert.Throws<ArgumentException>(() => default(string).TryParseLong());
        }

        [Test]
        public void When_merging_anonymous_data_object_into_string_should_ignore_fields_not_defined_in_data_object()
        {
            const string sourceString = "My name is {FirstName} {LastName} and I am {Age}.";
            const string expectedString = "My name is Casey Watson and I am {Age}.";

            var testObj = new {FirstName = "Casey", LastName = "Watson"};

            var mergedString = sourceString.Merge(testObj);

            Assert.IsNotNull(mergedString);
            Assert.AreEqual(expectedString, mergedString);
        }

        [Test]
        public void When_merging_anonymous_data_object_into_string_should_ignore_fields_not_defined_in_string()
        {
            const string sourceString = "My name is {FirstName} {LastName} and I am {Age}.";
            const string expectedString = "My name is Casey Watson and I am 33.";

            var testObj =
                new {FirstName = "Casey", LastName = "Watson", Age = 33, Birthdate = DateTime.Parse("1/11/1981")};

            var mergedString = sourceString.Merge(testObj);

            Assert.IsNotNull(mergedString);
            Assert.AreEqual(expectedString, mergedString);
        }

        [Test]
        public void When_merging_anonymous_data_object_into_string_should_use_custom_delimiters_if_provided()
        {
            const string sourceString = "My name is |FirstName| |LastName| and I am |Age|.";
            const string expectedString = "My name is Casey Watson and I am 33.";

            var testObj = new {FirstName = "Casey", LastName = "Watson", Age = 33};

            var mergedString = sourceString.Merge(testObj, '|', '|');

            Assert.IsNotNull(mergedString);
            Assert.AreEqual(expectedString, mergedString);
        }

        [Test]
        public void When_merging_data_object_into_string_should_throw_ArgumentException_if_source_is_null()
        {
            var testObj = new {FirstName = "Casey", LastName = "Watson"};

            Assert.Throws<ArgumentException>(() => default(string).Merge(testObj));
        }

        [Test]
        public void When_merging_data_object_into_string_should_throw_ArgumentNullException_if_data_is_null()
        {
            const string sourceString = "My name is {FirstName} {LastName} and I am {Age}.";

            Assert.Throws<ArgumentNullException>(() => sourceString.Merge(null));
        }
    }
}