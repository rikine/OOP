[System.Serializable]
public class BuyProductException : System.Exception
{
    public BuyProductException() { }
    public BuyProductException(string message) : base(message) { }
    public BuyProductException(string message, System.Exception inner) : base(message, inner) { }
    protected BuyProductException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}