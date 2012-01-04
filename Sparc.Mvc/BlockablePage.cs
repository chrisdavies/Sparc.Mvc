namespace Sparc.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public abstract class BlockablePage<TModel> : WebViewPage<TModel> where TModel : class
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

        /// <summary>
        /// Defines a block, to be rendered at a later time.
        /// </summary>
        /// <param name="name">The name of the block to be rendered.</param>
        /// <returns>The block chain.</returns>
        public static BlockChain DefineBlock(string name)
        {
            return new BlockChain(name, Blocks);
        }

        /// <summary>
        /// Defines and renders a block.
        /// </summary>
        /// <param name="name">The name of the block to be rendered.</param>
        /// <returns>The block chain.</returns>
        public static BlockChain RenderBlock(string name)
        {
            return new BlockChainRenderNow(name, Blocks);
        }
    }
}