using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Sparta.Core.Interfaces;

namespace Sparta.API._2
{
  /// <summary>
    /// Custom token validation handler for retrieving and validating the claims token, and setting the thread/Http Context to the current User.
    /// </summary>
    public class TokenValidationHandler : DelegatingHandler
    {
        private readonly IUnitOfWork unitOfWork;

        #region Constructors
        /// <summary>
        ///     Create the instance and initialize the UserService instance.
        /// </summary>
        public TokenValidationHandler()
        {
            unitOfWork = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
        }
        #endregion

        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        //                                                       CancellationToken cancellationToken)
        //{
        //    if (LoginCalled(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    if (DocumentationCalled(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    if (FilesCalled(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    if (PreFlightCalled(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    if (LandingPageReleaseCalendar(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    if (LandingPageFeatureFilms(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    if (ExporttoExcel(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }

        //    if (ExportDealstoExcel(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }

        //    if (SendTextFileForSAP(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    if (ExportReleaseCalendarToExcel(request))
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }

        //    HttpStatusCode statusCode;
        //    string jwtEncodedToken;
        //    string NTToken;

        //    if (!TryRetrieveToken(request, out jwtEncodedToken))
        //    {
        //        if (TryRetrieveNTToken(request, out NTToken))
        //        {
        //            Guid UserId = new Guid(NTToken.Substring(0, NTToken.IndexOf(":")));
        //            string Subject = NTToken.Substring(NTToken.IndexOf(":")+1);

        //            //get customer from Subject
        //            Core.Entities.CustomerCertificate oCustomerCertificate = unitOfWork.Repository<Core.Entities.CustomerCertificate>().GetAll()
        //                                                                                                .FirstOrDefault(c => c.Subject == Subject);

        //            if(oCustomerCertificate == null)
        //                throw  new Exception("Invalid Client Certificate");

        //            if(oCustomerCertificate.StartDate.Subtract(DateTime.Now).Days > 0)
        //                throw new Exception("Invalid Client Certificate, Starte Date is :" + oCustomerCertificate.StartDate.ToString("MM-dd-yyyy"));

        //            if(oCustomerCertificate.EndDate.HasValue && oCustomerCertificate.EndDate.Value.Subtract(DateTime.Now).Days <= 0 )
        //                throw new Exception("Invalid Client Certificate, End Date is :" + oCustomerCertificate.EndDate.Value.ToString("MM-dd-yyyy"));
        //            //get user id and customer id bonding

        //            var oUser = unitOfWork.Repository<Core.Entities.User>().GetAll()
        //                                .Include(c => c.Customer)
        //                                .Include(c => c.Customer.Roles)
        //                                .Include(c => c.Lab)
        //                                .Include(l => l.Lab.Roles)
        //                                .FirstOrDefault(u => u.Id == UserId && u.CustomerId == oCustomerCertificate.CustomerId);

        //            if (oUser == null)
        //                throw new SecurityTokenValidationException("Unauthorized user");
        //            else
        //            {
        //                SetThreadPrincipal(oUser);
        //                return base.SendAsync(request, cancellationToken);
        //            }
        //        }
        //        else
        //        {
        //            statusCode = HttpStatusCode.Unauthorized;
        //            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
        //        }
        //    }

        //    try
        //    {
        //        var symmetricKey = ConfigurationManager.AppSettings["TokenSymmetricKey"];
        //        var bytes = new byte[symmetricKey.Length * sizeof(char)];
        //        var tokenHandler = new JwtSecurityTokenHandler();

        //        Buffer.BlockCopy(symmetricKey.ToCharArray(), 0, bytes, 0, bytes.Length);

        //        var validationParameters = new TokenValidationParameters
        //        {
        //            AllowedAudience = ConfigurationManager.AppSettings["AppliesToAddress"],
        //            SigningToken = new BinarySecretSecurityToken(bytes),
        //            ValidIssuer = ConfigurationManager.AppSettings["TokenIssuerName"]
        //        };

        //        var claimsPrincipal = tokenHandler.ValidateToken(jwtEncodedToken, validationParameters);

        //        Thread.CurrentPrincipal = claimsPrincipal;
        //        HttpContext.Current.User = claimsPrincipal;

        //        return base.SendAsync(request, cancellationToken);
        //    }
        //    catch (SecurityTokenValidationException e)
        //    {
        //        statusCode = HttpStatusCode.Unauthorized;
        //    }
        //    catch (Exception)
        //    {
        //        statusCode = HttpStatusCode.InternalServerError;
        //    }

        //    return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
        //}

        //private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        //{
        //    token = null;
        //    IEnumerable<string> authzHeaders;

        //    if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
        //    {
        //        return false;
        //    }

        //    var bearerToken = authzHeaders.ElementAt(0);
        //    token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

        //    return true;
        //}

        //private static bool TryRetrieveNTToken(HttpRequestMessage request, out string token)
        //{
        //    token = null;
        //    IEnumerable<string> authzHeaders;

        //    if (!request.Headers.TryGetValues("NT-Authentication", out authzHeaders) || authzHeaders.Count() > 1)
        //    {
        //        return false;
        //    }

        //    token = authzHeaders.ElementAt(0);

        //    return true;
        //}

        //private static bool LoginCalled(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "GET"
        //           && request.RequestUri.Query.ToUpper().Contains("?LOGINNAME=")
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "USERS";
        //}

        //private static bool DocumentationCalled(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "GET"
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "SWAGGER";
        //}

        //private static bool FilesCalled(HttpRequestMessage request)
        //{
        //    return request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "FILES";
        //}

        //// allow API calls from select Non-Theatrical APIs
        //private static bool LandingPageReleaseCalendar(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "GET"
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "GETTITLESFORRELEASECALENDAR";
        //}

        //private static bool LandingPageFeatureFilms(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "GET"
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "GETTITLESFORFEATUREFILMS";
        //}

        //private static bool ExporttoExcel(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "POST"
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "EXPORTWORKORDERSQUEUEFORINVOICING";
        //}

        //private static bool ExportDealstoExcel(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "POST"
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "EXPORTDEALS";
        //}

        //private static bool SendTextFileForSAP(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "POST"
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "SENDTEXTFILEFORSAP";
        //}

        //private static bool ExportReleaseCalendarToExcel(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "POST"
        //           && request.RequestUri.Segments.Length == 3
        //           && request.RequestUri.Segments[0] == @"/"
        //           && request.RequestUri.Segments[1].ToUpper() == @"ATHENSAPI2/"
        //           && request.RequestUri.Segments[2].ToUpper() == "EXPORTRELEASECALENDAR";
        //}

        //private static bool PreFlightCalled(HttpRequestMessage request)
        //{
        //    return request.Method.Method == "OPTIONS";
        //}

        //private static void SetThreadPrincipal(User user)
        //{
        //    IEnumerable<string> roles;
        //    if (user.Lab != null)
        //        roles = user.Customer.Roles.Select(r => r.Name).Union(user.Lab.Roles.Select(r => r.Name));
        //    else
        //        roles = user.Customer.Roles.Select(r => r.Name);

        //    var identity = new GenericIdentity(user.Id.ToString(), "ClientCertificate");
        //    var principal = new GenericPrincipal(identity, roles.ToArray());

        //    Thread.CurrentPrincipal = principal;
        //    HttpContext.Current.User = principal;
        //}
    }
}