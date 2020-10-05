using System;

public class NotFoundException : Exception
{
    // This is a custom exception

    public NotFoundException() : base() { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, SystemException inner) : base(message, inner) { }

    protected NotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

}