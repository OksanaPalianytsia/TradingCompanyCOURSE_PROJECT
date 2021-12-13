﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM.Command
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}
