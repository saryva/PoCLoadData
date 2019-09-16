using System;

namespace Poc.Retail.Domain.Entities
{
    public partial class StockDetail
    {
        public string PointOfSale { get; set; }
        public string Product { get; set; }
        public DateTime Date { get; set; }
        public int Stock { get; set; }
        public bool IsImport { get; set; }
        public int Id { get; set; }
    }
}
