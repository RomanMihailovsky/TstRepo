using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvCtrl.Entities
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }  // № док-та

        public string Type { get; set; }  // Вид док-та

        public string Status { get; set; } //Состояние док-та

        public DateTime DateStatus { get; set; } //Дата

        public decimal Amount { get; set; } //Сумма

    }
}
