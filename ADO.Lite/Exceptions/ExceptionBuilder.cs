using System;

namespace ADO.Lite.Exceptions
{
    public class ExceptionBuilder
    {


        public ExceptionBuilder()
        {

        }


        public static Common.ExceptionMessage GetException(Exception exception)
        {

            return new Common.ExceptionMessage { Message = exception.Message, Trace = exception.ToString() };

        }


    }






}
