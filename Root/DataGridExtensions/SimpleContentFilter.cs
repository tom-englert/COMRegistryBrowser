using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataGridExtensions
{
    /// <summary>
    /// A content filter using a simple "contains" string comparison to match the content and the value.
    /// </summary>
    public class SimpleContentFilter : IContentFilter
    {
        private string content;
        private StringComparison stringComparison;

        public SimpleContentFilter(string content, StringComparison stringComparison)
        {
            this.content = content;
            this.stringComparison = stringComparison;
        }

        #region IFilter Members

        public bool IsMatch(object value)
        {
            if (value == null)
                return false;

            return value.ToString().IndexOf(content, stringComparison) >= 0;
        }

        #endregion
    }

    /// <summary>
    /// Factory to create a <see cref="SimpleContentFilter"/>
    /// </summary>
    public class SimpleContentFilterFactory : IContentFilterFactory
    {
        public SimpleContentFilterFactory()
            : this(StringComparison.CurrentCultureIgnoreCase)
        {
        }

        public SimpleContentFilterFactory(StringComparison stringComparison)
        {
            this.StringComparison = stringComparison;
        }

        public StringComparison StringComparison { get; set; }

        #region IFilterFactory Members

        public IContentFilter Create(object content)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            return new SimpleContentFilter(content.ToString(), StringComparison);
        }

        #endregion
    }
}
