using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvCtrl.Entities
{
    public class CartLine
    {
        public Article Article { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart // Корзина
    {

        private List<CartLine> lineCollection = new List<CartLine>();

        // Добавление в корзину
        public void AddItem(Article article, int quantity)
        {
            
            CartLine line = lineCollection
                .Where(g => g.Article.ArticleId == article.ArticleId)
                .FirstOrDefault();

            if (line == null)
            {
                if ( article.Cnt < quantity)
                {
                    quantity = article.Cnt;
                }

                lineCollection.Add(new CartLine
                {
                    Article = article,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }


        // Remove список
        public void RemoveLine(Article article)
        {
            // Remove список List<CartLine>
            lineCollection.RemoveAll(l => l.Article.ArticleId == article.ArticleId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Article.Price * e.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

}
