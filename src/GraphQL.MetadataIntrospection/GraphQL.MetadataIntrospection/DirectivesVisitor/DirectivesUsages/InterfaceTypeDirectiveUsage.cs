using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal class InterfaceTypeDirectiveUsage
    {
        public GraphQLInterfaceTypeDefinition OnInterface { get; }

        public GraphQLDirective Directive { get; }

        public InterfaceTypeDirectiveUsage(GraphQLInterfaceTypeDefinition onInterface, GraphQLDirective directive)
        {
            Directive = directive;
            OnInterface = onInterface;
        }
    }
}
