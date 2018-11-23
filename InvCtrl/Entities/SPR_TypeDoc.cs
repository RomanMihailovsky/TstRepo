using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvCtrl.Entities
{
    public class SPR_TypeDoc
    {
       [Key]
        public int Id { get; set; }
        public string Name {get;set;}
    }
}
