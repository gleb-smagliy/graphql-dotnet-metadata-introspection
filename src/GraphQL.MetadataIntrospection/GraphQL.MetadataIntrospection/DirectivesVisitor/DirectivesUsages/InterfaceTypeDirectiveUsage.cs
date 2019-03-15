using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal class InterfaceTypeDirectiveUsage
    {
        public GraphQLInterfaceTypeDefinition OnInterface { get; private set; }

        public GraphQLDirective Directive { get; private set; }

        public InterfaceTypeDirectiveUsage(GraphQLInterfaceTypeDefinition onInterface, GraphQLDirective directive)
        {
            this.Directive = directive;
            this.OnInterface = onInterface;
        }
    }
}
