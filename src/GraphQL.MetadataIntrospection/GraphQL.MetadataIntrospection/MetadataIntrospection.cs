using System;
using System.Collections.Generic;
using GraphQL.MetadataIntrospection.Schema;

namespace GraphQL.MetadataIntrospection
{
    public class MetadataIntrospection
    {
        internal string FieldName { get; set; }
        internal List<string> IncludedDirectives { get; set; }
        internal List<string> ExcludedDirectives { get; set; }

        public MetadataIntrospection IncludeDirective(string name)
        {
            this.IncludedDirectives.Add(name);
            
            return this;
        }

        public MetadataIntrospection ExcludeDirective(string name)
        {
            this.ExcludedDirectives.Add(name);
            
            return this;
        }
        
        public MetadataIntrospection SetMetadataField(string name)
        {
            var validationResult = MetadataFieldNameValidator.Validate(name);

            if (!validationResult.Success)
            {
                throw new ArgumentException(validationResult.Error, nameof(name));
            }

            this.FieldName = name;

            return this;
        }
    }
}