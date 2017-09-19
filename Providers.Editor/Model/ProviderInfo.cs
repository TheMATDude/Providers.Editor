// The MIT License(MIT)
//
// Copyright(c) 2017 Microsoft Corporation. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
// associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR 
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Xml.Linq;

namespace Multilingual.Utilities.Providers.Editor.Model
{
    /// <summary>
    /// Class to manage the provider details
    /// </summary>
    public class ProviderInfo
    {
        /// <summary>
        /// Constructor that populates the members based on the provider's XML node
        /// </summary>
        /// <param name="providerNode">Provider's XML Node</param>
        public ProviderInfo(XElement providerNode)
        {
            Id = providerNode?.Element("ID")?.Value.Trim();
            if (string.IsNullOrWhiteSpace(Id))
                throw new KeyNotFoundException("Provider ID node was not found");

            Name = providerNode?.Element("Name")?.Value.Trim();
            if (string.IsNullOrWhiteSpace(Id))
                throw new KeyNotFoundException("Provider Name node was not found");

            ConfigFile = providerNode?.Element("ConfigFile")?.Value.Trim();
            if (string.IsNullOrWhiteSpace(Id))
                throw new KeyNotFoundException("Provider ConfigFile node was not found");

            AssemblyPath = providerNode?.Element("AssemblyPath")?.Value.Trim();
            if (string.IsNullOrWhiteSpace(Id))
                throw new KeyNotFoundException("Provider AssemblyPath node was not found");
        }

        /// <summary>
        /// Constructs a provider's XML Node object
        /// </summary>
        /// <returns><see cref="XElement" /> representing the ProviderInfo in XML</returns>
        internal XElement GetProviderXmlNode()
        {
            return new XElement("Provider",
                new XElement("ID", Id),
                new XElement("Name", Name),
                new XElement("ConfigFile", ConfigFile),
                new XElement("AssemblyPath", AssemblyPath)
            );
        }

        /// <summary>
        /// Provider Unique ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Provider internal name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Provider configuration file (optional)
        /// </summary>
        public string ConfigFile { get; set; }

        /// <summary>
        /// Provider configuration Path to provider's physical location
        /// </summary>
        public string AssemblyPath { get; set; }
    }
}
