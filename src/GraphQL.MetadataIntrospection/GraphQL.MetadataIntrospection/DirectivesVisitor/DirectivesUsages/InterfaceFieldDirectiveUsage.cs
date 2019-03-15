using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal struct InterfaceFieldDirectiveUsage
    {
        public GraphQLInterfaceTypeDefinition OnInterface { get; private set; }

        public GraphQLFieldDefinition OnField { get; private set; }

        public GraphQLDirective Directive { get; private set; }

        public InterfaceFieldDirectiveUsage(GraphQLInterfaceTypeDefinition onInterface, GraphQLFieldDefinition onField, GraphQLDirective directive)
        {
            this.Directive = directive;
            this.OnField = onField;
            this.OnInterface = onInterface;
        }
    }
}
