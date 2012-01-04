namespace Sparc.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.WebPages;

    public class BlockChain : IHtmlString
    {
        private string name;
        private Func<dynamic, HelperResult> content;
        private bool overwritesParents;
        private BlockChain innerResult;
        private Dictionary<string, BlockChain> children;

        public BlockChain(string name, Dictionary<string, BlockChain> children)
        {
            this.name = name;
            this.children = children;
            children.TryGetValue(name, out this.innerResult);
        }

        public BlockChain InnerResult 
        {
            get { return this.innerResult; } 
        }

        private bool IsAlreadyDefined
        {
            get
            {
                return this.innerResult != null;
            }
        }

        private Func<dynamic, HelperResult> Content
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
                    this.children[this.name] = this;
                }
            }
        }

        public string Render()
        {
            return this.Content == null ? string.Empty : this.Content(null).ToHtmlString();
        }

        public string ToHtmlString()
        {
            return null;
        }

        public BlockChain IfDefined(Func<dynamic, HelperResult> content)
        {
            if (this.IsAlreadyDefined)
            {
                this.Content = content;
            }

            return this;
        }

        public BlockChain Else(Func<dynamic, HelperResult> content)
        {
            if (this.Content == null)
            {
                this.Content = content;
            }

            return this;
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
                throw RequiredException(this.name);
            }

            return this;
        }

        public BlockChain As(Func<dynamic, HelperResult> content)
        {
            this.Content = content;
            return this;
        }

        internal static Exception RequiredException(string name)
        {
            return new HttpException("The required section '" + name + "' has not been defined.");
        }
    }
}
