﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data.Blocks
{
    public class BlockState
    {
        public string Key { get; private set; }
        public object Value { get; private set; }

        public bool WrapValueInQuotes { get; set; }
        public string KeyWrap { get; set; }

        public BlockState(string key, string value, bool wrapValueInQuotes, char keyWrapChar) : this(key, value, wrapValueInQuotes)
        {
            KeyWrap = keyWrapChar.ToString();
        }

        public BlockState(string key, string value, bool wrapValueInQuotes)
        {
            Key = key;
            Value = value;
            WrapValueInQuotes = wrapValueInQuotes;
            KeyWrap = "";
        }

        public BlockState(string key, string value)
        {
            Key = key;
            Value = value;
            KeyWrap = "";
        }

        public BlockState(string key, BlockState value)
        {
            Key = key;
            Value = value;
            KeyWrap = "";
        }

        public override string ToString()
        {
            string valueString = "";
            Type valueType = Value.GetType();
            if (valueType == typeof(string))
            {
                if (WrapValueInQuotes)
                {
                    valueString = $"\"{Value}\"";
                }
                else
                {
                    valueString = $"{Value}";
                }
            }
            else
            {
                valueString = $"'{{{Value}}}'";
            }
            return $"{KeyWrap}{Key}{KeyWrap}:{valueString}";
        }
    }
}
