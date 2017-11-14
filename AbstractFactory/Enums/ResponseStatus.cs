using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractory.Enums
{
    public enum ResponseStatus
    {
        /// <summary>
        /// The Factory found everything without error
        /// </summary>
        Success,

        /// <summary>
        /// The Factory had problems finding a tag
        /// </summary>
        NotExist,

        /// <summary>
        /// There was a problem with execution outside of the Factory
        /// </summary>
        Problem
    }
}
