﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.HashFunction
{
    public class MurmurHash1
        : HashFunctionBase
    {
        public override IEnumerable<int> ValidHashSizes
        {
            get { return new[] { 32 }; }
        }

        public UInt32 Seed { get; set; }


        public MurmurHash1()
            : base(32)
        {
            Seed = 0;
        }


        public override byte[] ComputeHash(byte[] data)
        {
            if (HashSize != 32)
                throw new ArgumentOutOfRangeException("HashSize");

            const UInt32 m = 0XC6A4A793;

            UInt32 h = Seed ^ ((UInt32) data.Length * m);

            //----------
            
            for (int x = 0; x < data.Length / 4; ++x)
            {
                h += BitConverter.ToUInt32(data, x * 4);
                h *= m;
                h ^= h >> 16;
            }
            
            //----------

            var remainderStartIndex = data.Length - (data.Length % 4);

            switch(data.Length % 4)
            {
                case 3: h += (UInt32) data[remainderStartIndex + 2] << 16;  goto case 2;
                case 2: h += (UInt32) data[remainderStartIndex + 1] <<  8;  goto case 1;
                case 1:
                    h += (UInt32) data[remainderStartIndex];
                    h *= m;
                    h ^= h >> 16;
                    break;
            };
 
            //----------

            h *= m;
            h ^= h >> 10;
            h *= m;
            h ^= h >> 17;

            return BitConverter.GetBytes(h);
        }
    }
}
