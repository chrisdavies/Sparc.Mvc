namespace Sparc.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public class BlockChain : IHtmlString
    {
        private Func<dynamic, IHtmlString> content;
        private bool overwritesParents;
        private BlockChain innerResult;
        private Dictionary<string, BlockChain> children;

        public BlockChain(string name, Dictionary<string, BlockChain> children)
        {
            this.Name = name;
            this.children = children;
            children.TryGetValue(name, out this.innerResult);
        }
        
        public BlockChain InnerResult 
        {
            get { return this.innerResult; } 
        }

        protected string Name { get; set; }

        protected Func<dynamic, IHtmlString> Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
                if (!this.IsAlreadyDefined || !this.InnerResult.overwritesParents)
                {
                    this.children[this.Name] = this;
                }
            }
        }

        private bool IsAlreadyDefined
        {
            get
            {
                return this.innerResult != null;
            }
        }

        public string Render()
        {
            var hasContent = this.Content == null;

            try
            {
                // Pop this instance from the 'stack' to prevent stack overflow
                // when children call render on the same block-name.
                this.children[this.Name] = this.innerResult;
                return hasContent ? string.Empty : this.Content(null).ToHtmlString();
            }
            finally
            {
                // Push this instance back onto the 'stack' now that children
                // have rendered.
                if (hasContent)
                {
                    this.children[this.Name] = this;
                }
            }
        }

        public virtual string ToHtmlString()
        {
            return null;
        }

        public BlockChain IfDefined(Func<dynamic, IHtmlString> content)
        {
            if (this.IsAlreadyDefined)
            {
                this.Content = content;
            }

            return this;
        }

        public BlockChain IfDefined(string literal)
        {
            return this.IfDefined(this.EscapedContent(literal));
        }

        public BlockChain IfDefinedRaw(string literal)
        {
            return this.IfDefined(this.RawContent(literal));
        }

        public BlockChain Else(Func<dynamic, IHtmlString> content)
        {
            if (this.Content == null)
            {
                this.Content = content;
            }

            return this;
        }

        public BlockChain Else(string literal)
        {
            return this.Else(this.EscapedContent(literal));
        }

        public BlockChain ElseRaw(string literal)
        {
            return this.Else(this.RawContent(literal));
        }

        public BlockChain OverwriteParents()
        {
            this.overwritesParents = true;
            return this;
        }

        public BlockChain Required()
        {
            if (!this.IsAlreadyDefined)
            {
                throw RequiredException(this.Name);
            }

            return this;
        }

        public BlockChain As(string literal)
        {
            return this.As(this.EscapedContent(literal));
        }

        public BlockChain AsRaw(string literal)
        {
            return this.As(this.RawContent(literal));
        }

        public BlockChain As(Func<dynamic, IHtmlString> content)
        {
            this.Content = content;
            return this;
        }

        internal static Exception RequiredException(string name)
        {
            return new HttpException("The required section '" + name + "' has not been defined.");
        }
        
        private Func<dynamic, IHtmlString> EscapedContent(string literal)
        {
            return d => { return new HtmlString(HttpUtility.HtmlEncode(literal)); };
        }

        private Func<dynamic, IHtmlString> RawContent(string literal)
        {
            return d => { return new HtmlString(literal); };
        }
    }
}
