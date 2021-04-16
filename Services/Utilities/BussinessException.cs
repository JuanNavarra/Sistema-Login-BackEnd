namespace Services
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    public class BussinessException : Exception
    {
        #region Properties
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        #endregion
        #region Constructors
        public BussinessException()
        {
        }

        public BussinessException(string message) : base(message)
        {
        }

        public BussinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BussinessException(HttpStatusCode statusCode, string reasonPhrase)
        {
            this.StatusCode = statusCode;
            this.ReasonPhrase = reasonPhrase;
        }

        protected BussinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion
    }
}
