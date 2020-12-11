[System.Serializable]
public class NotEnoughMoneyException : System.Exception
{
    public NotEnoughMoneyException() { }
    public NotEnoughMoneyException(string message) : base(message) { }
    public NotEnoughMoneyException(string message, System.Exception inner) : base(message, inner) { }
    protected NotEnoughMoneyException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}