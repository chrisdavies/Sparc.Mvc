namespace Sparc.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public abstract class BlockablePage<TModel> : System.Web.Mvc.WebViewPage<TModel> where TModel : class
    {
        private static Dictionary<string, BlockChain> Blocks
        {
            get
            {
                const string BlocksDictionaryKey = "49EC142B-A520-4CD1-A47C-3F4FFCBCD949";
                if (HttpContext.Current.Items.Contains(BlocksDictionaryKey))
                {
                    return HttpContext.Current.Items[BlocksDictionaryKey] as Dictionary<string, BlockChain>;
                }

                var blocks = new Dictionary<string, BlockChain>(StringComparer.OrdinalIgnoreCase);
                HttpContext.Current.Items[BlocksDictionaryKey] = blocks;
                return blocks;
            }
        }

        public static BlockChain DefineBlock(string name)
        {
            return new BlockChain(name, Blocks);
        }

        public static IHtmlString RenderBlock(string name, bool required = false)
        {
            BlockChain result = null;
            IHtmlString html = null;
            var blocks = Blocks;

            if (blocks.TryGetValue(name, out result))
            {
                blocks[name] = result.InnerResult;
                html = new MvcHtmlString(result.Render());
                blocks[name] = result;
            }
            else if (required)
            {
                throw BlockChain.RequiredException(name);
            }

            return html;
        }
    }
}