using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace MdaToolkit
{
    public class SamlBuilder
    {
        private X509Certificate2 _privateCert;

        public SamlBuilder(X509Certificate2 privateCert)
        {
            this._privateCert = privateCert;
        }

        #region Create All Required Namespaces

        XNamespace xs = "http://www.w3.org/2001/XMLSchema";
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        XNamespace saml2 = "urn:oasis:names:tc:SAML:2.0:assertion";

        #endregion

        public String BuildSamlAssertion(AttributeHolder attributes)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //required by World Wide Web Consortium (W3C) for digitial signatures
            xmlDoc.PreserveWhitespace = true;

            #region Build the SAML 2.0 Root Element

            XElement root = new XElement(saml2 + "Assertion",
                new XAttribute(XNamespace.Xmlns + "saml2", saml2.ToString()),
                new XAttribute(XNamespace.Xmlns + "xs", xs.ToString()),
                new XAttribute("ID", attributes.Id),
                new XAttribute("IssueInstant", attributes.IssueInstant),
                new XAttribute("Version", "2.0"));
            XElement issuer = new XElement(saml2 + "Issuer",
                new XAttribute("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:entity"), attributes.ElectronicEntityId);
            root.Add(issuer);
            xmlDoc.Load(root.CreateReader());

            #endregion

            #region Build the Conditions Block
            /*.Net seems to process much quicker than JAVA, and so we set the NotBefore condition back by
            10 seconds so that when the request is sent we don't run into problems of not meeting our own 
            conditions on the ISI.  This was a problem found during testing, and this seems to have resolved
            the issue. */
            TimeSpan ts = new TimeSpan(0, 0, 10);

            XElement conditions = new XElement(saml2 + "Conditions",
                new XAttribute("NotBefore", DateTime.Now.Subtract(ts).ToUniversalTime()),
                new XAttribute("NotOnOrAfter", DateTime.Now.AddMonths(12).ToUniversalTime()));
            XElement restrictions = new XElement(saml2 + "AudienceRestriction");
            XElement audience = new XElement(saml2 + "Audience", "urn:mise:all");
            restrictions.Add(new XElement(audience));
            conditions.Add(new XElement(restrictions));

            XmlNode conditionNode = xmlDoc.ReadNode(conditions.CreateReader());
            xmlDoc.DocumentElement.AppendChild(conditionNode);

            #endregion

            #region Build the Attribute Statement Block

            XElement attributeHolder = new XElement(saml2 + "AttributeStatement");
            attributeHolder.Add(this.AssertionBuilder("ElectronicIdentityId", "mise:1.2:user:ElectronicIdentityId", "urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified", attributes.ElectronicEntityId));
            attributeHolder.Add(this.AssertionBuilder("FullName", "mise:1.2:user:FullName", "urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified", attributes.FullName));
            attributeHolder.Add(this.AssertionBuilder("CitizenshipCode", "mise:1.2:user:CitizenshipCode", "urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified", attributes.CitizenCodes));
            attributeHolder.Add(this.AssertionBuilder("Scope", "mise:1.2:user:Scope", "urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified", attributes.Scope));
            attributeHolder.Add(this.AssertionBuilder("LawEnforcementIndicator", "mise:1.2:user:LawEnforcementIndicator", "urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified", attributes.LEI.ToString()));
            attributeHolder.Add(this.AssertionBuilder("PrivacyProtectedIndicator", "mise:1.2:user:PrivacyProtectedIndicator", "urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified", attributes.PPI.ToString()));
            attributeHolder.Add(this.AssertionBuilder("COIIndicator", "mise:1.2:user:COIIndicator", "urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified", attributes.COI.ToString()));
            XmlNode attrNode = xmlDoc.ReadNode(attributeHolder.CreateReader());
            xmlDoc.DocumentElement.AppendChild(attrNode);

            #endregion

            //need to sign after all other nodes have been created
            #region Build the Signature Block

            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = _privateCert.PrivateKey;

            Reference reference = new Reference();
            reference.Uri = "";

            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform(true);
            env.Algorithm = SignedXml.XmlDsigEnvelopedSignatureTransformUrl;
            reference.AddTransform(env);

            //With respect to the trustfabric the transform c14t with comments are required 
            //for the interoperability between c# and java 
            XmlDsigC14NWithCommentsTransform c14t = new XmlDsigC14NWithCommentsTransform();
            c14t.Algorithm = SignedXml.XmlDsigC14NWithCommentsTransformUrl;
            reference.AddTransform(c14t);

            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data(_privateCert);
            keyInfo.AddClause(keyInfoData);

            signedXml.KeyInfo = keyInfo;
            signedXml.AddReference(reference);
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            signedXml.ComputeSignature();

            XmlElement assignedSignature = signedXml.GetXml();
            //insert the cert in the correct document location (not necassary will work if appendchild is used)
            xmlDoc.DocumentElement.InsertBefore(xmlDoc.ImportNode(assignedSignature, true), conditionNode);

            #endregion

            return xmlDoc.OuterXml;
        }

        private XElement AssertionBuilder(String friendlyName, String name, String nameFormat, String val)
        {
            return this.AssertionBuilder(friendlyName, name, nameFormat, new List<String>() { val });
        }

        private XElement AssertionBuilder(String friendlyName, String name, String nameFormat, List<String> values)
        {
            XElement attrHolder = new XElement(saml2 + "Attribute",
                           new XAttribute("FriendlyName", friendlyName),
                           new XAttribute("Name", name),
                           new XAttribute("NameFormat", nameFormat));
            foreach (String val in values)
            {
                XElement valueHolder = new XElement(saml2 + "AttributeValue",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi.ToString()), new XAttribute(xsi + "type", "xs:string"), val);
                attrHolder.Add(valueHolder);
            }

            return attrHolder;
        }
    }
}
