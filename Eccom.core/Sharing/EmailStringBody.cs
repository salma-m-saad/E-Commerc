using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Sharing
{
    public class EmailStringBody
    {
        public static string Send(string email,string token,string component,string message) 
        {
            string encodedToken = Uri.EscapeDataString(token);
            return $@"
                       <html>
                            <head></head>
                            <body>
                                <h1>{message}</h1>
                                <p>Click the link below to {component}:</p>
                                <a href=""https://localhost:5001/api/Auth/{component}?email={email}&code={encodedToken}"">{message}</a>
                            </body>
                       </html>
                    ";
        }
    }
}
