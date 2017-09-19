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
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Multilingual.Utilities.Providers.Editor.Model;

namespace Multilingual.Utilities.Providers.Editor
{
    /// <summary>
    /// Enabled removing and sorting of the Multilingual App Toolkit's configured providers list
    /// </summary>
    /// <remarks>
    /// This is targeting at Visual Studio 2015 / 2013 installations and modifies the list of available providers for both 
    /// the Multilingual Editor and the Multilingual App toolkit extension for Visual Studio.
    /// 
    /// Note: The same code will work with Multilingual App toolkit extension for Visual Studio 2017 - if the correct 
    /// configuration path is provided.
    /// </remarks>
    internal class ProvidersConfig
    {
        /// <summary>
        /// Loads the list of active providers for the Multilingual App toolkit
        /// </summary>
        /// <param name="configFile">Provider configuration file</param>
        /// <returns>List of <see cref="ProviderInfo"/></returns>
        internal static List<ProviderInfo> Load(string configFile)
        {
            var doc = XDocument.Load(configFile);
            var providers = doc.Descendants("Provider");
            return providers.Select(provider => new ProviderInfo(provider)).ToList();
        }

        /// <summary>
        /// Loads the list of active providers for the Multilingual App toolkit
        /// </summary>
        /// <param name="configFile">Provider configuration file</param>
        /// <param name="providers">List of <see cref="ProviderInfo"/> to make active</param>
        internal static void Save(string configFile, List<ProviderInfo> providers)
        {
            var doc = XDocument.Load(configFile);

            // Remove all existing providers
            doc.Descendants("Provider").Remove();

            // Replace with new list of providers
            var providersNode = doc.Descendants("Providers").FirstOrDefault();
            if (providersNode == null)
            {
                throw new XmlException("<Providers> node was not found.");
            }

            foreach (var provider in providers)
            {
                providersNode.Add(provider.GetProviderXmlNode());
            }

            doc.Save(configFile);
        }
    }
}
