[System.Serializable]
public class CancelTransactionException : System.Exception
{
    public CancelTransactionException() { }
    public CancelTransactionException(string message) : base(message) { }
    public CancelTransactionException(string message, System.Exception inner) : base(message, inner) { }
    protected CancelTransactionException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}