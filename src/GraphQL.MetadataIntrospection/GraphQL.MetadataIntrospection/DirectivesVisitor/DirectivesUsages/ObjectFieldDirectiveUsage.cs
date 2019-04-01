using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal struct ObjectFieldDirectiveUsage
    {
        public GraphQLObjectTypeDefinition OnObject { get; }

        public GraphQLFieldDefinition OnField { get; }

        public GraphQLDirective Directive { get; }

        public ObjectFieldDirectiveUsage(GraphQLObjectTypeDefinition onObject, GraphQLFieldDefinition onField, GraphQLDirective directive)
        {
            OnObject = onObject;
            OnField = onField;
            Directive = directive;
        }
    }
}
