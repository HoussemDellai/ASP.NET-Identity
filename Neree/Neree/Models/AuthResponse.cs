using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neree.Models
{
    public class AuthResponse
    {
        /// <summary>   
        /// Tells if the API call was processed successfully.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Result received from the API.
        /// Could be JSON or String.
        /// </summary>
        public string Result { get; set; }      
    }
}
