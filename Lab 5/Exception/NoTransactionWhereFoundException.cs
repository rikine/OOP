[System.Serializable]
public class NoTransactionWhereFoundException : System.Exception
{
    public NoTransactionWhereFoundException() { }
    public NoTransactionWhereFoundException(string message) : base(message) { }
    public NoTransactionWhereFoundException(string message, System.Exception inner) : base(message, inner) { }
    protected NoTransactionWhereFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}