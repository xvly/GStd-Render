namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Assertions;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ShaderKeywords : IEnumerable<int>, IEquatable<ShaderKeywords>, IEnumerable
    {
        private const int int_0 = 0x20;
        private static readonly string[] string_0;
        [SerializeField]
        private int keywords;
        static ShaderKeywords()
        {
            string_0 = new string[0x20];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Struct7(this.keywords);
        }

        public bool IsEmpty
        {
            get
            {
                return (this.keywords == 0);
            }
        }
        public static void SetKeywordName(int keyword, string name)
        {
            Assert.IsTrue((keyword >= 0) && (keyword < 0x20));
            string_0[keyword] = name;
        }

        public static string GetKeywordName(int keyword)
        {
            Assert.IsTrue((keyword >= 0) && (keyword < 0x20));
            return string_0[keyword];
        }

        public void SetKeyword(int keyword)
        {
            Assert.IsTrue((keyword >= 0) && (keyword < 0x20));
            this.keywords |= ((int) 1) << keyword;
        }

        public void UnsetKeyword(int keyword)
        {
            Assert.IsTrue((keyword >= 0) && (keyword < 0x20));
            this.keywords &= ~(((int) 1) << keyword);
        }

        public void ToggleKeyword(int keyword)
        {
            Assert.IsTrue((keyword >= 0) && (keyword < 0x20));
            this.keywords ^= ((int) 1) << keyword;
        }

        public void Merge(ShaderKeywords keywords)
        {
            this.keywords |= keywords.keywords;
        }

        public bool HasKeyword(int keyword)
        {
            Assert.IsTrue((keyword > 0) && (keyword < 0x20));
            return ((this.keywords & (((int) 1) << keyword)) != 0);
        }

        public bool Equals(ShaderKeywords other)
        {
            return (this.keywords == other.keywords);
        }

        public override int GetHashCode()
        {
            return this.keywords.GetHashCode();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new Struct7(this.keywords);
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct Struct7 : IEnumerator, IDisposable, IEnumerator<int>
        {
            private int int_0;
            private int int_1;
            public Struct7(int int_2)
            {
                this.int_0 = int_2;
                this.int_1 = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.int_1;
                }
            }
            public int Current
            {
                get
                {
                    return this.int_1;
                }
            }
            public bool MoveNext()
            {
                this.int_1++;
                int num = 0x20;
                for (int i = this.int_1; i < num; i++)
                {
                    if ((this.int_0 & (((int) 1) << i)) != 0)
                    {
                        this.int_1 = i;
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                this.int_1 = -1;
            }

            public void Dispose()
            {
            }
        }
    }
}

