// Project: WorldCreatorBridge
// Filename: UnityTerrainUtility.cs
// Copyright (c) 2025 BiteTheBytes GmbH. All rights reserved
// *********************************************************

using System.Xml;

namespace BtB.WC.Bridge
{
    public static class XML
    {
        public static string GetAttrib(XmlElement xml, string key, string def = "")
        {
            if (!xml.HasAttribute(key)) return def;

            return xml.Attributes[key].Value;
        }
    }
}