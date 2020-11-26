[System.Serializable]
public class WrongClearArgumentsException : System.Exception
{
    public WrongClearArgumentsException() { }
    public WrongClearArgumentsException(string message) : base(message) { }
    public WrongClearArgumentsException(string message, System.Exception inner) : base(message, inner) { }
    protected WrongClearArgumentsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}