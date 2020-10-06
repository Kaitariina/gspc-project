using System;
using System.Net;

public class PlayerBannedException : Exception
{
    public HttpStatusCode Status { get; set; }
    public PlayerBannedException(HttpStatusCode statusCode, string message) : base(message)
    {
        Status = statusCode;
    }
    public PlayerBannedException() : base() { }
    public PlayerBannedException(string message) : base(message) { }
    public PlayerBannedException(string message, SystemException inner) : base(message, inner) { }

    protected PlayerBannedException(System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
