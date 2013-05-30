namespace ExtensibleSiteMap.VideoNode
{
    public class VideoPrice
    {
        private readonly string _price;
        private readonly string _currency;
        private readonly PurchaseType _purchaseType;
        private readonly PurchaseResolution _resolution;

        public VideoPrice(string price, string currency, PurchaseType purchaseType, PurchaseResolution resolution)
        {
            _price = price;
            _currency = currency;
            _purchaseType = purchaseType;
            _resolution = resolution;
        }

        public string Price
        {
            get { return _price; }
        }

        public string Currency
        {
            get { return _currency; }
        }

        public PurchaseType PurchaseType
        {
            get { return _purchaseType; }
        }

        public PurchaseResolution Resolution
        {
            get { return _resolution; }
        }
    }

    public enum PurchaseResolution
    {
        Undefined, HD, SD
    }

    public enum PurchaseType
    {
        Undefined, Own, Rent
    }
}