using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;


namespace NaplampaWcfHost.Util
{
    public class CustomUserNamePasswordValidator : UserNamePasswordValidator
    {
        // This method validates users. It allows in two users, test1 and test2 
        // with passwords 1tset and 2tset respectively.
        // This code is for illustration purposes only and 
        // must not be used in a production environment because it is not secure.	
        public override void Validate(string userName, string password)
        {
            int personId = -1;

            if ((userName == null) || (null == password) || (!Int32.TryParse(userName, out personId)))
            {
                throw new ArgumentNullException();
            }

            NaplampaEntities context = new NaplampaEntities();
            Person result = context.Person.FirstOrDefault<Person>(p => p.PersonId == personId);

            if (result != null)
            {
                byte[] buffer = Encoding.Default.GetBytes(password);
                SHA512CryptoServiceProvider cryptoTransformSHA512 = new SHA512CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA512.ComputeHash(buffer)).Replace("-", "");

                if (hash != result.PasswordHash)
                {
                    // This throws an informative fault to the client.
                    throw new FaultException("Unknown Username or Incorrect Password");
                    // When you do not want to throw an informative fault to the client,
                    // throw the following exception.
                    // throw new SecurityTokenException("Unknown Username or Incorrect Password");
                }
            }
            else
            {
                throw new FaultException("Unknown Username or Incorrect Password");
            }
        }
    }
}
