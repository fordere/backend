﻿using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Final
{
    [Route("/finalday/tables/{Id}/disabled", "POST", Summary = "Update the disabled state of a FinalDay table")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UpdateFinalDayTableDisabledRequest : IReturnVoid
    {
        public int Id { get; set; }

        public bool Disabled { get; set; }
    }
}