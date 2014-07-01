using System;

namespace Mantle.Extensions
{
    public static class ActionExtensions
    {
        public static bool RaiseSafely(this Action action)
        {
            if (action == null)
                return false;

            action();

            return true;
        }

        public static bool RaiseSafely<T1>(this Action<T1> action, T1 parameter1)
        {
            if (action == null)
                return false;

            action(parameter1);

            return true;
        }

        public static bool RaiseSafely<T1, T2>(this Action<T1, T2> action, T1 parameter1, T2 parameter2)
        {
            if (action == null)
                return false;

            action(parameter1, parameter2);

            return true;
        }

        public static bool RaiseSafely<T1, T2, T3>(this Action<T1, T2, T3> action, T1 parameter1, T2 parameter2,
                                                   T3 parameter3)
        {
            if (action == null)
                return false;

            action(parameter1, parameter2, parameter3);

            return true;
        }

        public static bool RaiseSafely<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 parameter1, T2 parameter2,
                                                       T3 parameter3, T4 parameter4)
        {
            if (action == null)
                return false;

            action(parameter1, parameter2, parameter3, parameter4);

            return true;
        }
    }
}