using System;
using System.Collections.Generic;
using System.Text;
using WoringTask.Core.Data;

namespace WoringTask.Core.DomainEntity
{
    public class Product:BaseEntity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
