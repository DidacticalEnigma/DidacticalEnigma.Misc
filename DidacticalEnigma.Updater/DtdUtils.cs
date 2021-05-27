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
        
        private static readonly string NamePattern = "[:A-Za-z_\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD][:A-Za-z_\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02FF\u0370-\u037D\u037F-\u1FFF\u200C-\u200D\u2070-\u218F\u2C00-\u2FEF\u3001-\uD7FF\uF900-\uFDCF\uFDF0-\uFFFD.0-9\u00B7\u0300-\u036F\u203F-\u2040-]*";
        private static readonly string WhitespaceCharacterPattern = "[ \t\r\n]";

        private static readonly Regex EntityRegex = new Regex(
            "<!ENTITY" + 
            $"{WhitespaceCharacterPattern}+" +
            $"({NamePattern})" +
            $"{WhitespaceCharacterPattern}+" +
            "\"([^%&\"]*)\"" +
            $"{WhitespaceCharacterPattern}*" +
            ">");

        public static IReadOnlyList<string> JMDictXmlDeclarations(Stream stream)
        {
            var directives = DtdUtils.GetDtdDirectives(stream)
                .Select(decl =>
                {
                    var match = EntityRegex.Match(decl);
                    if (match.Success)
                    {
                        return $"<!ENTITY {match.Groups[1]} \"...\">";
                    }
                    else
                    {
                        return decl;
                    }
                })
                .OrderBy(x => x)
                .ToList();

            return directives;
        }
    }
}