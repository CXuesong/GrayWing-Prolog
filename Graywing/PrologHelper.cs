using System;
using System.Collections.Generic;
using System.Text;

namespace Graywing
{
    public static class PrologHelper
    {

        public static string EscapeAtom(string identifier)
        {
            return identifier.Replace("'", "''");
        }

        public static string AtomExpr(string identifier)
        {
            return "'" + EscapeAtom(identifier) + "'";
        }

    }
}
