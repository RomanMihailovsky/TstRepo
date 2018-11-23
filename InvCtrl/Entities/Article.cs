using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InvCtrl.Entities
{
    public class Article // Товар
    {
        [Key]
        public int ArticleId { get; set; }  // Id
        public string Name { get; set; } // Наименование 
        public decimal Price { get; set; } // Цена
        public int Cnt { get; set; } // Кол-во

    }
}
