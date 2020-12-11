[System.Serializable]
public class AddMoneyException : System.Exception
{
    public AddMoneyException() { }
    public AddMoneyException(string message) : base(message) { }
    public AddMoneyException(string message, System.Exception inner) : base(message, inner) { }
    protected AddMoneyException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}