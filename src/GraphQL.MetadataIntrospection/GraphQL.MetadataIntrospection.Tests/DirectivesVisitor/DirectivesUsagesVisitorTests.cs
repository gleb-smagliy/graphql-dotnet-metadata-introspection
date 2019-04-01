using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraphQL.MetadataIntrospection.DirectivesVisitor;
using GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages;
using GraphQLParser.AST;
using Xunit;

namespace GraphQL.MetadataIntrospection.Tests.DirectivesVisitor
{
    internal class TestDirectivesUsagesVisitor : DirectivesUsagesVisitor
    {
        private readonly IList<string> _data = new List<string>();

        public IList<string> Build(GraphQLDocument document)
        {
            Walk(document);
            return _data;
        }

        protected override void InterfaceFieldDirectiveUsageVisited(InterfaceFieldDirectiveUsage usage)
        {
            _data.Add($"I:{usage.OnInterface.Name.Value}_F:{usage.OnField.Name.Value}_D:{usage.Directive.Name.Value}");
        }

        protected override void InterfaceTypeDirectiveUsageVisited(InterfaceTypeDirectiveUsage usage)
        {
            _data.Add($"I:{usage.OnInterface.Name.Value}_D:{usage.Directive.Name.Value}");
        }

        protected override void ObjectFieldDirectiveUsageVisited(ObjectFieldDirectiveUsage usage)
        {
            _data.Add($"O:{usage.OnObject.Name.Value}_F:{usage.OnField.Name.Value}_D:{usage.Directive.Name.Value}");
        }

        protected override void ObjectTypeDirectiveUsageVisited(ObjectTypeDirectiveUsage usage)
        {
            _data.Add($"O:{usage.OnObject.Name.Value}_D:{usage.Directive.Name.Value}");
        }
    }

    public class DirectivesUsagesVisitorTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 0, 1, 1, 1)]
        [InlineData(1, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 1, 1, 1)]
        [InlineData(0, 1, 0, 0, 0, 0)]
        [InlineData(0, 1, 0, 1, 1, 1)]
        [InlineData(0, 0, 1, 0, 0, 0)]
        [InlineData(0, 0, 1, 1, 1, 1)]

        [InlineData(1, 1, 1, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 1, 1)]
        [InlineData(1, 1, 1, 1, 0, 0)]
        [InlineData(1, 1, 1, 0, 1, 0)]
        [InlineData(1, 1, 1, 0, 0, 1)]
        [InlineData(1, 1, 1, 0, 2, 2)]
        [InlineData(1, 1, 1, 3, 0, 3)]
        [InlineData(1, 1, 1, 4, 4, 0)]


        [InlineData(1, 1, 2, 0, 0, 0)]
        [InlineData(1, 2, 1, 1, 1, 1)]
        [InlineData(2, 1, 1, 1, 0, 0)]
        [InlineData(0, 2, 1, 0, 1, 0)]
        [InlineData(1, 0, 2, 0, 0, 1)]
        [InlineData(2, 1, 0, 0, 2, 2)]
        public void Node_Walk_Test(int objDefinitions, int extDefinitions, int interfaceDefinitions,
            int countDirectives, int countFields, int countFieldDirectives)
        {
            HashSet<string> directives = new HashSet<string>();

            var nodes = new List<ASTNode>();
            for (var i = 0; i < objDefinitions; i++)
                nodes.Add(GetObjectTypeDefinition(directives, countDirectives, countFields, countFieldDirectives));
            for (var i = 0; i < extDefinitions; i++)
                nodes.Add(GetTypeExtentionDefinition(directives, countDirectives, countFields, countFieldDirectives));
            for (var i = 0; i < interfaceDefinitions; i++)
                nodes.Add(GetInterfaceTypeDefinition(directives, countDirectives, countFields, countFieldDirectives));

            var doc = new GraphQLDocument {Definitions = nodes};

            var built = new TestDirectivesUsagesVisitor().Build(doc);

            Assert.Equal(directives.Count, built.Count);
            Assert.All(built, b => Assert.Contains(b, directives));
        }

        private static GraphQLInterfaceTypeDefinition GetInterfaceTypeDefinition(HashSet<string> directives,
            int countDirectives, int countFields, int countFieldDirectives)
        {
            var name = GetRandomAlphaNumeric();
            var dirs = GetDirective(countDirectives);

            var fieldDefs = GetFieldDefinition(countFields, countFieldDirectives);

            dirs.ToList().ForEach(d => directives.Add($"I:{name}_D:{d.Name.Value}"));
            fieldDefs.ToList().ForEach(f =>
                f.Directives.ToList().ForEach(d =>
                    directives.Add($"I:{name}_F:{f.Name.Value}_D:{d.Name.Value}")));

            return new GraphQLInterfaceTypeDefinition
            {
                Name = new GraphQLName() {Value = name},
                Directives = dirs,
                Fields = fieldDefs
            };
        }

        private static GraphQLTypeExtensionDefinition GetTypeExtentionDefinition(HashSet<string> directives,
            int countDirectives, int countFields, int countFieldDirectives)
        {
            var name = GetRandomAlphaNumeric();
            var dirs = GetDirective(countDirectives);
            var fieldDefs = GetFieldDefinition(countFields, countFieldDirectives);

            dirs.ToList().ForEach(d => directives.Add($"O:{name}_D:{d.Name.Value}"));
            fieldDefs.ToList().ForEach(f =>
                f.Directives.ToList().ForEach(d =>
                    directives.Add($"O:{name}_F:{f.Name.Value}_D:{d.Name.Value}")));


            return new GraphQLTypeExtensionDefinition
            {
                Definition = new GraphQLObjectTypeDefinition
                {
                    Name = new GraphQLName() {Value = name},
                    Directives = dirs,
                    Fields = fieldDefs
                }
            };
        }

        private static GraphQLObjectTypeDefinition GetObjectTypeDefinition(HashSet<string> directives,
            int countDirectives, int countFields, int countFieldDirectives)
        {
            var name = GetRandomAlphaNumeric();
            var dirs = GetDirective(countDirectives);
            var fieldDefs = GetFieldDefinition(countFields, countFieldDirectives);

            dirs.ToList().ForEach(d => directives.Add($"O:{name}_D:{d.Name.Value}"));
            fieldDefs.ToList().ForEach(f =>
                f.Directives.ToList().ForEach(d =>
                    directives.Add($"O:{name}_F:{f.Name.Value}_D:{d.Name.Value}")));

            return new GraphQLObjectTypeDefinition
            {
                Name = new GraphQLName() {Value = name},
                Directives = dirs,
                Fields = fieldDefs
            };
        }

        private static IEnumerable<GraphQLFieldDefinition> GetFieldDefinition(int count, int directivesCount)
        {
            var definitions = new List<GraphQLFieldDefinition>();
            for (var i = 0; i < count; i++)
            {
                definitions.Add(new GraphQLFieldDefinition
                {
                    Name = new GraphQLName {Value = GetRandomAlphaNumeric()},
                    Directives = GetDirective(directivesCount)
                });
            }

            return definitions;
        }

        private static IEnumerable<GraphQLDirective> GetDirective(int count)
        {
            var directives = new List<GraphQLDirective>();
            for (var i = 0; i < count; i++)
            {
                directives.Add(new GraphQLDirective
                {
                    Name = new GraphQLName {Value = GetRandomAlphaNumeric()}
                });
            }

            return directives;
        }

        public static string GetRandomAlphaNumeric()
        {
            return Path.GetRandomFileName().Replace(".", "").Substring(0, 8);
        }
    }
}