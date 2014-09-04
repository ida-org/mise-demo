using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdaToolkit
{
    /// <summary>
    /// This class holds the attributes needed for the SAML assertion.
    /// In a real world situation, when deploying the MISE services, this class would
    /// include an ADO or LINQ statement to get these attribute values for the user from the
    /// database and populate their values.  A sample (commented out) has been provided.
    /// </summary>
    public class AttributeHolder
    {
        public String Id { get; set; }
        public String ElectronicEntityId { get; set; }
        public String FullName { get; set; }
        public DateTime IssueInstant { get; set; }
        public Boolean LEI { get; set; }
        public Boolean COI { get; set; }
        public Boolean PPI { get; set; }
        public String Scope { get; set; }
        public List<String> CitizenCodes { get; set; }

        //public List<AttributeHolder> GetUserSamlAttributes(string userName)
        //{
            //List<AttributeHolder> userAttributes = new List<AttributeHolder>();

            //Assumes an entity model for the database named TrustedSystemDb, 
            //and database tables named UserInfo & UserCitizenShip

            //---->BEGIN SAMPLE<---

            //TrustedSystemDb db = new TrustedSystemDb;
            //var q = (from user in db.UserInfo
            //        join c in db.UserCitizenShip on user.UserId equals c.UserId
            //        where user.UserName == userName
            //        select new List<AttributeHolder>
            //         {
            //                  FullName = user.FullName
            //                  LEI = user.IsLEI
            //                  COI = user.IsCOI
            //                  PPI = user.IsPPI
            //                  CitizenShipCodes = c.CitizenShipCodes
            //        }).ToList<AttributeHolder();

            //return userAttributes;
        //}
    }
}
