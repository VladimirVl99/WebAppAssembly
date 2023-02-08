﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppAssembly.Shared.Entities.Api.Common.General.Terminals
{
    /// <summary>
    /// Contains information about the delivery terminal.
    /// </summary>
    [JsonObject]
    public class DeliveryTerminal
    {
        /// <summary>
        /// ID.
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>
        /// Terminal name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; } = default!;
    }
}