﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.Documents.OData.Sql
{
	/// <summary>
	/// abstract class for query formatter used in <see cref="ODataNodeToStringBuilder"/>
	/// </summary>
	public abstract class QueryFormatterBase
	{
		/// <summary>
		/// method to translate fieldName
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns>returns translated field</returns>
		public abstract string TranslateFieldName(string fieldName);

		/// <summary>
		/// method to translate enum values
		/// </summary>
		/// <param name="value">the enum value</param>
		/// <param name="nameSpace">Namespace of the enum type</param>
		/// <returns>returns an enumValue without the namespace</returns>
		public abstract string TranslateEnumValue(string enumValue, string nameSpace);

		/// <summary>
		/// method to convert parent/child field
		/// </summary>
		/// <param name="source">the parent field</param>
		/// <param name="edmProperty">the child field</param>
		/// <returns>returns translated parent and child</returns>
		public abstract string TranslateSource(string source, string edmProperty);

		/// <summary>
		/// method to convert function name
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns>returns a translated function name</returns>
		public abstract string TranslateFunctionName(string functionName);
	}

}
