

using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octolamp.Contracts.Extensions
{
    public static class MetadataExtensions
    {
        public static string GetString(this Metadata metadata, string key, string defaultValue = "UNKNOWN")
        {
            return metadata.FirstOrDefault(e => e.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value ?? defaultValue;
        }
    }
}
