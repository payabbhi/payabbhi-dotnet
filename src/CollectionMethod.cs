using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
    public class CollectionMethod : PayabbhiEntity
    {
        public BankAccount BankAccount { get; set; }
    }
}