namespace GraphQL.MetadataIntrospection.Schema
{
    internal class SchemaParser
    {
        public static GraphQLDocument ParseSchema(string typeDefinitions)
        {
            var lexer = new Lexer();
            var parser = new Parser(lexer);
            return parser.Parse(new Source(typeDefinitions));
        }
    }
}
