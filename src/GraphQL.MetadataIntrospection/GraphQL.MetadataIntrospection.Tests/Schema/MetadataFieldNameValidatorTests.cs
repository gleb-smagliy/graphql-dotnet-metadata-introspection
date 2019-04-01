using GraphQL.MetadataIntrospection.Schema;
using Xunit;

namespace GraphQL.MetadataIntrospection.Tests.Schema
{
    public class MetadataFieldNameValidatorTests
    {
        [Theory]
        [InlineData("Field_name")]
        [InlineData("field_Name")]
        [InlineData("fie34ld_Name")]
        [InlineData("field_Name123")]
        [InlineData("f")]
        public void Validate_Success_Test(string input)
        {
            var result = MetadataFieldNameValidator.Validate(input);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData("-Not_Field_name")]
        [InlineData("(NotFie9ld")]
        [InlineData("%NotField")]
        [InlineData("Not_F^%#ield")]
        [InlineData("")]
        public void Validate_Error_Test(string input)
        {
            var result = MetadataFieldNameValidator.Validate(input);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
            Assert.Contains($"Field name <{input}> should", result.Error);
        }
    }
}