using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    // interface to wrap your actions in a "command object"
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}