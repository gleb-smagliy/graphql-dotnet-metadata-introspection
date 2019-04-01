using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal struct ObjectTypeDirectiveUsage
    {
        public GraphQLObjectTypeDefinition OnObject { get; }

        public GraphQLDirective Directive { get; }

        public ObjectTypeDirectiveUsage(GraphQLObjectTypeDefinition onObject, GraphQLDirective directive)
        {
            OnObject = onObject;
            Directive = directive;
        }
    }
}
