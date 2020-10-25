[System.Serializable]
public class AddProductException : System.Exception
{
    public AddProductException() { }
    public AddProductException(string message) : base(message) { }
    public AddProductException(string message, System.Exception inner) : base(message, inner) { }
    protected AddProductException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}