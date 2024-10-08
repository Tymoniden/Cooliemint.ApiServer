﻿using System.Data.Common;

namespace Cooliemint.ApiServer.Models
{
    public class ContactProvider
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public ContactProviderModelType Type { get; set; }
        public required string Configuration { get; set; }
    }
}