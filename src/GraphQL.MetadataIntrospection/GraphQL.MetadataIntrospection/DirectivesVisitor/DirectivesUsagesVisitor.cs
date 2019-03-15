using GraphQL.MetadataIntrospection.DirectivesVisitor.DirectivesUsages;
using GraphQLParser.AST;

namespace GraphQL.MetadataIntrospection.DirectivesVisitor
{
    internal abstract class DirectivesUsagesVisitor
    {
        protected void Walk(GraphQLDocument document)
        {
            foreach (var astNode in document.Definitions)
            {
                switch (astNode.Kind)
                {
                    case ASTNodeKind.ObjectTypeDefinition:
                        this.VisitObjectType((GraphQLObjectTypeDefinition)astNode);
                        break;
                    case ASTNodeKind.TypeExtensionDefinition:
                        var extension = (GraphQLTypeExtensionDefinition)astNode;
                        this.VisitObjectType(extension.Definition);
                        break;
                    case ASTNodeKind.InterfaceTypeDefinition:
                        this.VisitInterfaceType((GraphQLInterfaceTypeDefinition)astNode);
                        break;
                }
            }
        }

        protected virtual void InterfaceFieldDirectiveUsageVisited(InterfaceFieldDirectiveUsage usage) { }
        protected virtual void InterfaceTypeDirectiveUsageVisited(InterfaceTypeDirectiveUsage usage) { }
        protected virtual void ObjectFieldDirectiveUsageVisited(ObjectFieldDirectiveUsage usage) { }
        protected virtual void ObjectTypeDirectiveUsageVisited(ObjectTypeDirectiveUsage usage) { }

        private void VisitObjectType(GraphQLObjectTypeDefinition objectType)
        {
            foreach (var directive in objectType.Directives)
            {
                var usage = new ObjectTypeDirectiveUsage(objectType, directive);
                this.ObjectTypeDirectiveUsageVisited(usage);
            }

            foreach (var field in objectType.Fields)
            {
                this.VisitObjectTypeField(objectType, field);
            }
        }

        private void VisitObjectTypeField(GraphQLObjectTypeDefinition objectType, GraphQLFieldDefinition field)
        {
            foreach (var directive in field.Directives)
            {
                var usage = new ObjectFieldDirectiveUsage(objectType, field, directive);
                this.ObjectFieldDirectiveUsageVisited(usage);
            }
        }

        private void VisitInterfaceType(GraphQLInterfaceTypeDefinition interfaceType)
        {
            foreach (var directive in interfaceType.Directives)
            {
                var usage = new InterfaceTypeDirectiveUsage(interfaceType, directive);
                this.InterfaceTypeDirectiveUsageVisited(usage);
            }

            foreach (var field in interfaceType.Fields)
            {
                this.VisitInterfaceTypeField(interfaceType, field);
            }
        }

        private void VisitInterfaceTypeField(GraphQLInterfaceTypeDefinition interfaceType, GraphQLFieldDefinition field)
        {
            foreach (var directive in field.Directives)
            {
                var usage = new InterfaceFieldDirectiveUsage(interfaceType, field, directive);
                this.InterfaceFieldDirectiveUsageVisited(usage);
            }
        }
    }
}
