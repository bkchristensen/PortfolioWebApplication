namespace PortfolioWebApplication.Models
{
    public class Purchase : Item
    {
        #region Fields & Properties

        public int NumItems { get; set; }
        public float PurchaseCost
        {
            get
            {
                return NumItems * ItemCost;
            }
        }

        #endregion
    }

    public class Item
    {
        #region Fields & Properties
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float ItemCost { get; set; }
        #endregion
    }
}
