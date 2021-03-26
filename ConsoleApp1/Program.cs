using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace ConsoleApp1 {
    class Program {
        private static List<string> openedBraces = new() {
            "(",
            "[",
            "{",
        };

        private static List<string> closedBraces = new() {
            ")",
            "]",
            "}",
        };

        enum Mode {
            Opened,
            Closed,
        }

        enum BraceType {
            Round,
            Rect,
            Fig,
        }

        static List<int> _closeFact = new();

        static void Main(string[] args) {
            while (true) {
                var braces = Console.ReadLine();
                Console.WriteLine(ValidateBraceNew(braces));
                _closeFact.Clear();
            }
        }

        static bool ValidateBraceNew(string brace) {
            (string Value, BraceType Type, Mode Mode)[] array = brace.ToList().Select(x => (x.ToString(), GetType(x.ToString()), GetMode(x.ToString())))
                .ToArray();

            for (var i = 0; i < array.Length; i++) {
                var x = array[i];

                if (i > 0) {
                    if (x.Mode == Mode.Closed) {
                        var closed = TryClose(array, i, x);

                        if (!closed) {
                            return false;
                        }
                    }
                }
            }

            if (HasNoClosed(array)) {
                return false;
            }

            return true;
        }

        static bool HasNoClosed((string Value, BraceType Type, Mode Mode)[] array) {
            var closeBraces = array.Where(x => GetMode(x.Value) == Mode.Opened).ToArray();
            return closeBraces.Length != _closeFact.Count;
        }

        static bool TryClose((string Value, BraceType Type, Mode Mode)[] array, int i,
            (string Value, BraceType Type, Mode Mode) x) {
            for (var q = i - 1; q > -1; q--) {
                var prevBrace = array[q];
                var inMode = prevBrace.Mode == x.Mode;
                var inType = prevBrace.Type == x.Type;

                if (inType && !inMode && !_closeFact.Contains(q)) {
                    _closeFact.Add(q);
                    return true;
                }
            }

            return false;
        }

        static Mode GetMode(string brace) {
            if (openedBraces.Contains(brace)) {
                return Mode.Opened;
            }

            if (closedBraces.Contains(brace)) {
                return Mode.Closed;
            }

            throw new Exception("wait what?");
        }

        static BraceType GetType(string brace) {
            if (brace is "(" or ")") return BraceType.Round;
            if (brace is "[" or "]") return BraceType.Rect;
            if (brace is "{" or "}") return BraceType.Fig;

            throw new Exception("wait what?");
        }
    }
}