using FluentAssertions;
using Notino.ConverterProviders;
using Xunit;

namespace Notino.Tests
{
    public class XmlToJsonConverterProviderTests
    {
        [Fact]
        public void GivenValidXmlString_WhenConvertingFromXmlToJson_ValidJsonShouldBeReturned()
        {
            var insertedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
                        <myData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                          <title>Title</title>
                          <text>Text</text>
                        </myData>";
            var expectedJson = @"{""Title"":""Title"",""Text"":""Text""}";

            var converter = new XmlToJsonConverterProvider();
            var result = converter.Convert(insertedXml);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void GivenInvalidXmlString_WhenConvertingFromXmlToJson_ErrorShouldBeReturned()
        {
            var insertedXml = @"
                          <title>Title</title>
                          <text>Text</text>
                        </myData>";

            var converter = new XmlToJsonConverterProvider();
            var result = converter.Convert(insertedXml);

            result.Succeeded.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            //Different kinds of errors should be tested (out of scope of this POC)
        }
    }
}
