[System.Serializable]
public class InnerAccountOrClientException : System.Exception
{
    public InnerAccountOrClientException() { }
    public InnerAccountOrClientException(string message) : base(message) { }
    public InnerAccountOrClientException(string message, System.Exception inner) : base(message, inner) { }
    protected InnerAccountOrClientException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}