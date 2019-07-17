using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptlex
{
    class LexActivatorException : Exception
    {
        public int code;
        public LexActivatorException()
        {
        }

        public LexActivatorException(int code)
        {
            this.code = code;
        }
    }
}