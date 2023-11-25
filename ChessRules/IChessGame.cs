using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace ChessRules
{
    public interface IChessGame
    {
        IEnumerable<Color> GetPlayingColors();
        Color GetActiveColor();
        IEnumerable<IMove> GetAvailableMoves();
        void ApplyMove(IMove move);
        int GetMaxWidth();
        int GetMaxHeight();
        IEnumerable<(Square, Piece)> GetBoardView();
        GameStatus GetGameStatus();
        bool IsKingUnderCheck(Color kingsColor);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ChessVariantAttribute : Attribute
    {
        public string Name { get; private set; }

        public ChessVariantAttribute(string name)
        {
            Name = name;
        }
    }

    public static class ChessVariants
    {
        private static readonly Dictionary<string, ConstructorInfo> _variants;

        static ChessVariants()
        {
            _variants = new Dictionary<string, ConstructorInfo>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    ChessVariantAttribute attribute = type.GetCustomAttribute(typeof(ChessVariantAttribute)) as ChessVariantAttribute;
                    if (attribute != null)
                    {
                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                        if (type.GetInterface(nameof(IChessGame)) != null && constructor != null)
                        {
                            _variants[attribute.Name] = constructor;
                        }
                    }
                }
            }
        }

        public static string[] GetAllVariantsNames()
        {
            return _variants.Keys.ToArray();
        }

        public static IChessGame CreateGameByVariantName(string variantName)
        {
            if (_variants.ContainsKey(variantName))
            {
                return _variants[variantName].Invoke(new object[0]) as IChessGame;
            }
            throw new ArgumentException("Couldn't find a chess game variant with that name: " + variantName);
        }
    }
}
