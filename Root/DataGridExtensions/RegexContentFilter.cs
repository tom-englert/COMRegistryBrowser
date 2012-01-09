using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataGridExtensions
{
    /// <summary>
    /// A content filter using the content as a regular expression to match the string representation of the value.
    /// </summary>
    public class RegexContentFilter : IContentFilter
    {
        Regex filterRegex;

        public RegexContentFilter(string expression, RegexOptions regexOptions)
        {
            try
            {
                filterRegex = new Regex(expression, regexOptions);
            }
            catch (ArgumentException)
            {
                // invalid user input, just go with a null expression.
            }
        }

        #region IColumnFilter Members

        public bool IsMatch(object value)
        {
            if (filterRegex == null)
                return true;
            if (value == null)
                return false;

            return filterRegex.IsMatch(value.ToString());
        }

        #endregion
    }

    /// <summary>
    /// Factory to create a <see cref="RegexContentFilter"/>
    /// </summary>
    public class RegexContentFilterFactory : IContentFilterFactory
    {
        public RegexContentFilterFactory()
            : this(RegexOptions.IgnoreCase)
        {
        }

        public RegexContentFilterFactory(RegexOptions regexOptions)
        {
            this.RegexOptions = regexOptions;
        }

        RegexOptions RegexOptions { get; set; }

        #region IFilterFactory Members

        public IContentFilter Create(object content)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            return new RegexContentFilter(content.ToString(), RegexOptions);
        }

        #endregion
    }
}
