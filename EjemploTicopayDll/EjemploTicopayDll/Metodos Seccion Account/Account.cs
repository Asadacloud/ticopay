using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Authentication;
using TicoPayDll.Response;

namespace EjemploTicopayDll.Metodos_Seccion_Account
{
    public class ApiAccount
    {
        /// <summary>
        /// Autentifica un usuario en Ticopay, si las credenciales son validas devuelve el token de sesión. (El Token expira en 25 min).
        /// </summary>
        /// <param name="tenancy">Tenant o Sub Dominio.</param>
        /// <param name="user">Usuario.</param>
        /// <param name="password">Clave.</param>
        /// <returns></returns>
        public static string AutentificarUsuario(string tenancy, string user, string password)
        {

            TicoPayDll.Response.Response respuestaServicio;
            TicoPayDll.Authentication.UserCredentials credenciales = new TicoPayDll.Authentication.UserCredentials();
            credenciales.tenancyName = tenancy;
            credenciales.usernameOrEmailAddress = user;
            credenciales.password = password;
            respuestaServicio = TicoPayDll.Authentication.Authentication.Authenticate(credenciales).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonAuthentication token = JsonConvert.DeserializeObject<JsonAuthentication>(respuestaServicio.result);
                return token.objectResponse.tokenAuthenticate;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }


    }
}
