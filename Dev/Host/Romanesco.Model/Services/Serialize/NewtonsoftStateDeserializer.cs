﻿using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace Romanesco.Model.Services.Serialize
{
	internal class NewtonsoftStateDeserializer : IStateDeserializer
    {
        public object? Deserialize(JObject encoded, Type type)
		{
			return JsonConvert.DeserializeObject(encoded.ToString(), type);
		}
    }
}
