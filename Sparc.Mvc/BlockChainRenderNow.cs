namespace Sparc.Mvc
{
    using System.Collections.Generic;

    public class BlockChainRenderNow : BlockChain
    {
        public BlockChainRenderNow(string name, Dictionary<string, BlockChain> children)
            : base(name, children)
        {
        }

        public override string ToHtmlString()
        {
            if (this.Content == null && this.InnerResult != null)
            {
                return this.InnerResult.Render();
            }

            return this.Render();
        }
    }
}
