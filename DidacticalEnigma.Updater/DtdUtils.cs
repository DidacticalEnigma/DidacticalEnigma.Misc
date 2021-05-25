using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace DidacticalEnigma.Updater
{
    public static class DtdUtils
    {
        private static readonly XmlReaderSettings xmlSettings = new XmlReaderSettings
        {
            CloseInput = false,
            DtdProcessing = DtdProcessing.Parse, // we have local entities
            XmlResolver = null, // we don't want to resolve against external entities
            MaxCharactersFromEntities = 1024,
            IgnoreComments = true
        };
        
        private static readonly Regex dtdDirectiveRegex = new Regex("<![A-Za-z].*?>");
        
        public static IEnumerable<string> GetDtdDirectives(Stream stream)
        {
            using (var xmlReader = XmlReader.Create(stream, xmlSettings))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.DocumentType)
                    {
                        return dtdDirectiveRegex.Matches(xmlReader.Value)
                            .Select(m => m.Groups[0].Value)
                            .ToList();
                    }

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        break;
                    }
                }
                return Enumerable.Empty<string>();
            }
        }
    }
}