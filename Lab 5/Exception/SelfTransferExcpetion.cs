[System.Serializable]
public class SelfTransferException : System.Exception
{
    public SelfTransferException() { }
    public SelfTransferException(string message) : base(message) { }
    public SelfTransferException(string message, System.Exception inner) : base(message, inner) { }
    protected SelfTransferException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}