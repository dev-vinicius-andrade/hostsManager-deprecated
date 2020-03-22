using System;
using System.Collections.Generic;

using System.Text.Json;
using HostsManager.Services.Entities;

namespace HostsManager.Services.Helpers
{
    public static class Validator
    {
        public static bool Equals(this KeyValuePair<string, Profile> profile, string jsonProfile)
        => JsonSerializer.Serialize(profile).Equals(JsonSerializer.Serialize(jsonProfile),
                StringComparison.InvariantCultureIgnoreCase);


    }
}
