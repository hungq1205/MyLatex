using System.Collections.Generic;
using System.Reflection;
using System;

namespace Latex
{
    public static class Utility
    {
        public const char EscapeChar = '\\';

        public static readonly HashSet<char> quickParamNotifiers = new(new[] { '^', '_' });

        private static Dictionary<string, ConstructorInfo> expressionConstructors;
        public static Dictionary<string, ConstructorInfo> ExpressionConstructors
        {
            get
            {
                if (expressionConstructors == null)
                    Init();
                return expressionConstructors;
            }
        }

        private static HashSet<char> notifiers;
        public static HashSet<char> Notifiers
        {
            get
            {
                if (notifiers == null)
                    Init();
                return notifiers;
            }
        }

        public static IExpression InstantiateExpression(string latexName, IExpression[] content)
        {
            return (IExpression)ExpressionConstructors[latexName].Invoke(new[] { content });
        }

        public static void Init()
        {
            notifiers = new HashSet<char>();
            expressionConstructors = new Dictionary<string, ConstructorInfo>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var type in assembly.GetTypes())
                {
                    var attr = type.GetCustomAttribute<LatexExpressionAttribute>(true);
                    if (attr != null)
                    {
                        notifiers.Add(attr.LatexID[0]);
                        var constructor = type.GetConstructor(new Type[] { typeof(IExpression[]) });
                        expressionConstructors.Add(attr.LatexID, constructor);
                    }
                }
        }
    }
}