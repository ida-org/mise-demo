using System;

namespace MdaToolkit
{
    public class ServiceEndPointManager
    {
        private string ResourceBase = "https://107.23.66.168:9443/miseresources/";
        private string ServicesBase = "https://107.23.66.168:9443/services/MDAService/";
        private string LoginBase = "https://107.23.66.168:9443/services/";

        /// <summary>
        /// This method build the "RESTFul" endpoint dynamically by taking in the following parameters.
        /// </summary>
        /// <param name="service">Publish, Search, Retrieve, Version, Login, Trust Fabric</param>
        /// <param name="operation">The IEPD Type to perfrom the operation against.
        /// Either: pos, ian, noa, or loa
        /// </param>
        /// <param name="id">The unquie ID given to the message (only applies to publish and retrieve)</param>
        /// <returns>String: the endpoint </returns>
        public string BuildServiceEndPoint(string service, string operation, string id)
        {
            string endPoint = String.Empty;
            switch (service)
            {
                case "TrustFabric":
                    endPoint = ResourceBase + "TrustFabric.xml";
                    break;
                case "Publish":
                case "Delete":
                    endPoint = ServicesBase + "publish/" + operation + "/" + id;
                    break;
                case "Search":
                    endPoint = ServicesBase + "search/" + operation;
                    break;
                case "Retrieve":
                    endPoint = ServicesBase + "retrieve/" + operation;// + "/" + id;
                    break;
                case "Version":
                    endPoint = ServicesBase + "publish/" + operation;
                    break;
                case "Login":
                    endPoint = LoginBase + "MDAUserSessionService/login";
                    break;
                default:
                    break;
            }

            return endPoint;
        }
    }
}
