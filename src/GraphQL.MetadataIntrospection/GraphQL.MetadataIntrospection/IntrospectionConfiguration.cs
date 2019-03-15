using System;
using System.Collections.Generic;
using GraphQL.MetadataIntrospection.Schema;

namespace GraphQL.MetadataIntrospection
{
    /// <summary>
    /// Metadata introspection configuration. Field name, included/excluded directives, etc. can be configured using this
    /// By default all directives are included
    /// </summary>
    public class IntrospectionConfiguration
    {
        internal string FieldName { get; set; }
        internal List<string> IncludedDirectives { get; } = new List<string>();
        internal List<string> ExcludedDirectives { get; } = new List<string>();

        /// <summary>
        /// Includes directive to the introspection. By default all directives are included.
        /// </summary>
        /// <param name="name">Directive name to include</param>
        /// <returns><see cref="IntrospectionConfiguration"/> for chaining</returns>
        public IntrospectionConfiguration IncludeDirective(string name)
        {
            this.IncludedDirectives.Add(name);
            
            return this;
        }

        /// <summary>
        /// Excludes directive from the introspection. By default all directives are included.
        /// </summary>
        /// <param name="name">Directive name to exclude</param>
        /// <returns><see cref="IntrospectionConfiguration"/> for chaining</returns>
        public IntrospectionConfiguration ExcludeDirective(string name)
        {
            this.ExcludedDirectives.Add(name);
            
            return this;
        }
        
        /// <summary>
        /// Sets introspection field name visible to schema client
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="IntrospectionConfiguration"/> for chaining</returns>
        /// <exception cref="ArgumentException">Thrown when passed field name does not satisfy GraphQL naming specification</exception>
        public IntrospectionConfiguration SetIntrospectionFieldName(string name)
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