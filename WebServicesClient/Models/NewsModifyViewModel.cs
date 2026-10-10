using System.Collections.Generic;

namespace WebTeploobmenAppClient.Models
{
    public class NewsModifyViewModel
    {
        public NewsViewModel News { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
}
