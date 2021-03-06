﻿using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Table
{
    [Route("/tables/{TableId}", "GET", Summary = "Gets a single table")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GetTableByIdRequest : IReturn<TableDto>
    {
        public int TableId { get; set; }
    }
}