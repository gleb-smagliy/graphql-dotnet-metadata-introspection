using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal struct InterfaceFieldDirectiveUsage
    {
        public GraphQLInterfaceTypeDefinition OnInterface { get; }

        public GraphQLFieldDefinition OnField { get; }

        public GraphQLDirective Directive { get; }

        public InterfaceFieldDirectiveUsage(GraphQLInterfaceTypeDefinition onInterface, GraphQLFieldDefinition onField, GraphQLDirective directive)
        {
            Directive = directive;
            OnField = onField;
            OnInterface = onInterface;
        }
    }
}
