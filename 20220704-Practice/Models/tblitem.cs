using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _20220704_Practice.Models
{
    public class tblitem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDetail { get; set; }
        public DateTime DateTime { get; set; }
    }
}
