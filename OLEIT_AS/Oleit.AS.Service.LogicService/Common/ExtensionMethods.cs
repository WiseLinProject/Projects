using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oleit.AS.Service.LogicService
{
    public static class ExtensionMethods
    {
        public static double ExtRound(this double source, int digits = 0)
        {
            return Math.Round(source, digits, MidpointRounding.AwayFromZero);
        }

        public static decimal ExtRound(this decimal source, int digits = 0)
        {
            return Math.Round(source, digits, MidpointRounding.AwayFromZero);
        }
    }
}