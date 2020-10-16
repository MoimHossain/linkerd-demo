

using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Octolamp.Contracts.Extensions
{
    public static class PrimitiveTypeExtensions
    {
        public static Timestamp ToProtoDateTime(this DateTime dt)
        {
            return Timestamp.FromDateTime(DateTime.SpecifyKind(dt, DateTimeKind.Utc));
        }
    }
}
