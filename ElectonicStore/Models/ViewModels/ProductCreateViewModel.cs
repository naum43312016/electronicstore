using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectonicStore.Models.ViewModels
{
    public class ProductCreateViewModel
    {
        public List<Category> Categories { get; set; }
        public Product product { get; set; }
    }
}
