using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MdaToolkit
{
    public static class HttpExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="queryParams">
        /// The query params are defined in the Search Specification.  The simplest way of passing
        /// these would be that the id or name of the control from the UI matches the name of the query 
        /// parameter that needs to be passed.  From this, the id or name could be added along with it's value 
        /// to the StringDictionary.  
        /// 
        /// The order in which parameters appear in the query string should not matter.
        /// </param>
        /// <returns></returns>
        public static Uri AddQuery(this Uri uri, Dictionary<string, string> queryParams)
        {
            var ub = new UriBuilder(uri);
            var queryString = HttpUtility.ParseQueryString(uri.Query);
            foreach (var de in queryParams)
            {
                if (!String.IsNullOrWhiteSpace(de.Value.ToString()))
                    queryString.Add(de.Key.ToString(), de.Value.ToString());
            }
            ub.Query = queryString.ToString();
            string s = HttpUtility.UrlEncode(ub.Uri.ToString());

            return ub.Uri;
        }
    }
}
