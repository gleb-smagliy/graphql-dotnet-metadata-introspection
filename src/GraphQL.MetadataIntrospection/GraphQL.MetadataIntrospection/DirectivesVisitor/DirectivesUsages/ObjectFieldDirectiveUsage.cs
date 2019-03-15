using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages
{
    internal struct ObjectFieldDirectiveUsage
    {
        public GraphQLObjectTypeDefinition OnObject { get; private set; }

        public GraphQLFieldDefinition OnField { get; private set; }

        public GraphQLDirective Directive { get; private set; }

        public ObjectFieldDirectiveUsage(GraphQLObjectTypeDefinition onObject, GraphQLFieldDefinition onField, GraphQLDirective directive)
        {
            this.OnObject = onObject;
            this.OnField = onField;
            this.Directive = directive;
        }
    }
}
