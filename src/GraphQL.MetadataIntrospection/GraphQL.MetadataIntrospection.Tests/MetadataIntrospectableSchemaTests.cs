using System;
using System.Text.RegularExpressions;
using Xunit;

namespace GraphQL.MetadataIntrospection.Tests
{
    public class MetadataIntrospectableSchemaTests
    {
        [Fact]
        public void Schema_For_OnField_Test()
        {
            var sdl = @"
            schema {
              query: Query
            }
            type Query {
              a: A
            }
           type A {
            id: ID! @ref(query: ""B"", as: ""C"")
            name: String
            }";

            const string expected =
                @"{""data"":{""_metadata"":[{""arguments"":[{""name"":""query"",""value"":""\""B\""""},{""name"":""as"",""value"":""\""C\""""}],""location"":""OBJECT_FIELD"",""name"":""ref"",""fieldName"":""id"",""typeName"":""A""}]}}";
            Assert.Equal(expected, Execute(sdl));
        }

        [Fact]
        public void Schema_For_OnObject_Test()
        {
            var sdl = @"
            schema {
              query: Query
            }
            type Query {
              a: A
            }
           type A @ref(query: ""B"", as: ""C"") {
            id: ID!
            name: String
            }";

            const string expected =
                @"{""data"":{""_metadata"":[{""arguments"":[{""name"":""query"",""value"":""\""B\""""},{""name"":""as"",""value"":""\""C\""""}],""location"":""OBJECT_TYPE"",""name"":""ref"",""fieldName"":null,""typeName"":""A""}]}}";

            Assert.Equal(expected, Execute(sdl));
        }

        [Fact]
        public void Schema_For_Multi_Test()
        {
            var sdl = @"
            schema {
              query: Query
            }
            type Query {
              a: A
            }
           type A @ref(query: ""Z"", as: ""Y"") {
            id: ID! @ref(query: ""X"", as: ""V"")
            name: String
            }
            type B @ref(query: ""U"", as: ""W"") {
            id: ID! @ref(query: ""X"", as: ""V"")
            name: String
            }
            type C @ref(query: ""U"", as: ""W"") {
            id: ID!
            name: String @ref(query: ""H"", as: ""Y"")
            }";

            const string expected =
                @"{""data"":{""_metadata"":[{""arguments"":[{""name"":""query"",""value"":""\""Z\""""},{""name"":""as"",""value"":""\""Y\""""}],""location"":""OBJECT_TYPE"",""name"":""ref"",""fieldName"":null,""typeName"":""A""},{""arguments"":[{""name"":""query"",""value"":""\""X\""""},{""name"":""as"",""value"":""\""V\""""}],""location"":""OBJECT_FIELD"",""name"":""ref"",""fieldName"":""id"",""typeName"":""A""},{""arguments"":[{""name"":""query"",""value"":""\""U\""""},{""name"":""as"",""value"":""\""W\""""}],""location"":""OBJECT_TYPE"",""name"":""ref"",""fieldName"":null,""typeName"":""B""},{""arguments"":[{""name"":""query"",""value"":""\""X\""""},{""name"":""as"",""value"":""\""V\""""}],""location"":""OBJECT_FIELD"",""name"":""ref"",""fieldName"":""id"",""typeName"":""B""},{""arguments"":[{""name"":""query"",""value"":""\""U\""""},{""name"":""as"",""value"":""\""W\""""}],""location"":""OBJECT_TYPE"",""name"":""ref"",""fieldName"":null,""typeName"":""C""},{""arguments"":[{""name"":""query"",""value"":""\""H\""""},{""name"":""as"",""value"":""\""Y\""""}],""location"":""OBJECT_FIELD"",""name"":""ref"",""fieldName"":""name"",""typeName"":""C""}]}}";

            Assert.Equal(expected, Execute(sdl));
        }

        [Fact]
        public void Schema_For_Non_Test()
        {
            var sdl = @"
            schema {
              query: Query
            }
            type Query {
              a: A
            }
           type A {
            id: ID!
            name: String
            }";

            const string expected =
                @"{""data"":{""_metadata"":[]}}";

            Assert.Equal(expected, Execute(sdl));
        }

        [Fact]
        public void Schema_For_Exclude_Test()
        {
            var sdl = @"
            schema {
              query: Query
            }
            type Query {
              a: A
            }
           type A @exclude(query: ""Z"", as: ""Y"") {
            id: ID! @ref(query: ""X"", as: ""V"")
            name: String
            }
            type B @ref(query: ""U"", as: ""W"") {
            id: ID! @ref(query: ""X"", as: ""V"")
            name: String
            }
            type C @ref(query: ""U"", as: ""W"") {
            id: ID!
            name: String @ref(query: ""H"", as: ""Y"")
            }";

            const string expected =
                @"{""data"":{""_metadata"":[{""arguments"":[{""name"":""query"",""value"":""\""X\""""},{""name"":""as"",""value"":""\""V\""""}],""location"":""OBJECT_FIELD"",""name"":""ref"",""fieldName"":""id"",""typeName"":""A""},{""arguments"":[{""name"":""query"",""value"":""\""U\""""},{""name"":""as"",""value"":""\""W\""""}],""location"":""OBJECT_TYPE"",""name"":""ref"",""fieldName"":null,""typeName"":""B""},{""arguments"":[{""name"":""query"",""value"":""\""X\""""},{""name"":""as"",""value"":""\""V\""""}],""location"":""OBJECT_FIELD"",""name"":""ref"",""fieldName"":""id"",""typeName"":""B""},{""arguments"":[{""name"":""query"",""value"":""\""U\""""},{""name"":""as"",""value"":""\""W\""""}],""location"":""OBJECT_TYPE"",""name"":""ref"",""fieldName"":null,""typeName"":""C""},{""arguments"":[{""name"":""query"",""value"":""\""H\""""},{""name"":""as"",""value"":""\""Y\""""}],""location"":""OBJECT_FIELD"",""name"":""ref"",""fieldName"":""name"",""typeName"":""C""}]}}";

            Assert.Equal(expected, Execute(sdl, c => c.ExcludeDirective("exclude")));
        }

        [Fact]
        public void Schema_For_Include_Test()
        {
            var sdl = @"
            schema {
              query: Query
            }
            type Query {
              a: A
            }
           type A @include(query: ""Z"", as: ""Y"") {
            id: ID! @ref(query: ""X"", as: ""V"")
            name: String
            }
            type B @ref(query: ""U"", as: ""W"") {
            id: ID! @ref(query: ""X"", as: ""V"")
            name: String
            }
            type C @ref(query: ""U"", as: ""W"") {
            id: ID!
            name: String @ref(query: ""H"", as: ""Y"")
            }";

            const string expected =
                @"{""data"":{""_metadata"":[{""arguments"":[{""name"":""query"",""value"":""\""Z\""""},{""name"":""as"",""value"":""\""Y\""""}],""location"":""OBJECT_TYPE"",""name"":""include"",""fieldName"":null,""typeName"":""A""}]}}";

            Assert.Equal(expected, Execute(sdl, c => c.IncludeDirective("include")));
        }

        private string Execute(string sdl, Action<IntrospectionConfiguration> configureEnrichment = null)
        {
            var query = @"
            {
                _metadata {
                  arguments {
                    name
                    value
                  }
                  location
                  name
                  fieldName
                  typeName
                }
            }";
            var schema = MetadataIntrospectableSchema.For(sdl, null, configureEnrichment);
            schema.Initialize();
            return Regex.Replace(schema.Execute(options => options.Query = query), @"\s+", "");
        }
    }
}