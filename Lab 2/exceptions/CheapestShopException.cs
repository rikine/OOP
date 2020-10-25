[System.Serializable]
public class CheapestShopException : System.Exception
{
    public CheapestShopException() { }
    public CheapestShopException(string message) : base(message) { }
    public CheapestShopException(string message, System.Exception inner) : base(message, inner) { }
    protected CheapestShopException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}